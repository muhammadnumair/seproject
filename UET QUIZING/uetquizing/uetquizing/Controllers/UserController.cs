using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uetquizing.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace uetquizing.Controllers
{
    public class UserController : Controller
    {
        uetquizingEntities db = new uetquizingEntities();
        // GET: User
        public ActionResult Index()
        {
            return Content(Convert.ToString(Session["userRole"]));
            // Getting Student Quizes
            var user_id = User.Identity.GetUserId();
            var student_quizes = db.studentQuizzes.Where(x => x.student_id == user_id).ToList();

            var quizes = new List<quizze>();
            // Fetching Quizes Data
            if(student_quizes != null)
            {
                foreach(var item in student_quizes)
                {
                    var quiz = db.quizzes.Where(x => x.quiz_id == item.quiz_id).SingleOrDefault();
                    quizes.Add(quiz);
                }
            }
            return View(quizes);
        }

        public ActionResult Quiz(int? id)
        {
            var quiz_variations = db.QuizVariations.Where(x => x.quiz_id == id).ToList();

            // Fetching Random Variation
            Random rnd = new Random();

            // Fetching Choosed Variation Questions
            int questions_count = 0;
            var choosed_variation = new QuizVariation();
            var quizQuestions = new List<quizQuestion>();
            while (questions_count == 0)
            {
                int number = rnd.Next(0, quiz_variations.Count);
                choosed_variation = quiz_variations[number];
                quizQuestions = db.quizQuestions.Where(x => x.variation_id == choosed_variation.variation_id).ToList();
                questions_count = quizQuestions.Count;
            }

            var questions = new List<question>();
            foreach(var que in quizQuestions)
            {
                var question = db.questions.Where(x => x.question_id == que.question_id).SingleOrDefault();
                questions.Add(question);
            }

            // Sending Data To View
            ViewBag.QuizID = id;
            ViewBag.VariationID = choosed_variation.variation_id;
            ViewBag.questions = questions;
            return View();
        }

        [HttpPost]
        public ActionResult SubmitQuiz(QuizQuestions collection)
        {
            studentQuizze quiz = new studentQuizze();
            quiz.student_id = User.Identity.GetUserId();
            quiz.quiz_id = collection.quizID;
            db.studentQuizzes.Add(quiz);
            db.SaveChanges();

            foreach(var que in collection.Questions)
            {
                studentMark questionMarks = new studentMark();
                questionMarks.choosed_option = que.selectedOption;
                questionMarks.quiz_id = collection.quizID;
                questionMarks.student_id = User.Identity.GetUserId();
                questionMarks.variation_id = collection.variations_id;
                questionMarks.question_id = db.quizQuestions.Where(x => x.variation_id == collection.variations_id).Where(x => x.question_id == que.questionID).Single().id;

                var question_id = que.questionID;
                var choosed_answer = que.selectedOption;

                var question = db.questions.Where(x => x.question_id == question_id).Single();
                if(question.correct_answer == choosed_answer)
                {
                    questionMarks.correct = 1;
                }
                else
                {
                    questionMarks.correct = 0;
                }
                db.studentMarks.Add(questionMarks);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}