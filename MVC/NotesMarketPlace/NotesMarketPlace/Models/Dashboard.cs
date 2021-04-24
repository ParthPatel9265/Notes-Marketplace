using NotesMarketPlace.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using PagedList.Mvc;
namespace NotesMarketPlace.Models
{
    public class Dashboard
    {
        public IPagedList<NoteDetail> InProgressNotes { get; set; }
        public IPagedList<NoteDetail> PublishedNotes { get; set; }
        public int? MyDownloads { get; set; }
        public int? NumberOfSoldNotes { get; set; }
        public decimal? MoneyEarned { get; set;}
        public int? MyRejectedNotes { get; set; }
        public int? BuyerRequest { get; set; }
    }
}