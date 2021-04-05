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
        [Required]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string EmailID { get; set; }

        [Required]
        public string Subjects { get; set; }

        [Required]
        public string Comments { get; set; }
    }
}