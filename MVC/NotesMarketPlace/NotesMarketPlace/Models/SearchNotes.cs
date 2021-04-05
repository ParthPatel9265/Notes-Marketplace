using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NotesMarketPlace.Context;
namespace NotesMarketPlace.Models
{
    public class SearchNotes
    {
        public NoteDetail Note { get; set; }
        public int AverageRating { get; set; }
        public int TotalRating { get; set; }
        public int TotalSpam { get; set; }
    }
}