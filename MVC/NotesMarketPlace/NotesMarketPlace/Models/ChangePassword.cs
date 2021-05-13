using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "This field is required")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"(?=.*\d)(?=.*[A-Za-z]).{6,}", ErrorMessage = "Invalid Password format.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [NotMapped]
        [Compare("NewPassword", ErrorMessage = "Incorrect Password")]
        public string ConfirmPassword { get; set; }
    }
}