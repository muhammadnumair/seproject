using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace uetquizing.Models
{
    public class QuestionViewModel
    {

        public int question_id { get; set; }

        [Required]
        [Display(Name = "Question Title")]
        public string question_title { get; set; }
        [Required]
        [Display(Name = "Option 01")]
        public string option_1 { get; set; }
        [Required]
        [Display(Name = "Option 02")]
        public string option_2 { get; set; }
        [Required]
        [Display(Name = "Option 03")]
        public string option_3 { get; set; }
        [Required]
        [Display(Name = "Option 04")]
        public string option_4 { get; set; }

        [Required]
        [Display(Name = "Correct Answer")]
        public string correct_answer { get; set; }
        [Required]
        [Display(Name = "Choose a Category")]
        public int catgory_id { get; set; }
        public string teacher_id { get; set; }



    }
}