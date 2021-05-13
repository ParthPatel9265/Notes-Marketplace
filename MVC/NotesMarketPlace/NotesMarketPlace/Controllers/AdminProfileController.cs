using NotesMarketPlace.Models;
using NotesMarketPlace.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace NotesMarketPlace.Controllers
{
    // GET: AdminProfile
    [OutputCache(Duration = 0)]
    [RoutePrefix("Admin")]
    public class AdminProfileController : Controller
    {
        private readonly database1Entities dobj = new database1Entities();

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [Route("Profile")]
        public ActionResult MyProfile()
        {
            // get logged in user
            var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();

            // get logged in user's profile
            var userprofile = dobj.AdminDetail.Where(x => x.AdminID == user.ID).FirstOrDefault();

            // create adminprofile
            AdminProfile adminprofile = new AdminProfile();
            if (userprofile != null)
            {
                adminprofile.FirstName = user.FirstName;
                adminprofile.LastName = user.LastName;
                adminprofile.Email = user.EmailID;
                adminprofile.AdminID = user.ID;
                adminprofile.CountryCode = userprofile.CountryCode;
                adminprofile.PhoneNumber = userprofile.PhoneNumber;
                if (userprofile.SecondaryEmail != null)
                {
                    adminprofile.SecondaryEmail = userprofile.SecondaryEmail;
                }
                if (userprofile.ProfilePicture != null)
                {
                    adminprofile.ProfilePictureUrl = userprofile.ProfilePicture;

                }
                adminprofile.CountryCodeList = dobj.Countries.Where(x => x.IsActive).OrderBy(x => x.CountryCode).Select(x => x.CountryCode).ToList();
            }
            else
            {
                adminprofile.FirstName = user.FirstName;
                adminprofile.LastName = user.LastName;
                adminprofile.Email = user.EmailID;
                adminprofile.CountryCodeList = dobj.Countries.Where(x => x.IsActive).OrderBy(x => x.CountryCode).Select(x => x.CountryCode).ToList();
            }
                return View(adminprofile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Profile")]
        public ActionResult MyProfile(AdminProfile obj)
        {
            // get logged in user
            var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                // get  logged in user profile
                var userprofile = dobj.AdminDetail.Where(x => x.AdminID == user.ID).FirstOrDefault();

                if (userprofile != null)
                {

                    // check if secondary email is already exists in User or Admin table or not
                    // if email already exists then give error
                    bool secondaryemailalreadyexistsinusers = dobj.Users.Where(x => x.EmailID == obj.SecondaryEmail).Any();
                    bool secondaryemailalreadyexistsinadmin = dobj.AdminDetail.Where(x => x.SecondaryEmail == obj.Email && x.AdminID != user.ID).Any();
                    if (secondaryemailalreadyexistsinusers || secondaryemailalreadyexistsinadmin)
                    {
                        ModelState.AddModelError("SecondaryEmail", "This email address is already exists");
                        obj.CountryCodeList = dobj.Countries.Where(x => x.IsActive).OrderBy(x => x.CountryCode).Select(x => x.CountryCode).ToList();
                        return View(obj);
                    }

                    // update user's data            
                    user.FirstName = obj.FirstName.Trim();
                    user.LastName = obj.LastName.Trim();
                    // update userprofile's data
                    if (obj.SecondaryEmail != null)
                    {
                        userprofile.SecondaryEmail = obj.SecondaryEmail.Trim();
                    }
                    userprofile.CountryCode = obj.CountryCode.Trim();
                    userprofile.PhoneNumber = obj.PhoneNumber.Trim();

                    // user upploaded profile picture and there is also previous profile picture then delete previous profile picture 
                    if (userprofile.ProfilePicture != null && obj.ProfilePicture != null)
                    {
                        string path = Server.MapPath(userprofile.ProfilePicture);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }

                    // save new profile picture and update data in userprofile table
                    if (obj.ProfilePicture != null)
                    {
                        // get extension
                        string fileextension = System.IO.Path.GetExtension(obj.ProfilePicture.FileName);
                        // set new name of file
                        string newfilename = "DP_" + DateTime.Now.ToString("ddMMyyyy_hhmmss") + fileextension;
                        // set where to save picture
                        string profilepicturepath = "~/Members/" + userprofile.AdminID + "/";
                        // create directory if not exists
                        CreateDirectoryIfMissing(profilepicturepath);
                        // get physical path and save profile picture there
                        string path = Path.Combine(Server.MapPath(profilepicturepath), newfilename);
                        obj.ProfilePicture.SaveAs(path);
                        // save path in database
                        userprofile.ProfilePicture = profilepicturepath + newfilename;
                    }

                  
                    userprofile.ModifiedDate = DateTime.Now;
                    userprofile.ModifiedBy = user.ID;

                    dobj.Entry(user).State = EntityState.Modified;
                    dobj.Entry(userprofile).State = EntityState.Modified;
                    dobj.SaveChanges();
                }
                else
                {
                    AdminDetail admin = new AdminDetail();
                    admin.AdminID = user.ID;
                    bool secondaryemailalreadyexistsinusers = dobj.Users.Where(x => x.EmailID == obj.SecondaryEmail).Any();
                    bool secondaryemailalreadyexistsinuserprofile = dobj.AdminDetail.Where(x => x.SecondaryEmail == obj.Email && x.AdminID != user.ID).Any();
                    if (secondaryemailalreadyexistsinusers || secondaryemailalreadyexistsinuserprofile)
                    {
                        ModelState.AddModelError("SecondaryEmail", "This email address is already exists");
                        obj.CountryCodeList = dobj.Countries.Where(x => x.IsActive).OrderBy(x => x.CountryCode).Select(x => x.CountryCode).ToList();
                        return View(obj);
                    }

                    // update user's data            
                    user.FirstName = obj.FirstName.Trim();
                    user.LastName = obj.LastName.Trim();
                    // update userprofile's data
                    if (obj.SecondaryEmail != null)
                    {
                        admin.SecondaryEmail = obj.SecondaryEmail.Trim();
                    }
                    admin.CountryCode = obj.CountryCode.Trim();
                    admin.PhoneNumber = obj.PhoneNumber.Trim();

                    // user upploaded profile picture and there is also previous profile picture then delete previous profile picture
                    if (admin.ProfilePicture != null && obj.ProfilePicture != null)
                    {
                        string path = Server.MapPath(admin.ProfilePicture);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }

                    // save new profile picture and update data in userprofile table
                    if (obj.ProfilePicture != null)
                    {
                        // get extension
                        string fileextension = System.IO.Path.GetExtension(obj.ProfilePicture.FileName);
                        // set new name of file
                        string newfilename = "DP_" + DateTime.Now.ToString("ddMMyyyy_hhmmss") + fileextension;
                        // set where to save picture
                        string profilepicturepath = "~/Members/" + userprofile.AdminID + "/";
                        // create directory if not exists
                        CreateDirectoryIfMissing(profilepicturepath);
                        // get physical path and save profile picture there
                        string path = Path.Combine(Server.MapPath(profilepicturepath), newfilename);
                        obj.ProfilePicture.SaveAs(path);
                        // save path in database
                        admin.ProfilePicture = profilepicturepath + newfilename;
                    }

                    dobj.AdminDetail.Add(admin);
                    dobj.SaveChanges();

                    dobj.Entry(user).State = EntityState.Modified;
                    dobj.SaveChanges();
                }
                return RedirectToAction("Dashboard", "Admin");
            }

            else
            {
                obj.CountryCodeList = dobj.Countries.Where(x => x.IsActive).OrderBy(x => x.CountryCode).Select(x => x.CountryCode).ToList();
                return View(obj);

            }
        }

        private void CreateDirectoryIfMissing(string folderpath)
        {
            
            bool folderalreadyexists = Directory.Exists(Server.MapPath(folderpath));
            if (!folderalreadyexists)
                Directory.CreateDirectory(Server.MapPath(folderpath));
        }
    }
}