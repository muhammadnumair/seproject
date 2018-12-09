using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace uetquizing.Models
{
    public class CategoryViewModel
    {
        public int categoryId { get; set; }
        
        [Required]
        [Display(Name = "Category Name*")]
        public string categoryName { get; set; }
        public string teacherId { get; set; }

    }
}