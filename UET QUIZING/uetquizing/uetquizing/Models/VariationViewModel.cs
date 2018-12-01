using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uetquizing.Models
{
    public class VariationViewModel
    {
        public int VarianceID { get; set; }
        public string VarianceTitle { get; set; }
        public string QuizId { get; set; }
        public List<QuestionViewModel> VarianceQuestions { get; set; }
    }
}