using NotesMarketPlace.Context;
using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace NotesMarketplace.Controllers
{
    [OutputCache(Duration = 0)]
    [RoutePrefix("User")]
    public class UserProfileController : Controller
    {
         database1Entities dbobj = new database1Entities();

        [HttpGet]
        [Authorize(Roles = "Member")]
        [Route("Profile")]
        public ActionResult UserProfile()
        {
            
            var user = dbobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();

            var userprofile = dbobj.UserProfileDetail.Where(x => x.UserID == user.ID).FirstOrDefault();

            UserProfile userprofilemodel = new UserProfile();

            if (userprofile != null)
            {
                userprofilemodel.CountryList = dbobj.Countries.Where(x => x.IsActive == true).ToList();
                userprofilemodel.GenderList = dbobj.ReferenceData.Where(x => x.RefCategory == "Gender" && x.IsActive == true).ToList();
                userprofilemodel.UserID = user.ID;
                userprofilemodel.EmailID = user.EmailID;
                userprofilemodel.FirstName = user.FirstName;
                userprofilemodel.LastName = user.LastName;
                userprofilemodel.DOB = userprofile.DOB;
                userprofilemodel.CountryCode = userprofile.CountryCode;
                userprofilemodel.PhoneNumber = userprofile.PhoneNumber;
                userprofilemodel.Gender = userprofile.Gender;
                userprofilemodel.AddressLine1 = userprofile.AddressLine1;
                userprofilemodel.AddressLine2 = userprofile.AddressLine2;
                userprofilemodel.City = userprofile.City;
                userprofilemodel.State = userprofile.State;
                userprofilemodel.ZipCode = userprofile.ZipCode;
                userprofilemodel.Country = userprofile.Country;
                userprofilemodel.University = userprofile.University;
                userprofilemodel.College = userprofile.College;
                userprofilemodel.ProfilePictureUrl = userprofile.ProfilePicture;
            }
            
            else
            {
                userprofilemodel.CountryList = dbobj.Countries.Where(x => x.IsActive == true).ToList();
                userprofilemodel.GenderList = dbobj.ReferenceData.Where(x => x.RefCategory.ToLower() == "gender" && x.IsActive == true).ToList();
                userprofilemodel.UserID = user.ID;
                userprofilemodel.EmailID = user.EmailID;
                userprofilemodel.FirstName = user.FirstName;
                userprofilemodel.LastName = user.LastName;
            }

            return View(userprofilemodel);
        }

        [HttpPost]
        [Authorize(Roles = "Member")]
        [Route("Profile")]
        public ActionResult UserProfile(UserProfile userprofilemodel)
        {
        
            var user = dbobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (userprofilemodel.ProfilePicture != null)
                {
                    //image size limit 10MB
                    var profilepicsize = userprofilemodel.ProfilePicture.ContentLength;
                    if (profilepicsize > 10 * 1024 *1024)
                    {
                        // error if image size is more than 10MB
                        ModelState.AddModelError("ProfilePicture", "Image size limit is 10 MB");
                        userprofilemodel.CountryList = dbobj.Countries.Where(x => x.IsActive == true).ToList();
                        userprofilemodel.GenderList = dbobj.ReferenceData.Where(x => x.RefCategory == "Gender" && x.IsActive == true).ToList();
                        return View(userprofilemodel);
                    }
                }

               
                var profile = dbobj.UserProfileDetail.Where(x => x.UserID == user.ID).FirstOrDefault();

                //edit profile 
                if (profile != null)
                {
                    profile.DOB = userprofilemodel.DOB;
                    profile.Gender = userprofilemodel.Gender;
                    profile.CountryCode = userprofilemodel.CountryCode.Trim();
                    profile.PhoneNumber = userprofilemodel.PhoneNumber.Trim();
                    profile.AddressLine1 = userprofilemodel.AddressLine1.Trim();
                    profile.AddressLine2 = userprofilemodel.AddressLine2.Trim();
                    profile.City = userprofilemodel.City.Trim();
                    profile.State = userprofilemodel.State.Trim();
                    profile.ZipCode = userprofilemodel.ZipCode.Trim();
                    profile.Country = userprofilemodel.Country.Trim();
                    profile.University = userprofilemodel.University.Trim();
                    profile.College = userprofilemodel.College.Trim();
                    profile.ModifiedDate = DateTime.Now;
                    profile.ModifiedBy = user.ID;

                    // delete old profilepic
                    if (userprofilemodel.ProfilePicture != null && profile.ProfilePicture != null)
                    {
                        string path = Server.MapPath(profile.ProfilePicture);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }

                    // if user upload profile picture then save it and store path in database
                    if (userprofilemodel.ProfilePicture != null)
                    {
                        string filename = System.IO.Path.GetFileName(userprofilemodel.ProfilePicture.FileName);
                        string fileextension = System.IO.Path.GetExtension(userprofilemodel.ProfilePicture.FileName);
                        string newfilename = "DP_" + DateTime.Now.ToString("ddMMyyyy_hhmmss") + fileextension;
                        string profilepicturepath = "~/Members/" + profile.UserID + "/";
                        CreateNewDirectory(profilepicturepath);
                        string path = Path.Combine(Server.MapPath(profilepicturepath), newfilename);
                        profile.ProfilePicture = profilepicturepath + newfilename;
                        userprofilemodel.ProfilePicture.SaveAs(path);
                    }

                    
                    dbobj.Entry(profile).State = EntityState.Modified;
                    dbobj.SaveChanges();

                    user.FirstName = userprofilemodel.FirstName.Trim();
                    user.LastName = userprofilemodel.LastName.Trim();
                    dbobj.Entry(user).State = EntityState.Modified;
                    dbobj.SaveChanges();

                }
                // new userprofile
                else
                {
                  
                    UserProfileDetail userprofile = new UserProfileDetail();

                    userprofile.UserID = user.ID;
                    userprofile.DOB = userprofilemodel.DOB;
                    userprofile.Gender = userprofilemodel.Gender;
                    userprofile.CountryCode = userprofilemodel.CountryCode.Trim();
                    userprofile.PhoneNumber = userprofilemodel.PhoneNumber.Trim();
                    userprofile.AddressLine1 = userprofilemodel.AddressLine1.Trim();
                    userprofile.AddressLine2 = userprofilemodel.AddressLine2.Trim();
                    userprofile.City = userprofilemodel.City.Trim();
                    userprofile.State = userprofilemodel.State.Trim();
                    userprofile.ZipCode = userprofilemodel.ZipCode.Trim();
                    userprofile.Country = userprofilemodel.Country.Trim();
                    userprofile.University = userprofilemodel.University.Trim();
                    userprofile.College = userprofilemodel.College.Trim();
                    userprofile.CreatedDate = DateTime.Now;
                    userprofile.CreatedBy = user.ID;

                   
                    if (userprofilemodel.ProfilePicture != null)
                    {
                        string filename = System.IO.Path.GetFileName(userprofilemodel.ProfilePicture.FileName);
                        string fileextension = System.IO.Path.GetExtension(userprofilemodel.ProfilePicture.FileName);
                        string newfilename = "DP_" + DateTime.Now.ToString("ddMMyyyy_hhmmss") + fileextension;
                        string profilepicturepath = "~/Members/" + userprofile.UserID + "/";
                        CreateNewDirectory(profilepicturepath);
                        string path = Path.Combine(Server.MapPath(profilepicturepath), newfilename);
                        userprofile.ProfilePicture = profilepicturepath + newfilename;
                        userprofilemodel.ProfilePicture.SaveAs(path);
                    }

                    dbobj.UserProfileDetail.Add(userprofile);
                    dbobj.SaveChanges();

                    user.FirstName = userprofilemodel.FirstName.Trim();
                    user.LastName = userprofilemodel.LastName.Trim();
                    dbobj.Entry(user).State = EntityState.Modified;
                    dbobj.SaveChanges();
                }

                return RedirectToAction("SearchNotes", "SearchNotes");
            }

            //for invalid ModelState 
            else
            {
                userprofilemodel.CountryList = dbobj.Countries.Where(x => x.IsActive == true).ToList();
                userprofilemodel.GenderList = dbobj.ReferenceData.Where(x => x.RefCategory == "Gender" && x.IsActive == true).ToList();
                return View(userprofilemodel);
            }
        }

        // Create Directory
        private void CreateNewDirectory(string folderpath)
        {
           
            bool folderalreadyexists = Directory.Exists(Server.MapPath(folderpath));
            if (!folderalreadyexists)
                Directory.CreateDirectory(Server.MapPath(folderpath));
        }
    }
}