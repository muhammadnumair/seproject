using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uetquizing.Models
{
    public class StudentQuiz
    {
        public string token { get; set; }
        public string StudentName { get; set; }
        public double? obtainedMarks { get; set; }
        public string studentRollNo { get; set; }
        public DateTime? attempted_on { get; set; }
        public quizze studentQuiz { get; set; }

        public List<question> QuizQuestions {get; set;}
        public List<studentMark> StudentAnswers { get; set; }
    }
}