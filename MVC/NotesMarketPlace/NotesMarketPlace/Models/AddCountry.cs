﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace NotesMarketPlace.Models
{
    public class AddCountry
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Invalid country name")]
        [MaxLength(100, ErrorMessage = "Country name is too long")]
        public string CountryName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(10, ErrorMessage = "Country code is too long")]
        public string CountryCode { get; set; }
    }
}