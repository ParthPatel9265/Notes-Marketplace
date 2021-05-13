using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class ContactUs
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Use valid email address")]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Subjects { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Comments { get; set; }
    }
}