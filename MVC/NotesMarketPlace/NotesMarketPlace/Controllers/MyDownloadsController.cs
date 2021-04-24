using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using NotesMarketPlace.Context;
using NotesMarketPlace.Models;
using PagedList;
using PagedList.Mvc;
using System.Net;

namespace NotesMarketPlace.Controllers
{
    public class MyDownloadsController : Controller
    {
        readonly private database1Entities dobj = new database1Entities();
        // GET: MyDownloads

        [Authorize]
        [Route("MyDownloads")]
        public ActionResult MyDownloads(string search, string sort, int? page)
        {
            ViewBag.MyDownloads = "active";
            ViewBag.Search = search;
            ViewBag.Sort = sort;
          
            //user
            var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();

            // downlaoded notes of user
            IEnumerable<MyDownloads> mydownloads = from download in dobj.Downloads
                                                   join users in dobj.Users on download.Seller equals users.ID
                                                   join review in dobj.NotesReview on download.ID equals review.AgainstDownloadsID into r
                                                   from notereview in r.DefaultIfEmpty()
                                                   where download.Downloader == user.ID && download.IsSellerHasAllowedDownload == true && download.AttachmentPath != null
                                                   select new MyDownloads
                                                   {
                                                       NoteID = download.NoteID,
                                                       DownloadID = download.ID,
                                                       Title = download.NoteTitle,
                                                       Category = download.NoteCategory,
                                                       Seller = users.EmailID,
                                                       SellType = download.IsPaid == true ? "Paid" : "Free",
                                                       Price = download.PurchasedPrice,
                                                       DownloadedDate = download.AttachmentDownloadedDate.Value,
                                                       NoteDownloaded = download.IsAttachmentDownloaded,
                                                       ReviewID = notereview.ID,
                                                       Rating = notereview.Ratings,
                                                       Comment = notereview.Comments
                                                   };

            // if search is not empty then search
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                mydownloads = mydownloads.Where(x => x.Title.ToLower().Contains(search) ||
                                                     x.Category.ToLower().Contains(search) ||
                                                     x.Seller.ToLower().Contains(search) ||
                                                     x.Price.ToString().ToLower().Contains(search) ||
                                                     x.SellType.ToLower().Contains(search)
                                               );
            }

            // sorting result
            mydownloads = SortTableMyDownloads(sort, mydownloads);

          
            var mydown = new List<MyDownloads>();
            mydown = mydownloads.ToList();
             
            // return results
            return View(mydown.ToList().AsQueryable().ToPagedList(page ?? 1,10));
        }

        // sorting 
        private IEnumerable<MyDownloads> SortTableMyDownloads(string sort, IEnumerable<MyDownloads> table)
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
                case "Seller_Asc":
                    {
                        table = table.OrderBy(x => x.Seller);
                        break;
                    }
                case "Seller_Desc":
                    {
                        table = table.OrderByDescending(x => x.Seller);
                        break;
                    }
                case "Type_Asc":
                    {
                        table = table.OrderBy(x => x.SellType);
                        break;
                    }
                case "Type_Desc":
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
                case "DownloadedDate_Asc":
                    {
                        table = table.OrderBy(x => x.DownloadedDate);
                        break;
                    }
                case "DownloadedDate_Desc":
                    {
                        table = table.OrderByDescending(x => x.DownloadedDate);
                        break;
                    }
                default:
                    {
                        table = table.OrderByDescending(x => x.DownloadedDate);
                        break;
                    }
            }
            return table;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Route("Note/AddReview")]
        public ActionResult AddReview(NotesReview notereview)
        {
            // check if comment is null or not
            if (String.IsNullOrEmpty(notereview.Comments))
            {
                return RedirectToAction("MyDownloads");
            }

            // check if rating is between 1 to 5
            if (notereview.Ratings < 1 || notereview.Ratings > 5)
            {
                return RedirectToAction("MyDownloads");
            }

            // get Download object for check if user is downloaded note or not
            var notedownloaded = dobj.Downloads.Where(x => x.ID == notereview.AgainstDownloadsID && x.IsAttachmentDownloaded == true).FirstOrDefault();

            // user can provide notereview after downloading the note
            if (notedownloaded != null)
            {
               
                var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();

                var alreadyprovidereview = dobj.NotesReview.Where(x => x.AgainstDownloadsID == notereview.AgainstDownloadsID && x.IsActive == true).FirstOrDefault();

                // if user not provide notereview then add notereview
                if (alreadyprovidereview == null)
                {
                    
                    NotesReview review = new NotesReview();

                    review.NoteID = notereview.NoteID;
                    review.AgainstDownloadsID = notereview.AgainstDownloadsID;
                    review.ReviewedByID = user.ID;
                    review.Ratings = notereview.Ratings;
                    review.Comments = notereview.Comments;
                    review.CreatedDate = DateTime.Now;
                    review.CreatedBy = user.ID;
                    review.IsActive = true;

                    dobj.NotesReview.Add(review);
                    dobj.SaveChanges();

                    return RedirectToAction("MyDownloads");
                }

                // edit previous notereview 
                else
                {
                    alreadyprovidereview.Ratings = notereview.Ratings;
                    alreadyprovidereview.Comments = notereview.Comments;
                    alreadyprovidereview.ModifiedDate = DateTime.Now;
                    alreadyprovidereview.ModifiedBy = user.ID;

                    // update and save notereview 
                    dobj.Entry(alreadyprovidereview).State = EntityState.Modified;
                    dobj.SaveChanges();

                    return RedirectToAction("MyDownloads");
                }
            }
            return RedirectToAction("MyDownloads");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Route("Note/ReportSpam")]
        public ActionResult SpamReport(FormCollection form)
        {
            
            int downloadid = Convert.ToInt32(form["downloadid"]);

            var alreadyreportedspam = dobj.SpamReport.Where(x => x.AgainstDownloadsID == downloadid).FirstOrDefault();

            // if user not already reported spam 
            if (alreadyreportedspam == null)
            { 
                var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();

                string membername = user.FirstName + " " + user.LastName;

                // create a spam report object
                SpamReport spamreport = new SpamReport();

                spamreport.NoteID = Convert.ToInt32(form["noteid"]);
                spamreport.AgainstDownloadsID = downloadid;
                spamreport.ReportedByID = user.ID;
                spamreport.Remarks = form["spamreport"];
                spamreport.CreatedDate = DateTime.Now;
                spamreport.CreatedBy = user.ID;

                // save spam report object into database 
                dobj.SpamReport.Add(spamreport);
                dobj.SaveChanges();

                // send mail to admin that buyer reported the notes as inappropriate
                SpamReportMail(spamreport, membername);
            }
            return RedirectToAction("MyDownloads");
        }

        public void SpamReportMail(SpamReport spam,string membername)
        {
            var note = dobj.NoteDetail.Find(spam.NoteID);
            var seller = dobj.Users.Find(note.SellerID);
            string sellername = seller.FirstName + " " + seller.LastName;

            var fromEmail = new MailAddress(dobj.SystemConfiguration.FirstOrDefault().EmailID1);//Support Email Address
            var toEmail = new MailAddress(dobj.SystemConfiguration.FirstOrDefault().EmailID2);
            var fromEmailPassword = "*******"; // Replace with actual password
            string subject = membername + " Reported an issue for " + note.Title;

            string body = "Hello Admins, " +
                "<br/><br/>We want to inform you that, " + membername + " Reported an issue for " +
                sellername + "’s Note with title " + note.Title + ". Please look at the notes and take required actions." +
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


