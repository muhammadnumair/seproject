using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace uetquizing.Models
{
    public class QuizViewModel
    {
        public int quizId { get; set; }

        [Required]
        [Display(Name = "Quiz Title")]
        public string quizTitle { get; set; }
        public float totalMarks { get; set; }
        
        [Display(Name = "Total Questions")]
        public int totalQuestions { get; set; }

        [Display(Name = "Questions Category")]
        public int questionsCategory{ get; set; }

        [Display(Name = "Marks Per Question")]
        public float marksPerQuestion { get; set; }
        public string teacherId { get; set; }

        public string quizType { get; set; }
    }
}