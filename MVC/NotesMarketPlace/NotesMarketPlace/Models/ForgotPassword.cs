using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class ForgotPassword
    {
        [Required(ErrorMessage = "This field is required")]
        [EmailAddress]
        public string EmailID { get; set; }
    }
}