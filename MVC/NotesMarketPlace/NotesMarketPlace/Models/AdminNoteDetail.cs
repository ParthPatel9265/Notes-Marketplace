using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotesMarketPlace.Context;
namespace NotesMarketPlace.Models
{
    public class AdminNoteDetail
    {
        public int? UserID { get; set; }
        public NoteDetail SellerNote { get; set; }
        public IEnumerable<UserReviews> NotesReview { get; set; }
        public int? AverageRating { get; set; }
        public int? TotalReview { get; set; }
        public int? TotalSpamReport { get; set; }
    }

    public class UserReviews
    {
        public Context.Users TblUser { get; set; }
        public UserProfileDetail TblUserProfile { get; set; }
        public NotesReview TblSellerNotesReview { get; set; }
    }
}