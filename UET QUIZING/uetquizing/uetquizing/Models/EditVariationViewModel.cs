using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uetquizing.Models
{
    public class EditVariationViewModel
    {
        public string questionTitle { get; set; }
        public int questionID { get; set; }
        public int quizQuestionID { get; set; }
        public int variationID { get; set; }
    }
}