using NotesMarketPlace.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace NotesMarketPlace.Models
{
    public class AddNotes

    {
        public int ID { get; set; }
        public int NoteID { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(100, ErrorMessage = "Note Title is too long")]
        public string Title { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public int Category { get; set;}
 
        public HttpPostedFileBase DisplayPicture { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public HttpPostedFileBase[] UploadNotes { get; set; }

        public Nullable<int> NoteType { get; set; }

        [RegularExpression("[0-9]*", ErrorMessage = "Only numeric entry allowed")]
        public Nullable<int> NumberofPages { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Description { get; set; }

        public string UniversityName { get; set; }

        public Nullable<int> Country { get; set; }
   
        public string Course { get; set; }
       
        public string CourseCode { get; set; }

       
        public string Professor { get; set; }

        public bool IsPaid { get; set; }

        public Nullable<decimal> SellingPrice { get; set; }

        public HttpPostedFileBase NotesPreview { get; set; }

        public IEnumerable<NoteCategories> NoteCategoryList { get; set; }

        public IEnumerable<NoteTypes> NoteTypeList { get; set; }

        public IEnumerable<Countries> CountryList { get; set; }
    }
}