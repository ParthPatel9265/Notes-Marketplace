using NotesMarketPlace.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using NotesMarketPlace.Models;
using System.Net;
using System.Net.Mail;
using System.Web.UI.WebControls;
using PagedList;

namespace NotesMarketPlace.Controllers
{
    public class SellNotesController : Controller
    {
        database1Entities dobj = new database1Entities();

        [HttpGet]
        [Authorize]
        [Route("SellNotes")] 
        public ActionResult Dashboard(string search1, string search2, string sort1, string sort2, int? page1 , int? page2 )
        {

            ViewBag.SellYourNotes = "active";
            ViewBag.Sort1 = sort1;
            ViewBag.Sort2 = sort2;
            
            ViewBag.Search1 = search1;
            ViewBag.Search2 = search2;

            Dashboard dashboard = new Dashboard();
            Context.Users user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();

            dashboard.NumberOfSoldNotes = dobj.Downloads.Where(x => x.Seller == user.ID && x.IsSellerHasAllowedDownload == true && x.AttachmentPath != null).Count();
            dashboard.MoneyEarned = dobj.Downloads.Where(x => x.Seller == user.ID && x.IsSellerHasAllowedDownload == true && x.AttachmentPath != null).Select(x => x.PurchasedPrice).Sum();
            dashboard.MyDownloads = dobj.Downloads.Where(x => x.Downloader == user.ID && x.IsSellerHasAllowedDownload == true && x.AttachmentPath != null).Count();
            dashboard.MyRejectedNotes = dobj.NoteDetail.Where(x => x.SellerID == user.ID && x.Status == 10 && x.IsActive == true).Count();
            dashboard.BuyerRequest = dobj.Downloads.Where(x => x.Seller == user.ID && x.IsSellerHasAllowedDownload == false && x.AttachmentPath == null).Count();

            IEnumerable<NoteDetail> inprogress;
            IEnumerable<NoteDetail> published;

            if (string.IsNullOrEmpty(search1))
            {
               inprogress = dobj.NoteDetail.Where(x => x.SellerID == user.ID && (x.Status == 6 || x.Status == 7 || x.Status == 8));
            }
            else
            {
                inprogress = dobj.NoteDetail.Where(x => x.SellerID == user.ID &&
                                                                  (x.Status == 6 || x.Status == 7 || x.Status == 8) &&
                                                                  (x.Title.ToLower().Contains(search1) || x.NoteCategories.Name.ToLower().Contains(search1) || x.ReferenceData.Value.ToLower().Contains(search1))
                                                                     );
            }

            if (string.IsNullOrEmpty(search2))
            {
                published = dobj.NoteDetail.Where(x => x.SellerID == user.ID && x.Status == 9);
            }
            else
            {
                search2 = search2.ToLower();
                published = dobj.NoteDetail.Where(x => x.SellerID == user.ID &&
                                                                 x.Status == 9 &&
                                                                 (x.Title.ToLower().Contains(search2) || x.NoteCategories.Name.ToLower().Contains(search2) || x.SellingPrice.ToString().ToLower().Contains(search2))
                                                                  );
            }

            inprogress = SortTableInProgressNote(sort1, inprogress);
            published = SortTablePublishNote(sort2, published);
           
            dashboard.InProgressNotes = inprogress.ToList().AsQueryable().ToPagedList(page1 ?? 1, 5);
          
            dashboard.PublishedNotes = published.ToList().AsQueryable().ToPagedList(page2 ?? 1 , 5);
            return View(dashboard);
        }

        private IEnumerable<NoteDetail> SortTableInProgressNote(string sort, IEnumerable<NoteDetail> table)
        {
            switch (sort)
            {
                case "CreatedDate_Asc":
                    {
                        table = table.OrderBy(x => x.CreatedDate);
                        break;
                    }
                case "CreatedDate_Desc":
                    {
                        table = table.OrderByDescending(x => x.CreatedDate);
                        break;
                    }
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
                case "Status_Asc":
                    {
                        table = table.OrderBy(x => x.ReferenceData.Value);
                        break;
                    }
                case "Status_Desc":
                    {
                        table = table.OrderByDescending(x => x.ReferenceData.Value);
                        break;
                    }
                default:
                    {
                        table = table.OrderByDescending(x => x.CreatedDate);
                        break;
                    }
            }
            return table;
        }

