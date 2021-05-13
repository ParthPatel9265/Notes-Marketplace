using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotesMarketPlace.Models;
using NotesMarketPlace.Context;
using PagedList;
using PagedList.Mvc;
namespace NotesMarketPlace.Controllers
{
    [OutputCache(Duration = 0)]
    [RoutePrefix("User")]
    public class MySoldNotesController : Controller
    {

        database1Entities dobj = new database1Entities();
        // GET: MySoldNotes
        [HttpGet]
        [Authorize(Roles = "Member")]
        [Route("MySoldNotes")]
        public ActionResult MySoldNotes(string search,string sort,int?page)
        {
            ViewBag.Search = search;
            ViewBag.Sort = sort;
            ViewBag.PageNumber = page;

            //get logged in user
            var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();

            //get my sold notes
            IEnumerable<MySoldNotes> mysoldnotes = from download in dobj.Downloads
                                                            join users in dobj.Users on download.Downloader equals users.ID
                                                            where download.Seller == user.ID && download.IsSellerHasAllowedDownload == true && download.AttachmentPath != null
                                                            select new MySoldNotes
                                                            {
                                                                DownloadID = download.ID,
                                                                NoteID = download.NoteID,
                                                                Title = download.NoteTitle,
                                                                Category = download.NoteCategory,
                                                                Buyer = users.EmailID,
                                                                SellType = download.IsPaid == true ? "Paid" : "Free",
                                                                Price = download.PurchasedPrice,
                                                                DownloadedDate = download.AttachmentDownloadedDate.Value,
                                                                NoteDownloaded = download.IsAttachmentDownloaded
                                                            };

            //search result
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                mysoldnotes = mysoldnotes.Where(x => x.Title.ToLower().Contains(search) ||
                                                     x.Category.ToLower().Contains(search) ||
                                                     x.Buyer.ToLower().Contains(search) ||
                                                     x.Price.ToString().ToLower().Contains(search) ||
                                                     x.SellType.ToLower().Contains(search)
                                               ).ToList();
            }

            //sort result
            mysoldnotes = SortTableMySoldNotes(sort, mysoldnotes);


            var soldnote = new List<MySoldNotes>();
            soldnote = mysoldnotes.ToList();

            return View(soldnote.ToList().AsQueryable().ToPagedList(page ?? 1,10));
        }

        
        private IEnumerable<MySoldNotes> SortTableMySoldNotes(string sort, IEnumerable<MySoldNotes> table)
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

            
        }
}