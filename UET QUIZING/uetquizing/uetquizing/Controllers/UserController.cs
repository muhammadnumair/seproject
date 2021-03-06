﻿using System;
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
            // FOR VALIDATION
            var userid = User.Identity.GetUserId();
            var user = db.AspNetUsers.Where(x => x.Id == userid).SingleOrDefault();
            if (user != null)
            {
                var role = user.userRole;
                if (role == "Teacher")
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                // END VALIDATIONS
            }

            // Getting Student Quizes
            var user_id = User.Identity.GetUserId();
            var student_quizes = db.studentQuizzes.Where(x => x.student_id == user_id).ToList();

            var quizes = new List<StudentQuiz>();
            // Fetching Quizes Data
            if(student_quizes != null)
            {
                foreach(var item in student_quizes)
                {
                    StudentQuiz s = new StudentQuiz();
                    s.attempted_on = item.attempted_on;
                    s.studentQuiz = db.quizzes.Where(x => x.quiz_id == item.quiz_id).SingleOrDefault();
                    quizes.Add(s);
                }
            }
            return View(quizes);
        }

        public ActionResult Quiz(int? id)
        {
            // FOR VALIDATION
            var userid = User.Identity.GetUserId();
            var user = db.AspNetUsers.Where(x => x.Id == userid).SingleOrDefault();
            if (user != null)
            {
                var role = user.userRole;
                if (role == "Teacher")
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            // END VALIDATIONS
            try
            {
                var quiz = db.quizzes.Where(x => x.quiz_id == id).SingleOrDefault();
                if (quiz != null)
                {
                    var student_quiz = db.studentQuizzes.Where(x => x.quiz_id == id).Where(x => x.student_id == userid).SingleOrDefault();
                    if (student_quiz == null)
                    {
                        if (quiz.status == 1)
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
                            foreach (var que in quizQuestions)
                            {
                                var question = db.questions.Where(x => x.question_id == que.question_id).SingleOrDefault();
                                questions.Add(question);
                            }

                            // Sending Data To View
                            ViewBag.QuizID = id;
                            ViewBag.VariationID = choosed_variation.variation_id;
                            ViewBag.questions = questions;
                        }

                        ViewBag.status = quiz.status;
                        ViewBag.quiz_id = quiz.quiz_id;
                        return View();
                    }
                    else
                    {
                        TempData["Error"] = "You Have Already Attempted The Quiz, Please Contact To Your Respected Teacher";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["Error"] = "Your Quiz ID is incorrect, Please Enter a valid ID.";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch(Exception e)
            {
                TempData["Error"] = "Something Went Wrong With Your Quiz ID, Please Enter Valid ID";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult SubmitQuiz(QuizQuestions collection)
        {
            double? obtained_marks = 0;
            foreach(var que in collection.Questions)
            {
                studentMark questionMarks = new studentMark();
                questionMarks.choosed_option = que.selectedOption;
                questionMarks.quiz_id = collection.quizID;
                questionMarks.student_id = User.Identity.GetUserId();
                questionMarks.variation_id = collection.variations_id;
                questionMarks.quizQuestionID = db.quizQuestions.Where(x => x.variation_id == collection.variations_id).Where(x => x.question_id == que.questionID).Single().id;
                questionMarks.question_id = que.questionID;

                var question_id = que.questionID;
                var choosed_answer = que.selectedOption;

                var question = db.questions.Where(x => x.question_id == question_id).Single();
                if(question.correct_answer == choosed_answer)
                {
                    questionMarks.correct = 1;
                    obtained_marks += db.quizzes.Where(x => x.quiz_id == collection.quizID).SingleOrDefault().marks_per_question;
                }
                else
                {
                    questionMarks.correct = 0;
                }
                db.studentMarks.Add(questionMarks);
            }
            db.SaveChanges();


            studentQuizze quiz = new studentQuizze();
            quiz.student_id = User.Identity.GetUserId();
            quiz.quiz_id = collection.quizID;
            quiz.attempted_on = DateTime.Now;
            quiz.marks = obtained_marks;
            db.studentQuizzes.Add(quiz);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult check_status(int id)
        {
            var quiz = db.quizzes.Where(x => x.quiz_id == id).Single();
            var status = quiz.status;
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Result(int id)
        {
            // FOR VALIDATION
            var userid = User.Identity.GetUserId();
            var user = db.AspNetUsers.Where(x => x.Id == userid).SingleOrDefault();
            if (user != null)
            {
                var role = user.userRole;
                if (role == "Teacher")
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            // END VALIDATIONS

            string token = User.Identity.GetUserId();
            var student_data = db.AspNetUsers.Where(x => x.Id == token).SingleOrDefault();
            var studentQuiz = db.studentQuizzes.Where(x => x.quiz_id == id).Where(x => x.student_id == token).SingleOrDefault();
            var quiz = db.quizzes.Where(x => x.quiz_id == id).SingleOrDefault();
            var studentAnswers = db.studentMarks.Where(x => x.quiz_id == id).Where(x => x.student_id == token).ToList();
            int? vid = 0;
            if (studentAnswers != null)
            {
                vid = studentAnswers[0].variation_id;
            }
            var variationQuestions = db.quizQuestions.Where(x => x.variation_id == vid).ToList();

            var quizQuestions = new List<question>();
            foreach (var que in variationQuestions)
            {
                var question = db.questions.Where(x => x.question_id == que.question_id).SingleOrDefault();
                quizQuestions.Add(question);
            }

            StudentQuiz s = new StudentQuiz();
            s.token = token;
            s.StudentName = student_data.fullName;
            s.studentRollNo = "2016-CS-213";
            s.obtainedMarks = studentQuiz.marks;
            s.attempted_on = studentQuiz.attempted_on;
            s.QuizQuestions = quizQuestions;
            s.StudentAnswers = studentAnswers;
            s.studentQuiz = quiz;
            return View(s);
        }
    }
}