        private IEnumerable<NoteDetail> SortTablePublishNote(string sort, IEnumerable<NoteDetail> table)
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
                case "IsPaid_Asc":
                    {
                        table = table.OrderBy(x => x.IsPaid);
                        break;
                    }
                case "IsPaid_Desc":
                    {
                        table = table.OrderByDescending(x => x.IsPaid);
                        break;
                    }
                case "SellingPrice_Asc":
                    {
                        table = table.OrderBy(x => x.SellingPrice);
                        break;
                    }
                case "SellingPrice_Desc":
                    {
                        table = table.OrderByDescending(x => x.SellingPrice);
                        break;
                    }
                default:
                    {
                        table = table.OrderByDescending(x => x.PublishedDate);
                        break;
                    }
            }
            return table;
        }

        [HttpGet]
        [Authorize]
        [Route("SellNotes/AddNotes")]
        public ActionResult AddNotes()
        {

            AddNotes viewModel = new AddNotes
            {
                NoteCategoryList = dobj.NoteCategories.Where(x => x.IsActive == true).ToList(),
                NoteTypeList = dobj.NoteTypes.Where(x => x.IsActive == true).ToList(),
                CountryList = dobj.Countries.Where(x => x.IsActive == true).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("SellNotes/AddNotes")]
        public ActionResult AddNotes(AddNotes add,string command)
        {

            if (add.UploadNotes[0] == null)
            {
                ModelState.AddModelError("UploadNotes", "This field is required");
                add.NoteCategoryList = dobj.NoteCategories.Where(x => x.IsActive == true).ToList();
                add.NoteTypeList = dobj.NoteTypes.Where(x => x.IsActive == true).ToList();
                add.CountryList = dobj.Countries.Where(x => x.IsActive == true).ToList();
                return View(add);
            }

            if (add.IsPaid == true && add.NotesPreview == null)
            {
                ModelState.AddModelError("NotesPreview", "This field is required if selling type is paid");
                add.NoteCategoryList = dobj.NoteCategories.Where(x => x.IsActive == true).ToList();
                add.NoteTypeList = dobj.NoteTypes.Where(x => x.IsActive == true).ToList();
                add.CountryList = dobj.Countries.Where(x => x.IsActive == true).ToList();
                return View(add);
            }
            foreach (HttpPostedFileBase file in add.UploadNotes)
            {
                if (!System.IO.Path.GetExtension(file.FileName).Equals(".pdf"))
                {
                    ModelState.AddModelError("UploadNotes", "Only PDF Format is allowed");
                    add.NoteCategoryList = dobj.NoteCategories.Where(x => x.IsActive == true).ToList();
                    add.NoteTypeList = dobj.NoteTypes.Where(x => x.IsActive == true).ToList();
                    add.CountryList = dobj.Countries.Where(x => x.IsActive == true).ToList();
                    return View(add);
                }
            }
            if (ModelState.IsValid)
            {

                NoteDetail sellnote = new NoteDetail();

                Context.Users user = dobj.Users.FirstOrDefault(x => x.EmailID == User.Identity.Name);

                sellnote.SellerID = user.ID;
                sellnote.Title = add.Title;
                sellnote.Status = command == "Save" ? 6 : 7;
                sellnote.Category = add.Category;
                sellnote.NoteType = add.NoteType;
                sellnote.NumberofPages = add.NumberofPages;
                sellnote.Description = add.Description;
                sellnote.UniversityName = add.UniversityName;
                sellnote.Country = add.Country;
                sellnote.Course = add.Course;
                sellnote.CourseCode = add.CourseCode;
                sellnote.Professor = add.Professor;
                sellnote.IsPaid = add.IsPaid;
                if (sellnote.IsPaid)
                {
                    sellnote.SellingPrice = add.SellingPrice;
                }
                else
                {
                    sellnote.SellingPrice = 0;
                }

                if(sellnote.Status == 7)
                {
                    string sellername = user.FirstName + " " + user.LastName;
                    PublishNoteRequestmail(sellnote.Title, sellername);
                }
                  

                sellnote.CreatedDate = DateTime.Now;
                sellnote.CreatedBy = user.ID;
                sellnote.IsActive = true;

                dobj.NoteDetail.Add(sellnote);
                dobj.SaveChanges();

                sellnote = dobj.NoteDetail.Find(sellnote.ID);
                if (add.DisplayPicture != null)
                {
                    string displaypicturefilename = System.IO.Path.GetFileName(add.DisplayPicture.FileName);
                    string displaypicturepath = "~/Members/" + user.ID + "/" + sellnote.ID + "/";
                    NewDirectory(displaypicturepath);
                    string displaypicturefilepath = Path.Combine(Server.MapPath(displaypicturepath), displaypicturefilename);
                    sellnote.DisplayPicture = displaypicturepath + displaypicturefilename;
                    add.DisplayPicture.SaveAs(displaypicturefilepath);
                }

                if (add.NotesPreview != null)
                {
                    string notespreviewfilename = System.IO.Path.GetFileName(add.NotesPreview.FileName);
                    string notespreviewpath = "~/Members/" + user.ID + "/" + sellnote.ID + "/";
                    NewDirectory(notespreviewpath);
                    string notespreviewfilepath = Path.Combine(Server.MapPath(notespreviewpath), notespreviewfilename);
                    sellnote.NotesPreview = notespreviewpath + notespreviewfilename;
                    add.NotesPreview.SaveAs(notespreviewfilepath);
                }

                dobj.NoteDetail.Attach(sellnote);
                dobj.Entry(sellnote).Property(x => x.DisplayPicture).IsModified = true;
                dobj.Entry(sellnote).Property(x => x.NotesPreview).IsModified = true;
                dobj.SaveChanges();

                foreach (HttpPostedFileBase file in add.UploadNotes)
                {

                    if (file != null)
                    {
                        string notesattachementfilename = System.IO.Path.GetFileName(file.FileName);
                        string notesattachementpath = "~/Members/" + user.ID + "/" + sellnote.ID + "/Attachements/";
                        NewDirectory(notesattachementpath);
                        string notesattachementfilepath = Path.Combine(Server.MapPath(notesattachementpath), notesattachementfilename);
                        file.SaveAs(notesattachementfilepath);

                        NotesAttachments notesattachements = new NotesAttachments
                        {
                            NoteID = sellnote.ID,
                            FileName = notesattachementfilename,
                            FilePath = notesattachementpath,
                            CreatedDate = DateTime.Now,
                            CreatedBy = user.ID,
                            IsActive = true
                        };

                        dobj.NotesAttachments.Add(notesattachements);
                        dobj.SaveChanges();
                    }
                }

                return RedirectToAction("Dashboard", "SellNotes");

            }
            else
            {
                AddNotes viewModel = new AddNotes
                {
                    NoteCategoryList = dobj.NoteCategories.Where(x => x.IsActive == true).ToList(),
                    NoteTypeList = dobj.NoteTypes.Where(x => x.IsActive == true).ToList(),
                    CountryList = dobj.Countries.Where(x => x.IsActive == true).ToList()
                };

                return View(viewModel);

            }
        }

        [HttpGet]
        [Authorize]
        [Route("SellNotes/EditNotes/{id}")]
        public ActionResult EditNotes(int id)
        {

            Context.Users user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            NoteDetail note = dobj.NoteDetail.Where(x => x.ID == id && x.IsActive == true && x.SellerID == user.ID).FirstOrDefault();
            NotesAttachments attachement = dobj.NotesAttachments.Where(x => x.NoteID == id).FirstOrDefault();
            if (note != null)
            {
                EditNotes viewModel = new EditNotes
                {
                    ID = note.ID,
                    NoteID = note.ID,
                    Title = note.Title,
                    Category = note.Category,
                    Picture = note.DisplayPicture,
                    Note = attachement.FileName,
                    NumberofPages = note.NumberofPages,
                    Description = note.Description,
                    NoteType = note.NoteType,
                    UniversityName = note.UniversityName,
                    Course = note.Course,
                    CourseCode = note.CourseCode,
                    Country = note.Country,
                    Professor = note.Professor,
                    IsPaid = note.IsPaid,
                    SellingPrice = note.SellingPrice,
                    Preview = note.NotesPreview,
                    NoteCategoryList = dobj.NoteCategories.Where(x => x.IsActive == true).ToList(),
                    NoteTypeList = dobj.NoteTypes.Where(x => x.IsActive == true).ToList(),
                    CountryList= dobj.Countries.Where(x => x.IsActive == true).ToList(),

                };

                return View(viewModel);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("SellNotes/EditNotes/{id}")]
        public ActionResult EditNotes(int id, EditNotes notes)
        {
            if (ModelState.IsValid)
            {
                var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();

                var sellnote = dobj.NoteDetail.Where(x => x.ID == id && x.IsActive == true && x.SellerID == user.ID).FirstOrDefault();

                if (sellnote == null)
                {
                    return HttpNotFound();
                }

                if (notes.IsPaid == true && notes.Preview == null && sellnote.NotesPreview == null)
                {
                    ModelState.AddModelError("NotesPreview", "This field is required if selling type is paid");
                    return View(notes);
                }

                var notesattachement = dobj.NotesAttachments.Where(x => x.NoteID == notes.NoteID && x.IsActive == true).ToList();


                dobj.NoteDetail.Attach(sellnote);
                sellnote.Title = notes.Title;
                sellnote.Category = notes.Category;
                sellnote.NoteType = notes.NoteType;
                sellnote.NumberofPages = notes.NumberofPages;
                sellnote.Description = notes.Description;
                sellnote.Country = notes.Country;
                sellnote.UniversityName = notes.UniversityName;
                sellnote.Course = notes.Course;
                sellnote.CourseCode = notes.CourseCode;
                sellnote.Professor = notes.Professor;
                if (notes.IsPaid == true)
                {
                    sellnote.IsPaid = true;
                    sellnote.SellingPrice = notes.SellingPrice;
                }
                else
                {
                    sellnote.IsPaid = false;
                    sellnote.SellingPrice = 0;
                }
                sellnote.ModifiedDate = DateTime.Now;
                sellnote.ModifiedBy = user.ID;
                dobj.SaveChanges();

                if (notes.DisplayPicture != null)
                {

                    if (sellnote.DisplayPicture != null)
                    {
                        string path = Server.MapPath(sellnote.DisplayPicture);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }

                    string displaypicturefilename = System.IO.Path.GetFileName(notes.DisplayPicture.FileName);
                    string displaypicturepath = "~/Members/" + user.ID + "/" + sellnote.ID + "/";
                    NewDirectory(displaypicturepath);
                    string displaypicturefilepath = Path.Combine(Server.MapPath(displaypicturepath), displaypicturefilename);
                    sellnote.DisplayPicture = displaypicturepath + displaypicturefilename;
                    notes.DisplayPicture.SaveAs(displaypicturefilepath);
                }

                if (notes.NotesPreview != null)
                {

                    if (sellnote.NotesPreview != null)
                    {
                        string path = Server.MapPath(sellnote.NotesPreview);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }

                    string notespreviewfilename = System.IO.Path.GetFileName(notes.NotesPreview.FileName);
                    string notespreviewpath = "~/Members/" + user.ID + "/" + sellnote.ID + "/";
                    NewDirectory(notespreviewpath);
                    string notespreviewfilepath = Path.Combine(Server.MapPath(notespreviewpath), notespreviewfilename);
                    sellnote.NotesPreview = notespreviewpath + notespreviewfilename;
                    notes.NotesPreview.SaveAs(notespreviewfilepath);
              
                }


                if (notes.UploadNotes[0] != null)
                {

                    string path = Server.MapPath(notesattachement[0].FilePath);
                    DirectoryInfo dir = new DirectoryInfo(path);
                    EmptyFolder(dir);


                    foreach (var item in notesattachement)
                    {
                        NotesAttachments attachement = dobj.NotesAttachments.Where(x => x.ID == item.ID).FirstOrDefault();
                        dobj.NotesAttachments.Remove(attachement);
                    }


                    foreach (HttpPostedFileBase file in notes.UploadNotes)
                    {

                        if (file != null)
                        {

                            string notesattachementfilename = System.IO.Path.GetFileName(file.FileName);
                            string notesattachementpath = "~/Members/" + user.ID + "/" + sellnote.ID + "/Attachements/";
                            NewDirectory(notesattachementpath);
                            string notesattachementfilepath = Path.Combine(Server.MapPath(notesattachementpath), notesattachementfilename);
                            file.SaveAs(notesattachementfilepath);


                            NotesAttachments notesattachements = new NotesAttachments
                            {
                                NoteID = sellnote.ID,
                                FileName = notesattachementfilename,
                                FilePath = notesattachementpath,
                                CreatedDate = DateTime.Now,
                                CreatedBy = user.ID,
                                IsActive = true
                            };


                            dobj.NotesAttachments.Add(notesattachements);
                            dobj.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("Dashboard", "SellNotes");
            }
            else
            {
                return RedirectToAction("EditNotes", new { id = notes.ID });
            }

        }

        [Authorize]
        [Route("SellNotes/DeleteDraft/{id}")]
        public ActionResult DeleteDraft(int id)
        {

            NoteDetail note = dobj.NoteDetail.Where(x => x.ID == id && x.IsActive == true).FirstOrDefault();

            if (note == null)
            {
                return HttpNotFound();
            }

            IEnumerable<NotesAttachments> noteattachement = dobj.NotesAttachments.Where(x => x.NoteID == id && x.IsActive == true).ToList();

            if (noteattachement.Count() == 0)
            {
                return HttpNotFound();
            }

            string notefolderpath = Server.MapPath("~/Members/" + note.SellerID + "/" + note.ID);
            string noteattachmentfolderpath = Server.MapPath("~/Members/" + note.SellerID + "/" + note.ID + "/Attachements");

            DirectoryInfo notefolder = new DirectoryInfo(notefolderpath);
            DirectoryInfo attachementnotefolder = new DirectoryInfo(noteattachmentfolderpath);

            EmptyFolder(attachementnotefolder);
            EmptyFolder(notefolder);

            Directory.Delete(notefolderpath);

            dobj.NoteDetail.Remove(note);

            foreach (var item in noteattachement)
            {
                NotesAttachments attachement = dobj.NotesAttachments.Where(x => x.ID == item.ID).FirstOrDefault();
                dobj.NotesAttachments.Remove(attachement);
            }
            dobj.SaveChanges();

            return RedirectToAction("Dashboard", "SellNotes");
        }

        [Route("SellNotes/Publish")]
        [Authorize]
        public ActionResult PublishNoteRequest(int id)
        {
            var note = dobj.NoteDetail.Find(id);

            if (note == null)
            {
                return HttpNotFound();
            }

            var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            string sellername = user.FirstName + " " + user.LastName;

            if (user.ID == note.SellerID)
            {
                dobj.NoteDetail.Attach(note);
                note.Status = 7;
                note.ModifiedDate = DateTime.Now;
                note.ModifiedBy = user.ID;
                dobj.SaveChanges();
                PublishNoteRequestmail(note.Title, sellername);
            }

            return RedirectToAction("Dashboard");
        }

        public void PublishNoteRequestmail(string note, string seller)
        {
            var email = dobj.SystemConfiguration.Select(x => x.EmailID1).FirstOrDefault();
            var email2 = dobj.SystemConfiguration.Select(x => x.EmailID2).FirstOrDefault();

            var fromEmail = new MailAddress(email);
            var toEmail = new MailAddress(email2);
            //s.EmailID1 password
            var fromEmailPassword = "####";
            string subject = seller + " sent his note for review";

            string body = "Hello Admins," +
                "<br/><br/>We want to inform you that," + seller + " sent his note " +
                "<br/>" + note + " for review. Please look at the notes and take required actions." +
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

        private void EmptyFolder(DirectoryInfo directory)
        {
            if (directory.GetFiles() != null)
            {

                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
            }

            if (directory.GetDirectories() != null)
            {

                foreach (DirectoryInfo subdirectory in directory.GetDirectories())
                {
                    EmptyFolder(subdirectory);
                    subdirectory.Delete();
                }
            }

        }

        private void NewDirectory(string folderpath)
        {

            bool folderalreadyexists = Directory.Exists(Server.MapPath(folderpath));

            if (!folderalreadyexists)
                Directory.CreateDirectory(Server.MapPath(folderpath));
        }


    }


}
