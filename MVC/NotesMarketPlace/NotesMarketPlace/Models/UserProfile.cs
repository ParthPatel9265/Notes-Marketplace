using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NotesMarketPlace.Context;
namespace NotesMarketPlace.Models
{
    public class UserProfile
    {
        
        public int ID { get; set; }
        public int UserID { get; set; }


        [Required(ErrorMessage = "This field is required")]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Invalid Name")]
        [MaxLength(50, ErrorMessage = "Name is too long")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "This field is required")]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Invalid Name")]
        [MaxLength(50, ErrorMessage = "Name is too long")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Use valid email address")]
        public string EmailID { get; set; }

       
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<int> Gender { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string CountryCode { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [RegularExpression("[0-9]*", ErrorMessage = "Invalid Mobile Number")]
        public string PhoneNumber { get; set; }
        public HttpPostedFileBase ProfilePicture { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(100, ErrorMessage = "Address is too long")]
        public string AddressLine1 { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(100, ErrorMessage = "Address is too long")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(50, ErrorMessage = "City name is too long")]
        public string City { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(50, ErrorMessage = "City name is too long")]
        public string State { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Country { get; set; }

        [MaxLength(100, ErrorMessage = "University name is too long")]
        public string University { get; set; }

        [MaxLength(100, ErrorMessage = "College name is too long")]
        public string College { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public IEnumerable<Countries> CountryList { get; set; }
        public IEnumerable<ReferenceData> GenderList { get; set; }
        public string ProfilePictureUrl { get; set; }

    }

}