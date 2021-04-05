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
        [Required]
        public string OldPassword { get; set; }
        [Required]
        [RegularExpression(@"(?=.*\d)(?=.*[A-Za-z]).{6,}", ErrorMessage = "Invalid Password format.")]
        public string NewPassword { get; set; }
        [Required]
        [NotMapped]
        [Compare("NewPassword", ErrorMessage = "Incorrect Password")]
        public string ConfirmPassword { get; set; }
    }
}