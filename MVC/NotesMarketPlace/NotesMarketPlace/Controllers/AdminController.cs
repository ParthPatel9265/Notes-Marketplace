using NotesMarketPlace.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.IO;
using System.IO.Compression;
using System.Web.Mvc;
using NotesMarketPlace.Models;
using PagedList;
using System.Net.Mail;
using PagedList.Mvc;
namespace NotesMarketPlace.Controllers
{
    [OutputCache(Duration = 0)]
    [RoutePrefix("Admin")]
    public class AdminController : Controller
    {
        private readonly database1Entities dobj = new database1Entities();

        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Dashboard")]
        public ActionResult Dashboard(string search, string sort, string month, int? page)
        {
          
            ViewBag.Search = search;
            ViewBag.Sort = sort;
            ViewBag.PageNumber = page;
            ViewBag.Month = month;
           
            int memberid = dobj.UserRole.Where(x => x.Name.ToLower() == "member").Select(x => x.ID).FirstOrDefault();

            //get submitted and inreviw note status id
            int submittedforreviewid = dobj.ReferenceData.Where(x => x.Value.ToLower() == "submitted").Select(x => x.ID).FirstOrDefault();
            int inreviewid = dobj.ReferenceData.Where(x => x.Value.ToLower() == "in review").Select(x => x.ID).FirstOrDefault();

            // current date and time
            var now = DateTime.Now;

            // viewbag for monthlist
            ViewBag.MonthList = Enumerable.Range(1, 12).Select(x => new
            {
                Value = now.AddMonths(-x + 1).ToString("MM").ToString(),
                Text = now.AddMonths(-x + 1).ToString("MMMM").ToString()
            }).ToList();


            var last7days = DateTime.Now.AddDays(-7);

            AdminDashboard adminDash = new AdminDashboard();
         
           //count noteinreview and count downloaded notes of last seven days and count new registration of last seven days
            adminDash.NotesInReview = dobj.NoteDetail.Where(x => (x.Status == submittedforreviewid || x.Status == inreviewid) && x.IsActive == true).Count();
            adminDash.NotesDownloaded = dobj.Downloads.Where(x => x.AttachmentDownloadedDate > last7days).Count();
            adminDash.NewRegistration = dobj.Users.Where(x => x.RoleID == memberid && x.CreatedDate > last7days).Count();
            
            //notelist for total published notes
            var notelist = new List<AdminDashboard.PublishedNotesList>();

            // get published notes
            var publishednotelist = dobj.NoteDetail.Where(x => x.Status == 9);

            // get current month
            var currentMonth = DateTime.Now.ToString("MM");

            foreach (var item in publishednotelist)
            {
                // filter published notes by selected month
                // default we have to show current month published note
                if (String.IsNullOrEmpty(month))
                {
                    month = DateTime.Now.ToString("MM");
                    ViewBag.Month = month;
                }

                // if current month - selected month >= 0 then don't need to subtract year by 1
                if (Convert.ToInt32(currentMonth) - Convert.ToInt32(month) >= 0)
                {
                    // get year 
                    var year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
                  
                    // compare note's published month with selected month
                    bool selectedmonth = item.PublishedDate.Value.ToString("MM").Equals(month);
                   
                    // compare note's year with selected year
                    bool selectedyear = item.PublishedDate.Value.ToString("yyyy").Equals(year.ToString());
                   
                    // if one of them is false then we don't need to add notes in list
                    if (selectedmonth == false || selectedyear == false)
                    {
                        continue;

                    }
                }

                // if current month - selectd month < 0 then need to subtract year by 1
                else
                {
                    // subtract year by 1
                    var year = Convert.ToInt32(DateTime.Now.ToString("yyyy")) - 1;
                    bool selectedmonth = item.PublishedDate.Value.ToString("MM").Equals(month);
                    bool selectedyear = item.PublishedDate.Value.ToString("yyyy").Equals(year.ToString());
                    if (selectedmonth == false || selectedyear == false)
                    {
                        continue;
                    }
                }

                //object for publishednoteslist
                var note = new AdminDashboard.PublishedNotesList();

                var attachement = dobj.NotesAttachments.Where(x => x.NoteID == item.ID);
                var downloads = dobj.Downloads.Where(x => x.NoteID == item.ID && x.IsSellerHasAllowedDownload == true).Count();
                var publisher = dobj.Users.Where(x => x.ID == item.SellerID).First();

                // declare file size var
                decimal filesize = 0;

                // iterate through each attachment
                foreach (var files in attachement)
                {
                    string filepath = Server.MapPath(files.FilePath + files.FileName);
                    FileInfo file = new FileInfo(filepath);
                    // count file size and add into filesize var
                    filesize += file.Length;
                }

                note.ID = item.ID;
                note.Title = item.Title;
                note.Category = item.NoteCategories.Name;
                note.Publisher = publisher.FirstName + " " + publisher.LastName;
                note.PublishedDate = (DateTime)item.PublishedDate;
                note.Price = item.SellingPrice;
                note.SellType = item.IsPaid == true ? "Paid" : "Free";
                note.FileSize = filesize;
                note.Downloads = downloads;

                decimal sizemb = 0, sizekb = 0;
                // get file size in mb
                if (note.FileSize / 1024 > 1024)
                {
                    sizemb = note.FileSize / (1024 * 1024);
                }
                // get file size in kb
                else
                {
                    sizekb = Math.Ceiling(note.FileSize / 1024);
                }
                // file size in kb or mb
                if (sizemb != 0)
                {
                    note.FileSizeKBMB = sizemb.ToString("0.00") + " MB";
                }
                else
                {
                    note.FileSizeKBMB = sizekb.ToString() + " KB";
                }
                // add note in notelist
                notelist.Add(note);
            }

            // if search is not empty then search from result
            if (!String.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                notelist = notelist.Where(x => x.Title.ToLower().Contains(search) ||
                                               x.Category.ToLower().Contains(search) ||
                                               x.FileSizeKBMB.ToLower().Contains(search) ||
                                               x.Price.ToString().Contains(search) ||
                                               x.SellType.ToLower().Contains(search) ||
                                               x.Publisher.ToLower().Contains(search) ||
                                               x.PublishedDate.ToString("dd-MM-yyyy hh:mm").Contains(search) ||
                                               x.Downloads.ToString().ToString().Contains(search)
                                         ).ToList();
            }
            
           
            IEnumerable<AdminDashboard.PublishedNotesList> notespublish = notelist.AsEnumerable();
            notespublish = SortTableDashboard(sort,notespublish);
        
            adminDash.PublishedNotesLists = notespublish.ToList().AsQueryable().ToPagedList(page ?? 1,5);

            return View(adminDash);
        }

