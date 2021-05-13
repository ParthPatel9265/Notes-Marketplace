using System;
using NotesMarketPlace.Models;
using NotesMarketPlace.Context;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace NotesMarketPlace.Controllers
{
    [OutputCache(Duration = 0)]
    [RoutePrefix("Admin")]
    public class AdminMembersController : Controller
    {
        private readonly database1Entities dobj = new database1Entities();

        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Member")]
        public ActionResult Members(string search, string sort, int? page)
        {
            
            ViewBag.Search = search;
            ViewBag.Sort = sort;
            ViewBag.PageNumber = page;

            // get id of user role member
            var userrolememberid = dobj.UserRole.Where(x => x.Name.ToLower() == "member").Select(x => x.ID).FirstOrDefault();

            // get id for note status submitted for review, in review, published
            var submittedforreviewid = dobj.ReferenceData.Where(x => x.Value.ToLower() == "submitted").Select(x => x.ID).FirstOrDefault();
            var inreviewid = dobj.ReferenceData.Where(x => x.Value.ToLower() == "in review").Select(x => x.ID).FirstOrDefault();
            var publishednoteid = 9;

            // get member list 
            IEnumerable<Members> memberlist = from member in dobj.Users
                                                       where member.IsActive == true && member.IsEmailVerified == true && member.RoleID == userrolememberid
                                                       select new Members
                                                       {
                                                           ID = member.ID,
                                                           FirstName = member.FirstName,
                                                           LastName = member.LastName,
                                                           Email = member.EmailID,
                                                           JoiningDate = member.CreatedDate.Value,
                                                           UnderReviewNotes = dobj.NoteDetail.Where(x => x.SellerID == member.ID && (x.Status == submittedforreviewid || x.Status == inreviewid)).Count(),
                                                           PublishedNotes = dobj.NoteDetail.Where(x => x.SellerID == member.ID && x.Status == publishednoteid).Count(),
                                                           DownloadedNotes = dobj.Downloads.Where(x => x.Downloader == member.ID && x.IsSellerHasAllowedDownload == true).Count(),
                                                           TotalExpenses = dobj.Downloads.Where(x => x.Downloader == member.ID && x.IsSellerHasAllowedDownload == true).Select(x => x.PurchasedPrice).Sum(),
                                                           TotalEarning = dobj.Downloads.Where(x => x.Seller == member.ID && x.IsSellerHasAllowedDownload == true).Select(x => x.PurchasedPrice).Sum()
                                                       };

            // search 
            if (!String.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                memberlist = memberlist.Where(x => x.FirstName.ToLower().Contains(search) ||
                                                   x.LastName.ToLower().Contains(search) ||
                                                   x.Email.ToLower().Contains(search) ||
                                                   x.JoiningDate.ToString("dd-MM-yyyy, hh:mm").Contains(search) ||
                                                   x.TotalExpenses.ToString().Contains(search) ||
                                                   x.TotalEarning.ToString().Contains(search)).ToList();
            }

            // sorting result
            memberlist = SortTableMembers(sort, memberlist);

            return View(memberlist.ToList().AsQueryable().ToPagedList(page?? 1, 5));
        }

        // sorting for Member Table
        private IEnumerable<Members> SortTableMembers(string sort, IEnumerable<Members> table)
        {
            switch (sort)
            {
                case "FirstName_Asc":
                    {
                        table = table.OrderBy(x => x.FirstName);
                        break;
                    }
                case "FirstName_Desc":
                    {
                        table = table.OrderByDescending(x => x.FirstName);
                        break;
                    }
                case "LastName_Asc":
                    {
                        table = table.OrderBy(x => x.LastName);
                        break;
                    }
                case "LastName_Desc":
                    {
                        table = table.OrderByDescending(x => x.LastName);
                        break;
                    }
                case "Email_Asc":
                    {
                        table = table.OrderBy(x => x.Email);
                        break;
                    }
                case "Email_Desc":
                    {
                        table = table.OrderByDescending(x => x.Email);
                        break;
                    }
                case "JoiningDate_Asc":
                    {
                        table = table.OrderBy(x => x.JoiningDate);
                        break;
                    }
                case "JoiningDate_Desc":
                    {
                        table = table.OrderByDescending(x => x.JoiningDate);
                        break;
                    }
                case "UnderReviewNotes_Asc":
                    {
                        table = table.OrderBy(x => x.UnderReviewNotes);
                        break;
                    }
                case "UnderReviewNotes_Desc":
                    {
                        table = table.OrderByDescending(x => x.UnderReviewNotes);
                        break;
                    }
                case "PublishedNotes_Asc":
                    {
                        table = table.OrderBy(x => x.PublishedNotes);
                        break;
                    }
                case "PublishedNotes_Desc":
                    {
                        table = table.OrderByDescending(x => x.PublishedNotes);
                        break;
                    }
                case "DownloadedNotes_Asc":
                    {
                        table = table.OrderBy(x => x.DownloadedNotes);
                        break;
                    }
                case "DownloadedNotes_Desc":
                    {
                        table = table.OrderByDescending(x => x.DownloadedNotes);
                        break;
                    }
                case "TotalExpenses_Asc":
                    {
                        table = table.OrderBy(x => x.TotalExpenses);
                        break;
                    }
                case "TotalExpenses_Desc":
                    {
                        table = table.OrderByDescending(x => x.TotalExpenses);
                        break;
                    }
                case "TotalEarning_Asc":
                    {
                        table = table.OrderBy(x => x.TotalEarning);
                        break;
                    }
                case "TotalEarning_Desc":
                    {
                        table = table.OrderByDescending(x => x.TotalEarning);
                        break;
                    }
                default:
                    {
                        table = table.OrderByDescending(x => x.JoiningDate);
                        break;
                    }
            }
            return table;
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Member/{member}")]
        public ActionResult MemberDetail(int member, string sort, int? page )
        {      
            ViewBag.Sort = sort;
            ViewBag.PageNumber = page;
            ViewBag.Member = member;

            // get id of note status draft
            var draftid = dobj.ReferenceData.Where(x => x.Value.ToLower() == "draft").Select(x => x.ID).FirstOrDefault();

            // get member
            Context.Users users = dobj.Users.Where(x => x.ID == member).FirstOrDefault();
           
            UserProfileDetail userprofiles = dobj.UserProfileDetail.Where(x => x.UserID == users.ID).FirstOrDefault();
          
            // get member's notes excluding note status draft
            var notes = dobj.NoteDetail.Where(x => x.SellerID == users.ID && x.Status != draftid).ToList();

            // create list of MemberDetail.MembersNote
            var notelist = new List<MemberDetail.MembersNote>();

            foreach (var note in notes)
            {
                // get data of downloaded notes for count how many people downloaded notes and get total earning
                var downloadednoteslist = dobj.Downloads.Where(x => x.NoteID == note.ID && x.IsSellerHasAllowedDownload == true && x.AttachmentPath != null);

                // create membernote object of MemberDetail.MembersNote
                var membernote = new MemberDetail.MembersNote();
                membernote.ID = note.ID;
                membernote.Title = note.Title;
                membernote.Category = note.NoteCategories.Name;
                membernote.Status = note.ReferenceData.Value;
                membernote.DownloadedNotes = downloadednoteslist.Count();
                membernote.TotalEarning = downloadednoteslist.Select(x => x.PurchasedPrice).Sum();
                membernote.DateAdded = note.CreatedDate;
                membernote.PublishedDate = note.PublishedDate;

                // add membernote object to notelist
                notelist.Add(membernote);
            }

            // create object of MemberDetail
            MemberDetail members = new MemberDetail();
            members.FirstName = users.FirstName;
            members.LastName = users.LastName;
            members.Email = users.EmailID;
            if (userprofiles != null)
            {
                members.DOB = userprofiles.DOB;
                members.PhoneNumberCountryCode = userprofiles.CountryCode;
                members.PhoneNumber = userprofiles.PhoneNumber;
                members.College = userprofiles.University;
                members.Address1 = userprofiles.AddressLine1;
                members.Address2 = userprofiles.AddressLine2;
                members.City = userprofiles.City;
                members.State = userprofiles.State;
                members.ZipCode = userprofiles.ZipCode;
                members.Country = userprofiles.Country;
                members.ProfilePicture = userprofiles.ProfilePicture;
            }

            IEnumerable<MemberDetail.MembersNote> note1 = notelist.AsEnumerable();
            
            // sorting member notes result
            note1 = SortTableMemberNotes(sort, note1);

            members.Notes = note1.ToList().AsQueryable().ToPagedList(page ?? 1, 5);
            
            return View(members);
        }

        // sorting for Member Detail Table
        private IEnumerable<MemberDetail.MembersNote> SortTableMemberNotes(string sort, IEnumerable<MemberDetail.MembersNote> table)
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
                case "Status_Asc":
                    {
                        table = table.OrderBy(x => x.Status);
                        break;
                    }
                case "Status_Desc":
                    {
                        table = table.OrderByDescending(x => x.Status);
                        break;
                    }
                case "DownloadedNotes_Asc":
                    {
                        table = table.OrderBy(x => x.DownloadedNotes);
                        break;
                    }
                case "DownloadedNotes_Desc":
                    {
                        table = table.OrderByDescending(x => x.DownloadedNotes);
                        break;
                    }
                case "TotalEarning_Asc":
                    {
                        table = table.OrderBy(x => x.TotalEarning);
                        break;
                    }
                case "TotalEarning_Desc":
                    {
                        table = table.OrderByDescending(x => x.TotalEarning);
                        break;
                    }
                case "DateAdded_Asc":
                    {
                        table = table.OrderBy(x => x.DateAdded);
                        break;
                    }
                case "DateAdded_Desc":
                    {
                        table = table.OrderByDescending(x => x.DateAdded);
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
                default:
                    {
                        table = table.OrderBy(x => x.DateAdded);
                        break;
                    }
            }
            return table;
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Member/Deactive/{memberid}")]
        public ActionResult DeactiveMember(int memberid)
        {
            // get logged in admin
            var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).First();

            // get ids of note status removed and published
            var removedid = dobj.ReferenceData.Where(x => x.Value.ToLower() == "removed").Select(x => x.ID).FirstOrDefault();
            var publishedid = dobj.ReferenceData.Where(x => x.Value.ToLower() == "published").Select(x => x.ID).FirstOrDefault();

            // get member by member id
            var member = dobj.Users.Where(x => x.ID == memberid && x.IsActive == true).First();

            // make member inactive
            member.IsActive = false;
            member.ModifiedDate = DateTime.Now;
            member.ModifiedBy = user.ID;

            // save updated member record
            dobj.Entry(member).State = EntityState.Modified;
            dobj.SaveChanges();

            // get member's published notes list
            var notelist = dobj.NoteDetail.Where(x => x.SellerID == member.ID && x.Status == publishedid && x.IsActive == true).ToList();

            // make member's each published note status removed
            foreach (var note in notelist)
            {
                note.Status = removedid;
                note.ModifiedDate = DateTime.Now;
                note.ModifiedBy = user.ID;

                dobj.Entry(note).State = EntityState.Modified;
                dobj.SaveChanges();
            }

            return RedirectToAction("Members", "AdminMembers");
        }
    }
}