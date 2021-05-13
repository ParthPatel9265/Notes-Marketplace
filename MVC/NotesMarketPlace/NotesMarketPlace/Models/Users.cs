using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class Users
    {
        public int ID { get; set; }
        public int RoleID { get; set; }

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
        [EmailAddress]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"(?=.*\d)(?=.*[A-Za-z]).{6,}",ErrorMessage = "Invalid Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [NotMapped]
        [Compare("Password", ErrorMessage = "Confirm password is not match with password")]
        public string ConfirmPassword { get; set; }

        public bool IsEmailVerified { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.Guid> SecretCode { get; set; }
        public bool IsActive { get; set; }

        public bool RememberMe { get; set; }

        
    }
}