        // sorting for dashboard published notes
        private IEnumerable<AdminDashboard.PublishedNotesList> SortTableDashboard(string sort, IEnumerable<AdminDashboard.PublishedNotesList> table)
        {
            switch (sort)
            {
                case "Title_Asc":
                    {
                        table = table.OrderBy(x => x.Title);
                        break;
                    }
                case "Title_Desc":
                    {
                        table = table.OrderByDescending(x => x.Title);
                        break;
                    }
                case "Category_Asc":
                    {
                        table = table.OrderBy(x => x.Category);
                        break;
                    }
                case "Category_Desc":
                    {
                        table = table.OrderByDescending(x => x.Category);
                        break;
                    }
                case "FileSize_Asc":
                    {
                        table = table.OrderBy(x => x.FileSize);
                        break;
                    }
                case "FileSize_Desc":
                    {
                        table = table.OrderByDescending(x => x.FileSize);
                        break;
                    }
                case "SellType_Asc":
                    {
                        table = table.OrderBy(x => x.SellType);
                        break;
                    }
                case "SellType_Desc":
                    {
                        table = table.OrderByDescending(x => x.SellType);
                        break;
                    }
                case "Price_Asc":
                    {
                        table = table.OrderBy(x => x.Price);
                        break;
                    }
                case "Price_Desc":
                    {
                        table = table.OrderByDescending(x => x.Price);
                        break;
                    }
                case "Publisher_Asc":
                    {
                        table = table.OrderBy(x => x.Publisher);
                        break;
                    }
                case "Publisher_Desc":
                    {
                        table = table.OrderByDescending(x => x.Publisher);
                        break;
                    }
                case "PublishedDate_Asc":
                    {
                        table = table.OrderBy(x => x.PublishedDate);
                        break;
                    }
                case "PublishedDate_Desc":
                    {
                        table = table.OrderByDescending(x => x.PublishedDate);
                        break;
                    }
                case "Downloads_Asc":
                    {
                        table = table.OrderBy(x => x.Downloads);
                        break;
                    }
                case "Downloads_Desc":
                    {
                        table = table.OrderByDescending(x => x.Downloads);
                        break;
                    }
                default:
                    {
                        table = table.OrderByDescending(x => x.Downloads);
                        break;
                    }
            }
            return table;
        }


        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Notes/Unpublish")]
        public ActionResult UnPublishNote(FormCollection form)
        {
            int noteid = Convert.ToInt32(form["noteid"]);
            string remark = form["unpublish-remark"];

            var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var note = dobj.NoteDetail.Where(x => x.ID == noteid).FirstOrDefault();

            // get note status removed id
            int removedid = dobj.ReferenceData.Where(x => x.Value.ToLower() == "removed").Select(x => x.ID).FirstOrDefault();

            if (note == null)
            {
                return HttpNotFound();
            }

            // update note status
            note.Status = removedid;
            note.AdminRemarks = remark;
            note.ModifiedBy = user.ID;
            note.ModifiedDate = DateTime.Now;
            note.ActionedBy = user.ID;
            // save note in database
            dobj.Entry(note).State = EntityState.Modified;
            dobj.SaveChanges();

            // get seller user objecct
            var seller = dobj.Users.Where(x => x.ID == note.SellerID).FirstOrDefault();

            // send mail to seller
            UnpublishNoteTemplate(remark, seller,note);

            return RedirectToAction("Dashboard", "Admin");
        }

