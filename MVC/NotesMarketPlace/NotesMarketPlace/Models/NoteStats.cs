using NotesMarketPlace.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class NoteStats
    { 
            public int? UserID { get; set; }
            public NoteDetail SellerNote { get; set; }
            public IEnumerable<ReviewsModel> NotesReview { get; set; }
            public int? AverageRating { get; set; }
            public int? TotalReview { get; set; }
            public int? TotalSpamReport { get; set; }
            public bool NoteRequested { get; set; }
            public bool AllowDownload { get; set; }
        }

        public class ReviewsModel
        {
            public Context.Users TblUser { get; set; }
            public Context.UserProfileDetail TblUserProfile { get; set; }
            public NotesReview TblSellerNotesReview { get; set; }
        }
    }

