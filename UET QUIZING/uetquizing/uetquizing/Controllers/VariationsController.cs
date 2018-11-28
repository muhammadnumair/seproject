using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Web.Mvc;

namespace uetquizing.Models
{
    public class VariationsController : Controller
    {
        uetquizingEntities db = new uetquizingEntities();
        // GET: Variances
        public ActionResult Index(int? id)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Account");
                }
                var userId = User.Identity.GetUserId();
                var quiz = db.quizzes.Where(x => x.quiz_id == id).SingleOrDefault();
                if (quiz != null)
                {
                    var variations = db.QuizVariations.Where(x => x.quiz_id == id).ToList();
                    return View(variations);
                }
                else
                {
                    TempData["Error"] = "No Quiz With This Id";
                    return RedirectToAction("Index", "Quizzes");
                }
            }catch(Exception e)
            {
                TempData["Error"] = "Something Wrong With Variation";
                return RedirectToAction("Index", "Quizzes");
            }
        }

        public ActionResult Details(int? id)
        {
            try
            {
                var items = db.quizQuestions.Where(x => x.variation_id == id).ToList();
                List<question> questions = new List<question>();
                foreach (var q in items)
                {
                    question a = db.questions.Where(x => x.question_id == q.question_id).Single();
                    if (a != null)
                    {
                        questions.Add(a);
                    }
                }
                var variation = db.QuizVariations.Where(x => x.variation_id == id).Single();
                var quiz = db.quizzes.Where(x => x.quiz_id == variation.quiz_id).Single();
                ViewBag.VID = id;
                ViewBag.QuizTitle = quiz.quiz_title;
                ViewBag.VariatonTitle = variation.variation_title;
                ViewBag.MarksPerQues = quiz.marks_per_question;

                return View(questions);
            }catch(Exception e)
            {
                TempData["Error"] = "Something Went Wrong, Try Again";
            }
            return Redirect("Index");
        }

        public ActionResult Delete(int? id)
        {
            try
            {
                var variation = db.QuizVariations.Where(x => x.variation_id == id).SingleOrDefault();
                var result = db.QuizVariations.Remove(variation);
                db.SaveChanges();
                if (result != null)
                {
                    TempData["Success"] = "Record Deleted Successfully";
                }
                else
                {
                    TempData["Error"] = "Something Went Wrong, Try Again";
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = "Something Went Wrong, Try Again";
            }
            return RedirectToAction("Index");
        }
    }
}