        public void UnpublishNoteTemplate(string remark, Context.Users seller,Context.NoteDetail note)
        {
            var email = dobj.SystemConfiguration.Select(x => x.EmailID1).FirstOrDefault();
           
            var fromEmail = new MailAddress(email);
            var toEmail = new MailAddress(seller.EmailID);
           
            var fromEmailPassword = "#####"; //replace with original password
            string subject = "Sorry! We need to remove your notes from our portal.";

            string body = "Hello " + seller.FirstName+" "+seller.LastName +","+
                "<br/><br/>We want to inform you that,your note " + note.Title + " has been removed from the portal." +
                "<br/>Please find our remarks as below.<br/>"+
                "<br/><br/>Regards,<br/>Notes Marketplace";

       
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);

        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Admin/Notes/{noteid}/Download")]
        public ActionResult AdminDownloadNote(int noteid)
        {
            // get note 
            var note = dobj.NoteDetail.Where(x => x.ID == noteid).FirstOrDefault();
            if (note == null)
            {
                return HttpNotFound();
            }

            // get first object of seller note attachement for attachement
            var noteattachement = dobj.NotesAttachments.Where(x => x.NoteID == note.ID).FirstOrDefault();
            var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();

            // get member role id
            int memberid = dobj.UserRole.Where(x => x.Name.ToLower() == "member").Select(x => x.ID).FirstOrDefault();

            // variable for attachement path
            string path;

            // check if user is admin or not
            if (user.RoleID != memberid)
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
            else
            {
                return HttpNotFound();
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Note/{noteid}")]
        public ActionResult NoteDetail(int noteid)
        {
            // get logged in user 
            var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();

            var NoteDetail = dobj.NoteDetail.Where(x => x.ID == noteid).FirstOrDefault();      
            if (NoteDetail == null)
            {
                return HttpNotFound();
            }

            // get reviews and user's full name and user's image
            IEnumerable<UserReviews> reviews = from review in dobj.NotesReview
                                                        join users in dobj.Users on review.ReviewedByID equals users.ID
                                                        join userprofile in dobj.UserProfileDetail on review.ReviewedByID equals userprofile.UserID
                                                        where review.NoteID == NoteDetail.ID && review.IsActive == true
                                                        orderby review.Ratings descending
                                                        select new UserReviews
                                                        {
                                                            TblSellerNotesReview = review,
                                                            TblUser = users,
                                                            TblUserProfile = userprofile
                                                        };
            // count total reviews
            var reviewcounts = reviews.Count();
           
            // count average review ratings
            decimal avgreview = 0;
            if (reviewcounts > 0)
            {
                avgreview = Math.Ceiling((from x in reviews select x.TblSellerNotesReview.Ratings).Average());
            }
            // count total spam report
            var spams = dobj.SpamReport.Where(x => x.NoteID == NoteDetail.ID).Count();
          
            // create adminnotedetail object
            AdminNoteDetail notesdetail = new AdminNoteDetail();
        
            //if user is authenticated
            if (user != null)
            {
                notesdetail.UserID = user.ID;
            }
            notesdetail.SellerNote = NoteDetail;
            notesdetail.NotesReview = reviews;
            notesdetail.AverageRating = Convert.ToInt32(avgreview);
            notesdetail.TotalReview = reviewcounts;
            notesdetail.TotalSpamReport = spams;

            return View(notesdetail);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Notes/DeleteReview/{id}")]  
        public ActionResult DeleteReview(int id)
        {
            // get review object
            var review = dobj.NotesReview.Where(x => x.ID == id).FirstOrDefault();

            if (review == null)
            {
                return HttpNotFound();
            }

            // remove review from database
            dobj.NotesReview.Remove(review);
            dobj.SaveChanges();

            return RedirectToAction("Note", new { id = review.NoteID });
        }


    }
}