using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace uetquizing.Models
{
    public class VariationsController : Controller
    {
        uetquizingEntities db = new uetquizingEntities();
        private static List<int> quizQuestionIds;
        private static List<int> addedQuestions;
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

        public ActionResult Edit(int? id)
        {

            quizQuestionIds = new List<int>();
            addedQuestions = new List<int>();
            try
            {
                var variation = db.QuizVariations.Where(x => x.variation_id == id).Single();
                var quiz = db.quizzes.Where(x => x.quiz_id == variation.quiz_id).Single();
                ViewBag.VID = id;
                ViewBag.QuizTitle = quiz.quiz_title;
                ViewBag.VariatonTitle = variation.variation_title;
                ViewBag.MarksPerQues = quiz.marks_per_question;

                VariationViewModel v = new VariationViewModel();
                v.VarianceTitle = variation.variation_title;
                v.VarianceID = variation.variation_id;
                return View(v);
            }
            catch (Exception e)
            {
                TempData["Error"] = "Something Went Wrong, Try Again";
            }
            return Redirect("Index");
        }

        [HttpPost]
        public ActionResult Edit(int? id, VariationViewModel collection)
        {
            try
            {
                var variation = db.QuizVariations.Where(x => x.variation_id == id).Single();
                variation.variation_title = collection.VarianceTitle;

                if (quizQuestionIds != null)
                {
                    foreach (var item in quizQuestionIds)
                    {
                        var qq = db.quizQuestions.Where(x => x.id == item).Single();
                        db.quizQuestions.Remove(qq);
                        db.SaveChanges();
                    }
                }

                if (addedQuestions != null)
                {
                    foreach(var item in addedQuestions)
                    {
                        quizQuestion q = new quizQuestion();
                        q.variation_id = collection.VarianceID;
                        q.question_id = item;
                        db.quizQuestions.Add(q);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index/" + collection.VarianceID);
            }
            catch (Exception e)
            {
                TempData["Error"] = "Something Went Wrong, Try Again";
            }
            return RedirectToAction("Index");
        }
        
        public JsonResult getAllQuestions()
        {
            var items = db.questions.ToList();
            List<QuestionViewModel> questions = new List<QuestionViewModel>();
            foreach(var item in items)
            {
                QuestionViewModel question = new QuestionViewModel();
                question.question_id = item.question_id;
                question.question_title = item.question_title;
                questions.Add(question);
            }
            return Json(questions, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getQuestionsData(int? id)
        {
            var items = db.quizQuestions.Where(x => x.variation_id == id).ToList();
            List<EditVariationViewModel> questions = new List<EditVariationViewModel> ();
            foreach (var q in items)
            {
                var found = 0;
                if (quizQuestionIds != null)
                {
                    foreach (var item in quizQuestionIds)
                    {
                        if (q.id == item)
                        {
                            found = 1;
                        }
                    }
                }

                if (found == 0)
                {
                    question a = db.questions.Where(x => x.question_id == q.question_id).Single();
                    EditVariationViewModel ed = new EditVariationViewModel();
                    ed.questionTitle = a.question_title;
                    ed.quizQuestionID = q.id;
                    ed.questionID = Convert.ToInt32(q.question_id);
                    questions.Add(ed);
                }
            }

            if(addedQuestions != null)
            {
                foreach(var item in addedQuestions)
                {
                    question a = db.questions.Where(x => x.question_id == item).Single();
                    EditVariationViewModel ed = new EditVariationViewModel();
                    ed.questionTitle = a.question_title;
                    ed.questionID = item;
                    questions.Add(ed);
                }
            }
            return Json(questions, JsonRequestBehavior.AllowGet);
        }
        

        public JsonResult deleteQuestion(int id)
        {
            try{
                quizQuestionIds.Add(id);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult addQuestionToQuiz(int id)
        {
            try
            {
                addedQuestions.Add(id);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
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