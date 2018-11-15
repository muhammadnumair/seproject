using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Web.Mvc;
using uetquizing.Models;

namespace uetquizing.Controllers
{
    public class DashboardController : Controller
    {
        uetquizingEntities db = new uetquizingEntities();
        // GET: Dashboard
        public ActionResult Index()
        {
            // FOR VALIDATION
            var userid = User.Identity.GetUserId();
            var user = db.AspNetUsers.Where(x => x.Id == userid).SingleOrDefault();
            if (user != null)
            {
                var role = user.userRole;
                if (role == "Student")
                {
                    return RedirectToAction("Index", "User");
                }
            }
            // END VALIDATIONS

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            // Getting Dashboard Values From Database
            var questions = db.questions.Where(x => x.teacher_id == userid).ToList();
            var quizzes = db.quizzes.Where(x => x.teacher_id == userid).ToList();
            var total_variations = 0;
            foreach(var quiz in quizzes)
            {
                var variations = db.QuizVariations.Where(x => x.quiz_id == quiz.quiz_id).ToList();
                total_variations += variations.Count;
            }

            var totalQuizAttemptions = 0;
            foreach (var quiz in quizzes)
            {
                var student_quizzes = db.studentQuizzes.Where(h => h.quiz_id == quiz.quiz_id).ToList();
                totalQuizAttemptions += student_quizzes.Count;
            }

            // Storing Values In ViewBag
            ViewBag.questions = questions.Count;
            ViewBag.quizzes = quizzes.Count;
            ViewBag.variations = total_variations;
            ViewBag.totalQuizzes = totalQuizAttemptions;

            return View();
        }
    }
}