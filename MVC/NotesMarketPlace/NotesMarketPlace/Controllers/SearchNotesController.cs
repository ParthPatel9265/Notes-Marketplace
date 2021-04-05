using NotesMarketPlace.Context;
using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace NotesMarketPlace.Controllers
{
    public class SearchNotesController : Controller
    {
        readonly private database1Entities dobj = new database1Entities();

        [HttpGet]
        [Route("SearchNotes")]
        public ActionResult SearchNotes(string search, string type, string category, string university, string course, string country, string ratings, int page = 1)
        {
            
            ViewBag.SearchNotes = "active";
            ViewBag.Search = search;
            ViewBag.Category = category;
            ViewBag.Type = type;
            ViewBag.University = university;
            ViewBag.Course = course;
            ViewBag.Country = country;
            ViewBag.Rating = ratings;

           //for dropdown 
            ViewBag.CategoryList = dobj.NoteCategories.Where(x => x.IsActive == true).ToList();
            ViewBag.TypeList = dobj.NoteTypes.Where(x => x.IsActive == true).ToList();
            ViewBag.CountryList = dobj.Countries.Where(x => x.IsActive == true).ToList();
            ViewBag.UniversityList = dobj.NoteDetail.Where(x => x.IsActive == true && x.UniversityName != null && x.Status == 9).Select(x => x.UniversityName).Distinct();
            ViewBag.CourseList = dobj.NoteDetail.Where(x => x.IsActive == true && x.Course != null && x.Status == 9).Select(x => x.Course).Distinct();
            ViewBag.RatingList = new List<SelectListItem> { new SelectListItem { Text = "1+", Value = "1" }, new SelectListItem { Text = "2+", Value = "2" }, new SelectListItem { Text = "3+", Value = "3" }, new SelectListItem { Text = "4+", Value = "4" }, new SelectListItem { Text = "5", Value = "5" } };

            //details of publish notes
            var noteslist = dobj.NoteDetail.Where(x => x.Status == 9);

            // search field is fillup
            if (!String.IsNullOrEmpty(search))
            {
                noteslist = noteslist.Where(x => x.Title.ToLower().Contains(search.ToLower()) ||
                                                 x.NoteCategories.Name.ToLower().Contains(search.ToLower())
                                            );
            }
            // for note type of notes
            if (!String.IsNullOrEmpty(type))
            {
                noteslist = noteslist.Where(x => x.NoteType.ToString().ToLower().Contains(type.ToLower()));
            }
            // for note category of notes
            if (!String.IsNullOrEmpty(category))
            {
                noteslist = noteslist.Where(x => x.Category.ToString().ToLower().Contains(category.ToLower()));
            }
            // for university
            if (!String.IsNullOrEmpty(university))
            {
                noteslist = noteslist.Where(x => x.UniversityName.ToLower().Contains(university.ToLower()));
            }
            // for course detail 
            if (!String.IsNullOrEmpty(course))
            {
                noteslist = noteslist.Where(x => x.Course.ToLower().Contains(course.ToLower()));
            }
            // for country detail
            if (!String.IsNullOrEmpty(country))
            {
                noteslist = noteslist.Where(x => x.Country.ToString().ToLower().Contains(country.ToLower()));
            }

            List<SearchNotes> searchnoteslist = new List<SearchNotes>();

            if (String.IsNullOrEmpty(ratings))
            {
                foreach (var item in noteslist)
                {
                    var review = dobj.NotesReview.Where(x => x.NoteID == item.ID && x.IsActive == true).Select(x => x.Ratings);
                 
                    var totalreview = review.Count();
                   
                    var avgreview = totalreview > 0 ? Math.Ceiling(review.Average()) : 0;
                 
                    var spamcount = dobj.SpamReport.Where(x => x.NoteID == item.ID).Count();

                    SearchNotes note = new SearchNotes()
                    {
                        Note = item,
                        AverageRating = Convert.ToInt32(avgreview),
                        TotalRating = totalreview,
                        TotalSpam = spamcount
                    };

                    //add obj in searchnoteslist
                    searchnoteslist.Add(note);
                }
            }
            
            else
            {
                foreach (var item in noteslist)
                {
                  
                    var review = dobj.NotesReview.Where(x => x.NoteID == item.ID).Select(x => x.Ratings);
                    
                    var totalreview = review.Count();
                    
                    var avgreview = totalreview > 0 ? Math.Ceiling(review.Average()) : 0;
                   
                    var spamcount = dobj.SpamReport.Where(x => x.NoteID == item.ID).Count();
                   
                    if (avgreview >= Convert.ToInt32(ratings))
                    {
                        
                        SearchNotes note = new SearchNotes()
                        {
                            Note = item,
                            AverageRating = Convert.ToInt32(avgreview),
                            TotalRating = totalreview,
                            TotalSpam = spamcount
                        };
                        // add object into list
                        searchnoteslist.Add(note);
                    }

                }
            }

            ViewBag.PageNumber = page;
            ViewBag.TotalPages = Math.Ceiling(searchnoteslist.Count() / 9.0);
            IEnumerable<SearchNotes> result = searchnoteslist.AsEnumerable().Skip((page - 1) * 9).Take(9);
            ViewBag.ResultCount = searchnoteslist.Count();
            return View(result);
        }


      
        [Authorize]
        [HttpGet] 
        [Route("SearchNotes/NoteDetail/{id}")]
        public ActionResult NoteDetail(int id)
        {
            var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            
            var NoteDetail = dobj.NoteDetail.Where(x => x.ID == id && x.IsActive == true).FirstOrDefault();
           
            if (NoteDetail == null)
            {
                return HttpNotFound();
            }
           
            IEnumerable<ReviewsModel> reviews = from review in dobj.NotesReview
                                                join users in dobj.Users on review.ReviewedByID equals users.ID
                                                join userprofile in dobj.UserProfileDetail on review.ReviewedByID equals userprofile.UserID
                                                where review.NoteID == id && review.IsActive == true
                                                orderby review.Ratings descending
                                                select new ReviewsModel { TblSellerNotesReview = review, TblUser = users, TblUserProfile = userprofile };
           
            var reviewcounts = reviews.Count();
          
            decimal avgreview = 0;
            if (reviewcounts > 0)
            {
                avgreview = Math.Ceiling((from x in reviews select x.TblSellerNotesReview.Ratings).Average());
            }
            
            var spams = dobj.SpamReport.Where(x => x.NoteID == id).Count();
            
            NoteStats notesdetail = new NoteStats();
           
            if (user != null)
            {
                notesdetail.UserID = user.ID;
            }
            notesdetail.SellerNote = NoteDetail;
            notesdetail.NotesReview = reviews;
            notesdetail.AverageRating = Convert.ToInt32(avgreview);
            notesdetail.TotalReview = reviewcounts;
            notesdetail.TotalSpamReport = spams;
           
            if (User.Identity.IsAuthenticated)
            {
               var request = dobj.Downloads.Where(x => x.NoteID == id && x.Downloader == user.ID && x.IsSellerHasAllowedDownload == false && x.AttachmentPath == null).FirstOrDefault();
               var allowdownloadnotes = dobj.Downloads.Where(x => x.NoteID == id && x.Downloader == user.ID && x.IsSellerHasAllowedDownload == true && x.AttachmentPath != null).FirstOrDefault();
               if (request == null && allowdownloadnotes == null)
                {
                    notesdetail.NoteRequested = false;
                }
                else
                {
                    notesdetail.NoteRequested = true;
                }

                if (allowdownloadnotes != null && request == null)
                {
                    notesdetail.AllowDownload = true;
                }
                else
                {
                    notesdetail.AllowDownload = false;
                }
            }
            if (User.Identity.IsAuthenticated)
            { return View(notesdetail); }
            else
            {
                return RedirectToAction("Login", "SignUp");
            }
        }
    }
}