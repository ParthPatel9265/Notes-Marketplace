using NotesMarketPlace.Models;
using NotesMarketPlace.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace NotesMarketplace.Controllers
{
    public class BuyerRequestController : Controller
    {
        readonly private database1Entities dobj = new database1Entities();

      

        [Authorize]
        [Route("BuyerRequest")]
        public ActionResult BuyerRequest(string search, string sort, int page=1 )
        {
          
            ViewBag.BuyerRequest = "active";

           
            ViewBag.Sort = sort;
            ViewBag.Search = search;
            ViewBag.PageNumber = page;
          
            NotesMarketPlace.Context.Users user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();

           
            IEnumerable<BuyerRequest> buyerrequest = (from download in dobj.Downloads
                                                              join users in dobj.Users on download.Downloader equals users.ID
                                                              join userprofile in dobj.UserProfileDetail on download.Downloader equals userprofile.UserID
                                                              where download.Seller == user.ID && download.IsSellerHasAllowedDownload == false && download.AttachmentPath == null
                           select new BuyerRequest { TblDownload = download, TblUser = users, TblUserProfile = userprofile });

            
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                buyerrequest = buyerrequest.Where(
                                                    x => x.TblDownload.NoteTitle.ToLower().Contains(search) ||
                                                         x.TblDownload.NoteCategory.ToLower().Contains(search) ||
                                                         x.TblUser.EmailID.ToLower().Contains(search) ||
                                                         x.TblDownload.PurchasedPrice.ToString().ToLower().Contains(search) ||
                                                         x.TblUserProfile.PhoneNumber.ToLower().Contains(search)
                                                 );
            }

            
            buyerrequest = SortTableBuyerRequest(sort, buyerrequest);
            
            ViewBag.TotalPages = (Math.Ceiling(buyerrequest.Count() / 10.0));

            buyerrequest = buyerrequest.Skip((page - 1) * 10).Take(10);


            return View(buyerrequest);
        }

        private IEnumerable<BuyerRequest> SortTableBuyerRequest(string sort,IEnumerable<BuyerRequest> table)
        {
            switch (sort)
            {
                case "Title_Asc":
                    {
                       table = table.OrderBy(x => x.TblDownload.NoteTitle);
                        break;
                    }
                case "Title_Desc":
                    {
                        table =table.OrderByDescending(x => x.TblDownload.NoteTitle);
                        break;
                    }
                case "Category_Asc":
                    {
                        table = table.OrderBy(x => x.TblDownload.NoteCategory);
                        break;
                    }
                case "Category_Desc":
                    {
                        table = table.OrderByDescending(x => x.TblDownload.NoteCategory);
                        break;
                    }
                case "Buyer_Asc":
                    {
                        table = table.OrderBy(x => x.TblUser.EmailID);
                        break;
                    }
                case "Buyer_Desc":
                    {
                        table = table.OrderByDescending(x => x.TblUser.EmailID);
                        break;
                    }
                case "Phone_Asc":
                    {
                        table = table.OrderBy(x => x.TblUserProfile.PhoneNumber);
                        break;
                    }
                case "Phone_Desc":
                    {
                        table = table.OrderByDescending(x => x.TblUserProfile.PhoneNumber);
                        break;
                    }
                case "Type_Asc":
                    {
                        table = table.OrderBy(x => x.TblDownload.IsPaid);
                        break;
                    }
                case "Type_Desc":
                    {
                        table = table.OrderByDescending(x => x.TblDownload.IsPaid);
                        break;
                    }
                case "Price_Asc":
                    {
                        table = table.OrderBy(x => x.TblDownload.PurchasedPrice);
                        break;
                    }
                case "Price_Desc":
                    {
                        table = table.OrderByDescending(x => x.TblDownload.PurchasedPrice);
                        break;
                    }
                case "DownloadedDate_Asc":
                    {
                        table = table.OrderBy(x => x.TblDownload.CreatedDate);
                        break;
                    }
                case "DownloadedDate_Desc":
                    {
                        table = table.OrderByDescending(x => x.TblDownload.CreatedDate);
                        break;
                    }
                default:
                    {
                        table = table.OrderByDescending(x => x.TblDownload.CreatedDate);
                        break;
                    }
            }
            return table;
        }

        [Authorize]
        [Route("BuyerRequest/AllowDownload/{id}")]
        public ActionResult AllowDownload(int id)
        {
            
            NotesMarketPlace.Context.Users user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            
            Downloads download = dobj.Downloads.Find(id);
          
            if (user.ID == download.Seller)
            {
               
                NotesAttachments attachement = dobj.NotesAttachments.Where(x => x.NoteID == download.NoteID && x.IsActive == true).FirstOrDefault();

                
                dobj.Downloads.Attach(download);
                download.IsSellerHasAllowedDownload = true;
                download.AttachmentPath = attachement.FilePath;
                download.ModifiedBy = user.ID;
                download.ModifiedDate = DateTime.Now;
                dobj.SaveChanges();

                
                AllowDownloadTemplate(download, user);

                return RedirectToAction("BuyerRequest");

            }
            else
            {
                return RedirectToAction("BuyerRequest");

            }
        }

        public void AllowDownloadTemplate(Downloads download, NotesMarketPlace.Context.Users seller)
        {
            
            var downloader = dobj.Users.Where(x => x.ID == download.Downloader).FirstOrDefault();
          
            SystemConfiguration s = new SystemConfiguration();

            var fromEmail = new MailAddress(s.EmailID1);
            var toEmail = new MailAddress(downloader.EmailID);
            var fromEmailPassword = "*****"; // Replace with actual password
            string subject = seller.FirstName + "Allows you to download a note";

            string body = "Hello"+ downloader.FirstName+","+
                "<br/><br/>We would like to inform you that,"+ seller.FirstName+ "Allows you to download a note " +
                "<br/> Please login and see My Download tabs to download particular note." +
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