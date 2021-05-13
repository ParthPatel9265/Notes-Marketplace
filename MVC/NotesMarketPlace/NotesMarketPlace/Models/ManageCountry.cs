﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class ManageCountry
    {
        public int ID { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public DateTime DateAdded { get; set; }
        public string AddedBy { get; set; }
        public string Active { get; set; }
    }
}