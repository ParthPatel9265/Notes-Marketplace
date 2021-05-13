using NotesMarketPlace.Context;
using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using System.IO.Compression;
using System.Net.Mail;
using System.Net;
using PagedList;
using PagedList.Mvc;
namespace NotesMarketPlace.Controllers
{
    [OutputCache(Duration = 0)]
    public class SearchNotesController : Controller
    {
        readonly private database1Entities dobj = new database1Entities();

        [HttpGet]
        [AllowAnonymous]
        [Route("SearchNotes")]
        public ActionResult SearchNotes(string search, string type, string category, string university, string course, string country, string ratings, int? page )
        {
            //if logged in user's userrole is not member then redirect to admin dashboard
            if (User.Identity.IsAuthenticated)
            {
                var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
                if (user.RoleID != dobj.UserRole.Where(x => x.Name.ToLower() == "member").Select(x => x.ID).FirstOrDefault())
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
            }

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

            ViewBag.ResultCount = searchnoteslist.Count();
           
            return View(searchnoteslist.ToList().AsQueryable().ToPagedList(page ?? 1, 9));
        }


        [AllowAnonymous]    
        [Route("SearchNotes/NoteDetail/{id}")]
        public ActionResult NoteDetail(int id)
        {
            var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();

            // if logged in user's role is not member then redirect to admin dashboard
            if (User.Identity.IsAuthenticated)
            {
                if (user.RoleID != dobj.UserRole.Where(x => x.Name.ToLower() == "member").Select(x => x.ID).FirstOrDefault())
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
            }
            var NoteDetail = dobj.NoteDetail.Where(x => x.ID == id && x.IsActive == true).FirstOrDefault();
           
            if (NoteDetail == null)
            {
                return HttpNotFound();
            }
            var seller = dobj.Users.Where(x => x.ID == NoteDetail.SellerID).FirstOrDefault();
          
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
            notesdetail.Seller = seller.FirstName + " " + seller.LastName;
            notesdetail.Buyer = user.FirstName;
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

        [Authorize(Roles = "Member")]
        [Route("SearchNotes/NoteDetail/{noteid}/Download")]
        public ActionResult DownloadNotes(int noteid)
        {
            //find note 
            var note = dobj.NoteDetail.Find(noteid);
       
            if (note == null)
            {
                return HttpNotFound();
            }

            var noteattachement = dobj.NotesAttachments.Where(x => x.NoteID == note.ID).FirstOrDefault();
           
            var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();

            // variable for attachement path
            string path;

            if (note.SellerID == user.ID)
            {
                // get attachement path
                path = Server.MapPath(noteattachement.FilePath);

                DirectoryInfo dir = new DirectoryInfo(path);
               
                // create zip of attachement
                using (var memoryStream = new MemoryStream())
                {
                    using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var item in dir.GetFiles())
                        {
                            // file path is attachement path + file name
                            string filepath = path + item.ToString();
                            ziparchive.CreateEntryFromFile(filepath, item.ToString());
                        }
                    }
                    // return zip
                    return File(memoryStream.ToArray(), "application/zip", note.Title + ".zip");
                }
            }

            // if note is free then we need to add entry in download table with allow download is true
       
