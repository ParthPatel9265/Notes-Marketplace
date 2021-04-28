using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotesMarketPlace.Context;
using NotesMarketPlace.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.IO.Compression;
namespace NotesMarketPlace.Controllers
{
    public class MyRejectedNotesController : Controller
    {
         database1Entities dobj = new database1Entities();
        // GET: MyRejectedNotes

        public ActionResult MyRejectedNotes(string search, string sort, int?page)
        {
           
            ViewBag.Search = search;
            ViewBag.Sort = sort;
            ViewBag.PageNumber = page;

            // logged in user 
            var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();

            //user's rejected notes
            IEnumerable<NoteDetail> rejectednotes = dobj.NoteDetail.Where(x => x.SellerID == user.ID && x.Status == 10 && x.IsActive == true).ToList();

            // searching result
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                rejectednotes = rejectednotes.Where(x => x.Title.ToLower().Contains(search) ||
                                                     x.NoteCategories.Name.ToLower().Contains(search) ||
                                                     x.AdminRemarks.ToLower().Contains(search)).ToList();
            }

            // sort result
            rejectednotes = SortTableMyRejectedNotes(sort, rejectednotes);

          

            var myrejectednotes = new List<NoteDetail>();
            myrejectednotes = rejectednotes.ToList();

            // return results
            return View(myrejectednotes.ToList().AsQueryable().ToPagedList(page ?? 1, 10));

        }

      
        private IEnumerable<NoteDetail> SortTableMyRejectedNotes(string sort, IEnumerable<NoteDetail> table)
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
                        table = table.OrderBy(x => x.NoteCategories.Name);
                        break;
                    }
                case "Category_Desc":
                    {
                        table = table.OrderByDescending(x => x.NoteCategories.Name);
                        break;
                    }
                case "Remark_Asc":
                    {
                        table = table.OrderBy(x => x.AdminRemarks);
                        break;
                    }
                case "Remark_Desc":
                    {
                        table = table.OrderByDescending(x => x.AdminRemarks);
                        break;
                    }
                default:
                    {
                        table = table.OrderByDescending(x => x.ModifiedDate);
                        break;
                    }
            }
            return table;
          
        }

        [HttpGet]
        [Authorize]
        [Route("MyRejectedNotes/{noteid}/Clone")]
        public ActionResult CloneNote(int noteid)
        {
            // logged in user
            var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();

            //get id of rejected notes
            var rejectednote = dobj.NoteDetail.Find(noteid);

            // creare clone of note
            NoteDetail clonenote = new NoteDetail();

            clonenote.SellerID = rejectednote.SellerID;
            clonenote.Status = 6;
            clonenote.Title = rejectednote.Title;
            clonenote.Category = rejectednote.Category;
            clonenote.NoteType = rejectednote.NoteType;
            clonenote.NumberofPages = rejectednote.NumberofPages;
            clonenote.Description = rejectednote.Description;
            clonenote.UniversityName = rejectednote.UniversityName;
            clonenote.Country = rejectednote.Country;
            clonenote.Course = rejectednote.Course;
            clonenote.CourseCode = rejectednote.CourseCode;
            clonenote.Professor = rejectednote.Professor;
            clonenote.IsPaid = rejectednote.IsPaid;
            clonenote.SellingPrice = rejectednote.SellingPrice;
            clonenote.CreatedBy = user.ID;
            clonenote.CreatedDate = DateTime.Now;
            clonenote.IsActive = true;

            // save note in database
            dobj.NoteDetail.Add(clonenote);
            dobj.SaveChanges();

            // get clonenote 
            clonenote = dobj.NoteDetail.Find(clonenote.ID);

            // if display picture is not null then copy file from rejected note's folder to clone note's new folder
            if (rejectednote.DisplayPicture != null)
            {
                var rejectednotefilepath = Server.MapPath(rejectednote.DisplayPicture);
                var clonenotefilepath = "~/Members/" + user.ID + "/" + clonenote.ID + "/";

                var filepath = Path.Combine(Server.MapPath(clonenotefilepath));

                FileInfo file = new FileInfo(rejectednotefilepath);

                Directory.CreateDirectory(filepath);
                if (file.Exists)
                {
                    System.IO.File.Copy(rejectednotefilepath, Path.Combine(filepath, Path.GetFileName(rejectednotefilepath)));
                }

                clonenote.DisplayPicture = Path.Combine(clonenotefilepath, Path.GetFileName(rejectednotefilepath));
                dobj.SaveChanges();
            }

            // if note preview is not null then copy file from rejected note's folder to clone note's new folder
            if (rejectednote.NotesPreview != null)
            {
                var rejectednotefilepath = Server.MapPath(rejectednote.NotesPreview);
                var clonenotefilepath = "~/Members/" + user.ID + "/" + clonenote.ID + "/";

                var filepath = Path.Combine(Server.MapPath(clonenotefilepath));

                FileInfo file = new FileInfo(rejectednotefilepath);

                Directory.CreateDirectory(filepath);

                if (file.Exists)
                {
                    System.IO.File.Copy(rejectednotefilepath, Path.Combine(filepath, Path.GetFileName(rejectednotefilepath)));
                }

                clonenote.NotesPreview = Path.Combine(clonenotefilepath, Path.GetFileName(rejectednotefilepath));
                dobj.SaveChanges();
            }

            // attachment path of rejected note and clone note
            var rejectednoteattachement = Server.MapPath("~/Members/" + user.ID + "/" + rejectednote.ID + "/Attachements/");
            var clonenoteattachement = "~/Members/" + user.ID + "/" + clonenote.ID + "/Attachements/";

            var attachementfilepath = Path.Combine(Server.MapPath(clonenoteattachement));

            // create directory for attachement folder
            Directory.CreateDirectory(attachementfilepath);

            // get attachements files from rejected note and copy to clone note
            foreach (var files in Directory.GetFiles(rejectednoteattachement))
            {

                FileInfo file = new FileInfo(files);

                if (file.Exists)
                {
                    System.IO.File.Copy(file.ToString(), Path.Combine(attachementfilepath, Path.GetFileName(file.ToString())));
                }

                // save attachment in database
                NotesAttachments attachement = new NotesAttachments();
                attachement.NoteID = clonenote.ID;
                attachement.FileName = Path.GetFileName(file.ToString());
                attachement.FilePath = clonenoteattachement;
                attachement.CreatedDate = DateTime.Now;
                attachement.CreatedBy = user.ID;
                attachement.IsActive = true;

                dobj.NotesAttachments.Add(attachement);
                dobj.SaveChanges();
            }
            return RedirectToAction("Dashboard", "SellNotes");
        }
    }


}