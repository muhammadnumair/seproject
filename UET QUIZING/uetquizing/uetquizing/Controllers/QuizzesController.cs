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
    public class QuizzesController : Controller
    {
        uetquizingEntities db = new uetquizingEntities();
        // GET: Quizzes
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

            var quizzes = db.quizzes.Where(x => x.teacher_id == userId).ToList();
            return View(quizzes);
        }

        public ActionResult StartQuiz(int id)
        {
            var quiz = db.quizzes.Where(x => x.quiz_id == id).SingleOrDefault();
            quiz.status = 1;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult StopQuiz(int id)
        {
            var quiz = db.quizzes.Where(x => x.quiz_id == id).SingleOrDefault();
            quiz.status = 0;
            db.SaveChanges();
            return RedirectToAction("Index");
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
        public ActionResult Add(QuizViewModel collection)
        {
            try
            {
                var a = Request.Form.GetValues("quizTitle");

                int categoryId = collection.questionsCategory;
                int num_of_questions = collection.totalQuestions;
                string quizType = collection.quizType;
                bool haveVaiations = collection.toggleOne;

                int noOfVariations;
                if (haveVaiations)
                {
                    noOfVariations = collection.NoOfVariations;
                }
                else
                {
                    noOfVariations = 1;
                }
                
                // Creating Quiz
                quizze quiz = new quizze();
                quiz.quiz_title = collection.quizTitle;
                quiz.total_marks = collection.totalMarks;
                quiz.total_questions = collection.totalQuestions;
                quiz.marks_per_question = collection.marksPerQuestion;
                quiz.teacher_id = User.Identity.GetUserId();
                quiz.total_marks = collection.marksPerQuestion * collection.totalQuestions;
                quiz.created_on = DateTime.Now;
                quiz.status = 0;
                db.quizzes.Add(quiz);
                int result = db.SaveChanges();

                if(result > 0)
                {
                    char alphabet = 'A';
                    for (int i = 0; i < noOfVariations; i++)
                    {
                        // Variation Creation
                        QuizVariation variation = new QuizVariation();
                        variation.variation_title = collection.quizTitle + " ( " + alphabet + " )";
                        variation.quiz_id = quiz.quiz_id;
                        variation.created_on = DateTime.Now;
                        db.QuizVariations.Add(variation);
                        db.SaveChanges();
                        alphabet++;

                        //Assigning Question To Variation
                        if (quizType == "random")
                        {
                            // GENERATING RANDOM QUESTIONS
                            Random rand = new Random();
                            int count = db.questions.Count();
                            int toSkip = rand.Next(0, count);
                            List<question> questions = null;

                            if (categoryId > 0)
                            {
                                questions = db.questions.Where(x => x.category_id == categoryId).OrderBy(r => Guid.NewGuid()).Skip(toSkip).Take(num_of_questions).ToList();
                            }
                            else
                            {
                                questions = db.questions.OrderBy(r => Guid.NewGuid()).Skip(toSkip).Take(num_of_questions).ToList();
                            }

                           
                            if (questions != null)
                            {
                                foreach (question que in questions)
                                {
                                    if (que.question_title != null)
                                    {
                                        quizQuestion quizq = new quizQuestion();
                                        quizq.variation_id = variation.variation_id;
                                        quizq.question_id = que.question_id;
                                        db.quizQuestions.Add(quizq);
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                    TempData["Success"] = "Record Added Successfully";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {

            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            try
            {
                var variations = db.QuizVariations.Where(x => x.quiz_id == id);
                foreach(var variation  in variations)
                {
                    db.QuizVariations.Remove(variation);
                    var variationQuestions = db.quizQuestions.Where(x => x.variation_id == variation.variation_id).ToList();
                    foreach(var q in variationQuestions)
                    {
                        db.quizQuestions.Remove(q);
                    }
                }

                var quiz = db.quizzes.Where(x => x.quiz_id == id).SingleOrDefault();
                var result = db.quizzes.Remove(quiz);
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
                var item = db.quizzes.Where(x => x.quiz_id == id).Single();
                QuizViewModel quiz = new QuizViewModel();
                quiz.quizTitle = item.quiz_title;
                quiz.totalQuestions = Convert.ToInt32(item.total_questions);
                quiz.marksPerQuestion = double.Parse(Convert.ToString(item.marks_per_question));

                ViewBag.Type = "Edit";
                return View("Add", quiz);
            }catch(Exception e)
            {
                TempData["Error"] = "Something Went Wrong, Try Again";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(int id, QuizViewModel collection)
        {
            try
            {
                var item = db.quizzes.Where(x => x.quiz_id == id).Single();
                item.quiz_title = collection.quizTitle;
                item.marks_per_question = collection.marksPerQuestion;
                item.total_questions = collection.totalQuestions;
                item.total_marks = collection.marksPerQuestion * collection.totalQuestions;
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

        public ActionResult Marksheet(int? id)
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


            var students = db.studentQuizzes.Where(x => x.quiz_id == id).ToList();
            var studentMarks = new List<StudentQuiz>();
            foreach (var student in students)
            {
                var student_data = db.AspNetUsers.Where(x => x.Id == student.student_id).SingleOrDefault();
                var marks = db.studentMarks.Where(x => x.quiz_id == id).Where(x => x.student_id == student.student_id).ToList();
                var quiz = db.quizzes.Where(x => x.quiz_id == student.quiz_id).SingleOrDefault();

                StudentQuiz s = new StudentQuiz();
                s.token = student_data.Id;
                s.StudentName = student_data.fullName;
                s.studentRollNo = "2016-CS-213";
                s.obtainedMarks = student.marks;
                s.attempted_on = student.attempted_on;
                s.studentQuiz = quiz;
                studentMarks.Add(s);
            }

            return View(studentMarks);
        }

        public ActionResult Result(int id, string token)
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

            var student_data = db.AspNetUsers.Where(x => x.Id == token).SingleOrDefault();
            var studentQuiz = db.studentQuizzes.Where(x => x.quiz_id == id).Where(x => x.student_id == token).SingleOrDefault();
            var quiz = db.quizzes.Where(x => x.quiz_id == id).SingleOrDefault();
            var studentAnswers = db.studentMarks.Where(x => x.quiz_id == id).Where(x => x.student_id == token).ToList();
            int? vid = 0;
            if(studentAnswers != null)
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
            s.token = student_data.Id;
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