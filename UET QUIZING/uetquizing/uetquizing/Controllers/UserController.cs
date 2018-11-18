using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using uetquizing.Models;

namespace uetquizing.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public ActionResult addQuestion()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public ActionResult Questions()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public ActionResult addCategory()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public ActionResult Categories()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public ActionResult addQuiz()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            uetquizingEntities db = new uetquizingEntities();
            List<category> categories = db.categories.ToList();
            ViewBag.categories = categories;
            return View();
        }

        [HttpPost]
        public ActionResult addQuiz(QuizViewModel collection)
        {
            try
            {
                int categoryId = collection.questionsCategory;
                int num_of_questions = collection.totalQuestions;
                string quizType = collection.quizType;
                uetquizingEntities db = new uetquizingEntities();

                // GENERATING RANDOM QUESTIONS
                Random rand = new Random();
                int count = db.questions.Count();
                int toSkip = rand.Next(0, count);
                IQueryable<question> questions = null;
                if(quizType == "random")
                {
                    if (categoryId > 0)
                    {
                        questions = db.questions.Where(x => x.category_id == categoryId).OrderBy(r => Guid.NewGuid()).Skip(toSkip).Take(num_of_questions);
                    }
                    else
                    {
                        questions = db.questions.OrderBy(r => Guid.NewGuid()).Skip(toSkip).Take(num_of_questions);
                    }
                }else if(quizType == "manual")
                {

                }

                // Creating Quiz
                quizze quiz = new quizze();
                quiz.quiz_title = collection.quizTitle;
                quiz.total_marks = collection.totalMarks;
                quiz.total_questions = collection.totalQuestions;
                quiz.marks_per_question = collection.marksPerQuestion;
                quiz.teacher_id = User.Identity.GetUserId();

                db.quizzes.Add(quiz);
                db.SaveChanges();

                // Adding Quiz Questions To Table
                if (questions != null)
                {
                    foreach (question que in questions)
                    {
                        quizQuestion qq = new quizQuestion();
                        qq.quiz_id = quiz.quiz_id;
                        qq.question_id = que.question_id;
                        db.quizQuestions.Add(qq);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {

            }
            return View();
        }

        public ActionResult Quizes()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

    }
}