            if (note.IsPaid == false)
            {

                // if user has already downloaded note then get download object
                var downloadfreenote = dobj.Downloads.Where(x => x.NoteID == noteid && x.Downloader == user.ID && x.IsSellerHasAllowedDownload == true && x.AttachmentPath != null).FirstOrDefault();
              
                // user is not downloaded note
                if (downloadfreenote == null)
                {
                    
                    Downloads download = new Downloads
                    {
                        NoteID = note.ID,
                        Seller = note.SellerID,
                        Downloader = user.ID,
                        IsSellerHasAllowedDownload = true,
                        AttachmentPath = noteattachement.FilePath,
                        IsAttachmentDownloaded = true,
                        AttachmentDownloadedDate = DateTime.Now,
                        IsPaid = note.IsPaid,
                        PurchasedPrice = note.SellingPrice,
                        NoteTitle = note.Title,
                        NoteCategory = note.NoteCategories.Name,
                        CreatedDate = DateTime.Now,
                        CreatedBy = user.ID,
                    };

                    
                    dobj.Downloads.Add(download);
                    dobj.SaveChanges();

                    path = Server.MapPath(download.AttachmentPath);
                }

                // if user is already downloaded note then get attachement path
                else
                {
                    path = Server.MapPath(downloadfreenote.AttachmentPath);
                }

                DirectoryInfo dir = new DirectoryInfo(path);
             
                
                //zip of attachment 
         
                using (var memoryStream = new MemoryStream())
                {
                    using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var item in dir.GetFiles())
                        {
                            // file path is attachement path + file name
                            string filepath = path + item.ToString();
                            ziparchive.CreateEntryFromFile(filepath, item.ToString());
                        }
                    }
                    // return zip
                    return File(memoryStream.ToArray(), "application/zip", note.Title + ".zip");
                }
            }
           

            //paid note
            else
            {
                // get download object
                var downloadpaidnote = dobj.Downloads.Where(x => x.NoteID == noteid && x.Downloader == user.ID && x.IsSellerHasAllowedDownload == true && x.AttachmentPath != null).FirstOrDefault();

                // if user is not already downloaded
                if (downloadpaidnote != null)
                {
                    // if user is download note first time then we need to update following record in download table 
                    if (downloadpaidnote.IsAttachmentDownloaded == false)
                    {
                        downloadpaidnote.IsAttachmentDownloaded = true;
                        downloadpaidnote.AttachmentDownloadedDate = DateTime.Now;
                        downloadpaidnote.ModifiedDate = DateTime.Now;
                        downloadpaidnote.ModifiedBy = user.ID;

                        // update ans save data in database
                        dobj.Entry(downloadpaidnote).State = EntityState.Modified;
                        dobj.SaveChanges();
                    }

                    path = Server.MapPath(downloadpaidnote.AttachmentPath);

                    DirectoryInfo dir = new DirectoryInfo(path);

                    //  zip of attachement
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                        {
                            foreach (var item in dir.GetFiles())
                            {
                                // file path is attachement path + file name
                                string filepath = path + item.ToString();
                                ziparchive.CreateEntryFromFile(filepath, item.ToString());
                                
                            }
                        }
                       
                        return File(memoryStream.ToArray(), "application/zip", note.Title + ".zip");
                    }
                }
                return RedirectToAction("NoteDetail", "SearchNotes", new { id = noteid });
            }
        }


        [Authorize(Roles = "Member")]
        [Route("SearchNotes/NoteDetail/{noteid}/Request")]     
        public ActionResult RequestPaidNotes(int noteid)
        {
            // get note
            var note = dobj.NoteDetail.Find(noteid);
            int ID = noteid;
            NoteStats noteobj = new NoteStats();
            
            
            // get logged in user
            var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();

            // create download object
            Downloads download = new Downloads
            {
                NoteID = note.ID,
                Seller = note.SellerID,
                Downloader = user.ID,
                IsSellerHasAllowedDownload = false,
                IsAttachmentDownloaded = false,
                IsPaid = note.IsPaid,
                PurchasedPrice = note.SellingPrice,
                NoteTitle = note.Title,
                NoteCategory = note.NoteCategories.Name,
                CreatedDate = DateTime.Now,
                CreatedBy = user.ID,
            };

            // add,save database 
            dobj.Downloads.Add(download);
            dobj.SaveChanges();

            TempData["ShowModal"] = 1;
            //mail 
            //Mailforpaid(download, user);

            return RedirectToAction("NoteDetail", new { id = note.ID });
        }
        public void Mailforpaid(Downloads download, NotesMarketPlace.Context.Users user)
        {

            var downloader = dobj.Users.Where(x => x.ID == download.Downloader).FirstOrDefault();
            var seller = dobj.Users.Where(x => x.ID == download.Seller).FirstOrDefault();

            var fromEmail = new MailAddress(dobj.SystemConfiguration.FirstOrDefault().EmailID1);
            var toEmail = new MailAddress(seller.EmailID);
            var fromEmailPassword = "****"; // Replace with actual password
            string subject = seller.FirstName + "wants to purchase your notes";

            string body = "Hello" + seller.FirstName + "," +
                "<br/><br/>We would like to inform you that," + downloader.FirstName + "wants to purchase your notes.Please see" +
                "<br/>Buyer Requests tab and allow download access to Buyer if you have received the payment from" +
                "<br/>him."+
                "<br/><br/>Regards,<br/>Notes Marketplace";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
        }
    }