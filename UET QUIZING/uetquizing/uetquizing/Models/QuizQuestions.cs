using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uetquizing.Models
{
    public class QuixXQuestion
    {
        public int? questionID { get; set; }
        public string selectedOption { get; set; }
    }

    public class QuizQuestions
    {
        public int quizID { get; set; }
        public int variations_id { get; set; }
        public List<QuixXQuestion> Questions { get; set; }
    }
}