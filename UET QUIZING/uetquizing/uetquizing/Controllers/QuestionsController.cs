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
    public class QuestionsController : Controller
    {
        uetquizingEntities db = new uetquizingEntities();
        // GET: Questions
        public ActionResult Index()
        {
            // FOR VALIDATION
            var userid = User.Identity.GetUserId();
            var user = db.AspNetUsers.Where(x => x.Id == userid).SingleOrDefault();
            var role = user.userRole;
            if (role == "Student")
            {
                return RedirectToAction("Index", "User");
            }
            // END VALIDATIONS

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = User.Identity.GetUserId();
            var questions = db.questions.Where(x => x.teacher_id == userId).ToList();
            return View(questions);
        }

        public ActionResult Add()
        {
            // FOR VALIDATION
            var userid = User.Identity.GetUserId();
            var user = db.AspNetUsers.Where(x => x.Id == userid).SingleOrDefault();
            var role = user.userRole;
            if (role == "Student")
            {
                return RedirectToAction("Index", "User");
            }
            // END VALIDATIONS

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            List<category> categories = db.categories.ToList();
            ViewBag.categories = categories;
            return View();

        }

        [HttpPost]
        public ActionResult Add(QuestionViewModel collection)
        {
            question q = new question();
            q.teacher_id = User.Identity.GetUserId();
            q.question_title = collection.question_title;
            q.option_1 = collection.option_1;
            q.option_2 = collection.option_2;
            q.option_3 = collection.option_3;
            q.option_4 = collection.option_4;
            q.correct_answer = collection.correct_answer;
            q.category_id = collection.catgory_id;
            q.added_on = DateTime.Now;
            db.questions.Add(q);
            int result = db.SaveChanges();
            if(result > 0)
            {
                TempData["Success"] = "Record Added Successfully";
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Delete(int? id)
        {
            try
            {
                var question = db.questions.Where(x => x.question_id == id).SingleOrDefault();
                var result = db.questions.Remove(question);
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

        public ActionResult Edit(int? id)
        {
            // FOR VALIDATION
            var userid = User.Identity.GetUserId();
            var user = db.AspNetUsers.Where(x => x.Id == userid).SingleOrDefault();
            var role = user.userRole;
            if (role == "Student")
            {
                return RedirectToAction("Index", "User");
            }
            // END VALIDATIONS

            try
            {
                var que = db.questions.Where(x => x.question_id == id).SingleOrDefault();
                QuestionViewModel question = new QuestionViewModel();
                question.question_id = que.question_id;
                question.question_title = que.question_title;
                question.option_1 = que.option_1;
                question.option_2 = que.option_2;
                question.option_3 = que.option_3;
                question.option_4 = que.option_4;
                question.correct_answer = que.correct_answer;
                question.catgory_id = Convert.ToInt32(que.category_id);
                ViewBag.Type = "Edit";
                return View("Add", question);
            }
            catch (Exception e)
            {
                TempData["Error"] = "Please Enter Valid Id";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(int? id, QuestionViewModel collection)
        {
            try
            {
                var question = db.questions.Where(x => x.question_id == id).SingleOrDefault();
                question.question_title = collection.question_title;
                question.option_1 = collection.option_1;
                question.option_2 = collection.option_2;
                question.option_3 = collection.option_3;
                question.option_4 = collection.option_4;
                question.correct_answer = collection.correct_answer;
                question.category_id = collection.catgory_id;

                var result = db.SaveChanges();
                if (result > 0)
                {
                    TempData["Success"] = "Record Updated Successfully";
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