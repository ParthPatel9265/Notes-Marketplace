using System.Web;
using System.ComponentModel.DataAnnotations;
namespace NotesMarketPlace.Models
{
    public class SystemConfigurationModel
    {

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Use valid email address")]
        [MaxLength(100, ErrorMessage = "Email is too long")]
        public string EmailID1 { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Use valid email address")]
        public string EmailID2 { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Url]
        public string FacebookURL { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Url]
        public string TwitterURL { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Url]
        public string LinkedInURL { get; set; }

        public HttpPostedFileBase DefaultProfilePicture { get; set; }

        public HttpPostedFileBase DefaultNotePreview { get; set; }

        public string DefaultProfileURL { get; set; }

        public string DefaultNoteURL { get; set; }
    }
}