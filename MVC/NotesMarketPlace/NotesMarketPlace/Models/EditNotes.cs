﻿using NotesMarketPlace.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class EditNotes
    {
        public int ID { get; set; }

        public int NoteID { get; set; }

        [Required]
        
        public string Title { get; set; }

        [Required]
        public int Category { get; set; }

      
        public HttpPostedFileBase DisplayPicture { get; set; }

        public HttpPostedFileBase[] UploadNotes { get; set; }

        
        public Nullable<int> NoteType { get; set; }

       
        public Nullable<int> NumberofPages { get; set; }

        [Required]
        public string Description { get; set; }

        public string UniversityName { get; set; }

       
        public Nullable<int> Country { get; set; }

      
        public string Course { get; set; }
        
        public string CourseCode { get; set; }

        
        public string Professor { get; set; }

        
        public bool IsPaid { get; set; }

        public Nullable<decimal> SellingPrice { get; set; }

       
        public HttpPostedFileBase NotesPreview { get; set; }

        public List<NoteCategories> NoteCategoryList { get; set; }

        public List<NoteTypes> NoteTypeList { get; set; }

        public List<Countries> CountryList { get; set; }
        public string Note { get; set; }

        public string Picture { get; set; }
        public string Preview { get; set; }
    }

}