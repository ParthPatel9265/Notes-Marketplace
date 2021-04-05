using NotesMarketPlace.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class BuyerRequest
    {
        public Context.Downloads TblDownload { get; set; }
        public Context.Users TblUser { get; set; }
        public Context.UserProfileDetail TblUserProfile { get; set; }
    }
     
}   