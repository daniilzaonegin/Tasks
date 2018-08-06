using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tasks.Models
{
    public class CompleteDialogViewModel
    {
        [Required(ErrorMessage ="Task not found in the database!")]
        public int? TaskId { get; set; }

        [Required(ErrorMessage = "Please enter a completion message!")]
        [Display(Name ="Complete message")]
        public string CompleteMsg { get; set; }

        public bool Completed { get; set; }
    }
}