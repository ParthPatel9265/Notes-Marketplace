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
         database1Entities dobj = new database1Entities();


        [Authorize]
        [Route("BuyerRequest")]
        public ActionResult BuyerRequest(string search, string sort, int? page )
        {
          
            ViewBag.BuyerRequest = "active";

           
            ViewBag.Sort = sort;
            ViewBag.Search = search;
            ViewBag.PageNumber = page;
          
            NotesMarketPlace.Context.Users user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();

           
            IEnumerable<BuyerRequest> buyerrequest = from download in dobj.Downloads
                                                              join users in dobj.Users on download.Downloader equals users.ID
                                                              join userprofile in dobj.UserProfileDetail on download.Downloader equals userprofile.UserID
                                                              where download.Seller == user.ID && download.IsSellerHasAllowedDownload == false && download.AttachmentPath == null
                           select new BuyerRequest
                           {
                               NoteID = download.NoteID,
                               DownloadID = download.ID,
                               Title = download.NoteTitle,
                               Category = download.NoteCategory,
                               Buyer = users.EmailID,
                               PhoneNo = userprofile.CountryCode + " " + userprofile.PhoneNumber,
                               SellType = download.IsPaid == true ? "Paid" : "Free",
                               SellingPrice = download.PurchasedPrice,
                               DownloadedDate = download.CreatedDate.Value
                           };

            
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                buyerrequest = buyerrequest.Where(
                                                    x => x.Title.ToLower().Contains(search) ||
                                                         x.Category.ToLower().Contains(search) ||
                                                         x.Buyer.ToLower().Contains(search) ||
                                                         x.SellingPrice.ToString().ToLower().Contains(search) ||
                                                         x.PhoneNo.ToLower().Contains(search)||
                                                         x.SellType.ToLower().Contains(search)
                                                 );
            }

            
            buyerrequest = SortTableBuyerRequest(sort, buyerrequest);

            var buyer = new List<BuyerRequest>();
            buyer = buyerrequest.ToList();
  
            return View(buyer.ToList().AsQueryable().ToPagedList(page ?? 1, 10));
              
              
        }

        private IEnumerable<BuyerRequest> SortTableBuyerRequest(string sort,IEnumerable<BuyerRequest> table)
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
                        table =table.OrderByDescending(x => x.Title);
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
                case "Buyer_Asc":
                    {
                        table = table.OrderBy(x => x.Buyer);
                        break;
                    }
                case "Buyer_Desc":
                    {
                        table = table.OrderByDescending(x => x.Buyer);
                        break;
                    }
                case "Phone_Asc":
                    {
                        table = table.OrderBy(x => x.PhoneNo);
                        break;
                    }
                case "Phone_Desc":
                    {
                        table = table.OrderByDescending(x => x.PhoneNo);
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
                        table = table.OrderBy(x => x.SellingPrice);
                        break;
                    }
                case "Price_Desc":
                    {
                        table = table.OrderByDescending(x => x.SellingPrice);
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

                
                //AllowDownloadTemplate(download, user);

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

            var email = dobj.SystemConfiguration.Select(x => x.EmailID1).FirstOrDefault();
            var fromEmail = new MailAddress(email);
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