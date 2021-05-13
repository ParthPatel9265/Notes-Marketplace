using NotesMarketPlace.Models;
using NotesMarketPlace.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
namespace NotesMarketplace.Controllers
{
    [OutputCache(Duration = 0)]
    [RoutePrefix("Admin")]
    public class AdminSettingsController : Controller
    {
        private readonly database1Entities dobj = new database1Entities();

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Settings/ManageCategory/Add")]
        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Settings/ManageCategory/Add")]
        public ActionResult AddCategory(AddCategory obj)
        {
            if (ModelState.IsValid)
            {
                
                var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
                NoteCategories noteCategory = new NoteCategories
                {
                    Name = obj.Name.Trim(),
                    Description = obj.Description.Trim(),
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.ID,
                    IsActive = true
                };

                dobj.NoteCategories.Add(noteCategory);
                dobj.SaveChanges();

                return RedirectToAction("ManageCategory");
            }
            else
            {
                return View(obj);
            }
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Settings/ManageCategory/Edit/{id}")]
        public ActionResult EditCategory(int id)
        {
            
            var category = dobj.NoteCategories.Where(x => x.ID == id).FirstOrDefault();
            if (category == null)
            {
                return HttpNotFound();
            }

            AddCategory editCategory = new AddCategory
            {
                ID = category.ID,
                Name = category.Name,
                Description = category.Description
            };

            return View(editCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Settings/ManageCategory/Edit/{id}")]
        public ActionResult EditCategory(AddCategory obj)
        {
            if (ModelState.IsValid)
            {
                var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
                var category = dobj.NoteCategories.Where(x => x.ID == obj.ID).FirstOrDefault();

                if (category == null)
                {
                    return HttpNotFound();
                }

                category.Name = obj.Name.Trim();
                category.Description = obj.Description.Trim();
                category.ModifiedDate = DateTime.Now;
                category.ModifiedBy = user.ID;

                dobj.Entry(category).State = EntityState.Modified;
                dobj.SaveChanges();

                return RedirectToAction("ManageCategory");
            }
            else
            {
                return View(obj);
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Settings/ManageCategory")]
        public ActionResult ManageCategory(string search, string sort, int?page)
        {
           
            ViewBag.Search = search;
            ViewBag.Sort = sort;
            ViewBag.PageNumber = page;

            IEnumerable<ManageCategory> categorylist = from category in dobj.NoteCategories
                                                       join user in dobj.Users on category.CreatedBy equals user.ID
                                                       select new ManageCategory
                                                       {
                                                           ID = category.ID,
                                                           Category = category.Name,
                                                           Description = category.Description,
                                                           DateAdded = category.CreatedDate.Value,
                                                           AddedBy = user.FirstName + " " + user.LastName,
                                                           Active = category.IsActive == true ? "YES" : "NO"
                                                       };

            // search result
            if (!String.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                categorylist = categorylist.Where(x => x.Category.ToLower().Contains(search) ||
                                                       x.Description.ToLower().Contains(search) ||
                                                       x.DateAdded.ToString("dd-MM-yyyy hh:mm").Contains(search) ||
                                                       x.AddedBy.ToLower().Contains(search) ||
                                                       x.Active.ToLower().Contains(search)
                                                 ).ToList();
            }

            // sort result
            categorylist = SortTableCategory(sort, categorylist);

             return View(categorylist.ToList().AsQueryable().ToPagedList(page??1,5));

        }

        // sorting for category
        private IEnumerable<ManageCategory> SortTableCategory(string sort, IEnumerable<ManageCategory> table)
        {
            switch (sort)
            {
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
                case "Description_Asc":
                    {
                        table = table.OrderBy(x => x.Description);
                        break;
                    }
                case "Description_Desc":
                    {
                        table = table.OrderByDescending(x => x.Description);
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
                case "AddedBy_Asc":
                    {
                        table = table.OrderBy(x => x.AddedBy);
                        break;
                    }
                case "AddedBy_Desc":
                    {
                        table = table.OrderByDescending(x => x.AddedBy);
                        break;
                    }
                case "Active_Asc":
                    {
                        table = table.OrderBy(x => x.Active);
                        break;
                    }
                case "Active_Desc":
                    {
                        table = table.OrderByDescending(x => x.Active);
                        break;
                    }
                default:
                    {
                        table = table.OrderByDescending(x => x.DateAdded);
                        break;
                    }
            }
            return table;
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Settings/ManageCategory/Delete/{id}")]
        public ActionResult DeleteCategory(int id)
        {
          
            var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var category = dobj.NoteCategories.Where(x => x.ID == id).FirstOrDefault();

            if (category == null)
            {
                return HttpNotFound();
            }

            category.ModifiedDate = DateTime.Now;
            category.ModifiedBy = user.ID;
            category.IsActive = false;

            dobj.Entry(category).State = EntityState.Modified;
            dobj.SaveChanges();

            return RedirectToAction("ManageCategory");
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Settings/ManageType/Add")]
        public ActionResult AddType()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Settings/ManageType/Add")]
        public ActionResult AddType(AddType obj)
        {
            if (ModelState.IsValid)
            {
                
                var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
                NoteTypes noteType = new NoteTypes
                {
                    Name = obj.Name.Trim(),
                    Description = obj.Description.Trim(),
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.ID,
                    IsActive = true

                };

                dobj.NoteTypes.Add(noteType);
                dobj.SaveChanges();

                return RedirectToAction("ManageType");
            }
            else
            {
                return View(obj);
            }
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Settings/ManageType/Edit/{id}")]
        public ActionResult EditType(int id)
        {
          
            var type = dobj.NoteTypes.Where(x => x.ID == id).FirstOrDefault();
            if (type == null)
            {
                return HttpNotFound();
            }

            AddType editType = new AddType
            {
                ID = type.ID,
                Name = type.Name,
                Description = type.Description
            };

            return View(editType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Settings/ManageType/Edit/{id}")]
        public ActionResult EditType(AddType obj)
        {
            if (ModelState.IsValid)
            {
            
                var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
                var type = dobj.NoteTypes.Where(x => x.ID == obj.ID).FirstOrDefault();
                if (type == null)
                {
                    return HttpNotFound();
                }

                type.Name = obj.Name.Trim();
                type.Description = obj.Description.Trim();
                type.ModifiedDate = DateTime.Now;
                type.ModifiedBy = user.ID;

                dobj.Entry(type).State = EntityState.Modified;
                dobj.SaveChanges();

                return RedirectToAction("ManageType");
            }
            else
            {
                return View(obj);
            }
        }
       
        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Settings/ManageType")]
        public ActionResult ManageType(string search, string sort, int? page)
        {
        
            ViewBag.Search = search;
            ViewBag.Sort = sort;
            ViewBag.PageNumber = page;

            IEnumerable<ManageType> typelist = from type in dobj.NoteTypes
                                               join user in dobj.Users on type.CreatedBy equals user.ID
                                               select new ManageType
                                               {
                                                   ID = type.ID,
                                                   Type = type.Name,
                                                   Description = type.Description,
                                                   DateAdded = type.CreatedDate.Value,
                                                   AddedBy = user.FirstName + " " + user.LastName,
                                                   Active = type.IsActive == true ? "YES" : "NO"
                                               };

            // search result
            if (!String.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                typelist = typelist.Where(x => x.Type.ToLower().Contains(search) ||
                                               x.Description.ToLower().Contains(search) ||
                                               x.DateAdded.ToString("dd-MM-yyyy hh:mm").Contains(search) ||
                                               x.AddedBy.ToLower().Contains(search) ||
                                               x.Active.ToLower().Contains(search)
                                          ).ToList();
            }

            // sort results
            typelist = SortTableType(sort, typelist);

            return View(typelist.ToList().AsQueryable().ToPagedList(page ?? 1, 5));
        }

        // sorting for type
        private IEnumerable<ManageType> SortTableType(string sort, IEnumerable<ManageType> table)
        {
            switch (sort)
            {
                case "Type_Asc":
                    {
                        table = table.OrderBy(x => x.Type);
                        break;
                    }
                case "Type_Desc":
                    {
                        table = table.OrderByDescending(x => x.Type);
                        break;
                    }
                case "Description_Asc":
                    {
                        table = table.OrderBy(x => x.Description);
                        break;
                    }
                case "Description_Desc":
                    {
                        table = table.OrderByDescending(x => x.Description);
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
                case "AddedBy_Asc":
                    {
                        table = table.OrderBy(x => x.AddedBy);
                        break;
                    }
                case "AddedBy_Desc":
                    {
                        table = table.OrderByDescending(x => x.AddedBy);
                        break;
                    }
                case "Active_Asc":
                    {
                        table = table.OrderBy(x => x.Active);
                        break;
                    }
                case "Active_Desc":
                    {
                        table = table.OrderByDescending(x => x.Active);
                        break;
                    }
                default:
                    {
                        table = table.OrderByDescending(x => x.DateAdded);
                        break;
                    }
            }
            return table;
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Settings/ManageType/Delete/{id}")]
        public ActionResult DeleteType(int id)
        {
  
            var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var type = dobj.NoteTypes.Where(x => x.ID == id).FirstOrDefault();

            if (type == null)
            {
                return HttpNotFound();
            }

            type.ModifiedDate = DateTime.Now;
            type.ModifiedBy = user.ID;
            type.IsActive = false;

            dobj.Entry(type).State = EntityState.Modified;
            dobj.SaveChanges();

            return RedirectToAction("ManageType");
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Settings/ManageCountry/Add")]
        public ActionResult AddCountry()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Settings/ManageCountry/Add")]
        public ActionResult AddCountry(AddCountry obj)
        {
            if (ModelState.IsValid)
            {
       
                var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
                Countries country = new Countries
                {
                    Name = obj.CountryName.Trim(),
                    CountryCode = obj.CountryCode.Trim(),
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.ID,
                    IsActive = true
                };

                dobj.Countries.Add(country);
                dobj.SaveChanges();

                return RedirectToAction("ManageCountry");
            }
            else
            {
                return View(obj);
            }
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Settings/ManageCountry/Edit/{id}")]
        public ActionResult EditCountry(int id)
        {

            var country = dobj.Countries.Where(x => x.ID == id).FirstOrDefault();
            if (country == null)
            {
                return HttpNotFound();
            }

            AddCountry editCountry = new AddCountry
            {
                ID = country.ID,
                CountryName = country.Name,
                CountryCode = country.CountryCode
            };

            return View(editCountry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Settings/ManageCountry/Edit/{id}")]
        public ActionResult EditCountry(AddCountry obj)
        {
            if (ModelState.IsValid)
            {
      
                var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
                var country = dobj.Countries.Where(x => x.ID == obj.ID).FirstOrDefault();

                if (country == null)
                {
                    return HttpNotFound();
                }

                country.Name = obj.CountryName.Trim();
                country.CountryCode = obj.CountryCode.Trim();
                country.ModifiedDate = DateTime.Now;
                country.ModifiedBy = user.ID;

                dobj.Entry(country).State = EntityState.Modified;
                dobj.SaveChanges();

                return RedirectToAction("ManageCountry");
            }
            else
            {
                return View(obj);
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Settings/ManageCountry")]
        public ActionResult ManageCountry(string search, string sort, int?page )
        {
            ViewBag.Search = search;
            ViewBag.Sort = sort;
            ViewBag.PageNumber = page;

            IEnumerable<ManageCountry> countrylist = from country in dobj.Countries
                                                     join user in dobj.Users on country.CreatedBy equals user.ID
                                                     select new ManageCountry
                                                     {
                                                         ID = country.ID,
                                                         CountryName = country.Name,
                                                         CountryCode = country.CountryCode,
                                                         DateAdded = country.CreatedDate.Value,
                                                         AddedBy = user.FirstName + " " + user.LastName,
                                                         Active = country.IsActive == true ? "YES" : "NO"
                                                     };

            //search result
            if (!String.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                countrylist = countrylist.Where(x => x.CountryName.ToLower().Contains(search) ||
                                                     x.CountryCode.ToLower().Contains(search) ||
                                                     x.DateAdded.ToString("dd-MM-yyyy hh:mm").Contains(search) ||
                                                     x.AddedBy.ToLower().Contains(search) ||
                                                     x.Active.ToLower().Contains(search)
                                                ).ToList();
            }

            // sort results
            countrylist = SortTableCountry(sort, countrylist);

        
            return View(countrylist.ToList().AsQueryable().ToPagedList(page ?? 1, 5));
        }

        // sorting for country
        private IEnumerable<ManageCountry> SortTableCountry(string sort, IEnumerable<ManageCountry> table)
        {
            switch (sort)
            {
                case "CountryName_Asc":
                    {
                        table = table.OrderBy(x => x.CountryName);
                        break;
                    }
                case "CountryName_Desc":
                    {
                        table = table.OrderByDescending(x => x.CountryName);
                        break;
                    }
                case "CountryCode_Asc":
                    {
                        table = table.OrderBy(x => x.CountryCode);
                        break;
                    }
                case "CountryCode_Desc":
                    {
                        table = table.OrderByDescending(x => x.CountryCode);
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
                case "AddedBy_Asc":
                    {
                        table = table.OrderBy(x => x.AddedBy);
                        break;
                    }
                case "AddedBy_Desc":
                    {
                        table = table.OrderByDescending(x => x.AddedBy);
                        break;
                    }
                case "Active_Asc":
                    {
                        table = table.OrderBy(x => x.Active);
                        break;
                    }
                case "Active_Desc":
                    {
                        table = table.OrderByDescending(x => x.Active);
                        break;
                    }
                default:
                    {
                        table = table.OrderByDescending(x => x.DateAdded);
                        break;
                    }
            }
            return table;
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("Settings/ManageCountry/Delete/{id}")]
        public ActionResult DeleteCountry(int id)
        {
   
            var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var country = dobj.Countries.Where(x => x.ID == id).FirstOrDefault();

            if (country == null)
            {
                return HttpNotFound();
            }

            country.ModifiedDate = DateTime.Now;
            country.ModifiedBy = user.ID;
            country.IsActive = false;

            dobj.Entry(country).State = EntityState.Modified;
            dobj.SaveChanges();

            return RedirectToAction("ManageCountry");
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        [Route("Settings/ManageAdministrator/Add")]

        public ActionResult AddAdministrator()
        {
            AddAdministrator AddAdministrator = new AddAdministrator
            {
                CountryCodeList = dobj.Countries.Where(x => x.IsActive).OrderBy(x => x.CountryCode).Select(x => x.CountryCode).ToList(),
                CountryCode = "+91"
            };

            return View(AddAdministrator);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        [Route("Settings/ManageAdministrator/Add")]
        public ActionResult AddAdministrator(AddAdministrator obj)
        {
            if (ModelState.IsValid)
            {
                // check if EmailID already exists or not
                bool EmailIDalreadyexists = dobj.Users.Where(x => x.EmailID == obj.Email).Any();
                if (EmailIDalreadyexists)
                {
                    ModelState.AddModelError("EmailID", "EmailID already exists");
                    obj.CountryCodeList = dobj.Countries.Where(x => x.IsActive).OrderBy(x => x.CountryCode).Select(x => x.CountryCode).ToList();
                    return View(obj);
                }

                // get logged in superadmin
                var superadmin = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();

                //create object of user
                //set default password for admin is Admin@123
                //admin can change password after login through change password page
                NotesMarketPlace.Context.Users user = new NotesMarketPlace.Context.Users
                {
                    FirstName = obj.FirstName.Trim(),
                    LastName = obj.LastName.Trim(),
                    RoleID = 2,
                    EmailID = obj.Email.Trim(),
                    Password = "Admin@123",
                    IsEmailVerified = true,
                    CreatedDate = DateTime.Now,
                    CreatedBy = superadmin.ID,
                    IsActive = true
                };

                dobj.Users.Add(user);
                dobj.SaveChanges();

                // get saved admin id
                var addedadmin = dobj.Users.Find(user.ID);

                // crate userprofile object
                AdminDetail userProfile = new AdminDetail
                {
                    AdminID = addedadmin.ID,
                    CountryCode = obj.CountryCode.Trim(),
                    PhoneNumber = obj.PhoneNumber.Trim(),
                   
                    CreatedDate = DateTime.Now,
                    CreatedBy = superadmin.ID
                };

                // save object in admindetail database
                dobj.AdminDetail.Add(userProfile);
                dobj.SaveChanges();

                return RedirectToAction("ManageAdministrator");
            }
            else
            {
                obj.CountryCodeList = dobj.Countries.Where(x => x.IsActive).OrderBy(x => x.CountryCode).Select(x => x.CountryCode).ToList();
                return View(obj);
            }
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        [Route("Settings/ManageAdministrator/Edit/{id}")]
        public ActionResult EditAdministrator(int id)
        {
           
            var user = dobj.Users.Where(x => x.ID == id).FirstOrDefault();
            var userprofile = dobj.AdminDetail.Where(x => x.AdminID == id).FirstOrDefault();

            // check if user or userprofile is null or not
            if (user == null || userprofile == null)
            {
                return HttpNotFound();
            }

            // create object of AddAdministrator
            AddAdministrator AddAdministrator = new AddAdministrator
            {
                ID = user.ID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.EmailID,
                CountryCode = userprofile.CountryCode,
                PhoneNumber = userprofile.PhoneNumber,
                CountryCodeList = dobj.Countries.Where(x => x.IsActive).OrderBy(x => x.CountryCode).Select(x => x.CountryCode).ToList()
            };

            return View(AddAdministrator);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        [Route("Settings/ManageAdministrator/Edit/{id}")]
        public ActionResult EditAdministrator(AddAdministrator obj)
        {
            if (ModelState.IsValid)
            {
                // check if EmailID already exists or not
                bool EmailIDalreadyexists = dobj.Users.Where(x => x.EmailID == obj.Email && x.ID != obj.ID).Any();
                if (EmailIDalreadyexists)
                {
                    ModelState.AddModelError("EmailID", "EmailID already exists");
                    obj.CountryCodeList = dobj.Countries.Where(x => x.IsActive).OrderBy(x => x.CountryCode).Select(x => x.CountryCode).ToList();
                    return View(obj);
                }

                // get user, userprofile
                var user = dobj.Users.Where(x => x.ID == obj.ID).FirstOrDefault();
                var userprofile = dobj.AdminDetail.Where(x => x.AdminID == obj.ID).FirstOrDefault();

                // get logged in superadmin
                var superadmin = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();

                // check if user or userprofile is null or not
                if (user == null || userprofile == null)
                {
                    return HttpNotFound();
                }
                user.FirstName = obj.FirstName.Trim();
                user.LastName = obj.LastName.Trim();
                user.EmailID = obj.Email.Trim();
                user.ModifiedDate = DateTime.Now;
                user.ModifiedBy = superadmin.ID;
                user.IsActive = true;
               
                dobj.Entry(user).State = EntityState.Modified;
                dobj.SaveChanges();

                // update userprofile data
                userprofile.CountryCode = obj.CountryCode;
                userprofile.PhoneNumber = obj.PhoneNumber;
                userprofile.ModifiedDate = DateTime.Now;
                userprofile.ModifiedBy = superadmin.ID;

                // save userprofile in database
                dobj.Entry(userprofile).State = EntityState.Modified;
                dobj.SaveChanges();

                return RedirectToAction("ManageAdministrator");
            }
            else
            {
                obj.CountryCodeList = dobj.Countries.Where(x => x.IsActive).OrderBy(x => x.CountryCode).Select(x => x.CountryCode).ToList();
                return View(obj);
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [Route("Settings/ManageAdministrator")]
        public ActionResult ManageAdministrator(string search, string sort, int? page )
        {
            ViewBag.Search = search;
            ViewBag.Sort = sort;
            ViewBag.PageNumber = page;

           
            int adminid = dobj.UserRole.Where(x => x.Name.ToLower() == "admin").Select(x => x.ID).FirstOrDefault();

            IEnumerable<ManageAdministrator> admins = from user in dobj.Users
                                                      join profile in dobj.AdminDetail on user.ID equals profile.AdminID
                                                      where user.RoleID == adminid
                                                      select new ManageAdministrator
                                                      {
                                                          ID = user.ID,
                                                          FirstName = user.FirstName,
                                                          LastName = user.LastName,
                                                          Email = user.EmailID,
                                                          PhoneNumber = profile.PhoneNumber,
                                                          DateAdded = user.CreatedDate.Value,
                                                          Active = user.IsActive == true ? "YES" : "NO"
                                                      };

            // search result
            if (!String.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                admins = admins.Where(x => x.FirstName.ToLower().Contains(search) ||
                                           x.LastName.ToLower().Contains(search) ||
                                           x.Email.ToLower().Contains(search) ||
                                           x.PhoneNumber.Contains(search) ||
                                           x.DateAdded.ToString("dd-MM-yyyy hh:mm").Contains(search) ||
                                           x.Active.ToLower().Contains(search)
                                     ).ToList();
            }

            // sort results
            admins = SortTableAdministrator(sort, admins);

            return View(admins.ToList().AsQueryable().ToPagedList(page ?? 1, 5));
        }

        // sorting for administrator
        private IEnumerable<ManageAdministrator> SortTableAdministrator(string sort, IEnumerable<ManageAdministrator> table)
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
                case "EmailID_Asc":
                    {
                        table = table.OrderBy(x => x.Email);
                        break;
                    }
                case "EmailID_Desc":
                    {
                        table = table.OrderByDescending(x => x.Email);
                        break;
                    }
                case "Phone_Asc":
                    {
                        table = table.OrderBy(x => x.PhoneNumber);
                        break;
                    }
                case "Phone_Desc":
                    {
                        table = table.OrderByDescending(x => x.PhoneNumber);
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
                case "Active_Asc":
                    {
                        table = table.OrderBy(x => x.Active);
                        break;
                    }
                case "Active_Desc":
                    {
                        table = table.OrderByDescending(x => x.Active);
                        break;
                    }
                default:
                    {
                        table = table.OrderByDescending(x => x.DateAdded);
                        break;
                    }
            }
            return table;
        }




        [Authorize(Roles = "SuperAdmin")]
        [Route("Settings/ManageAdministrator/Delete/{id}")]
        public ActionResult DeleteAdministrator(int id)
        {
            // get logged in superadmin
            var superadmin = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();

            // get admin user
            var admin = dobj.Users.Where(x => x.ID == id).FirstOrDefault();

            if (admin == null)
            {
                return HttpNotFound();
            }

            admin.ModifiedDate = DateTime.Now;
            admin.ModifiedBy = superadmin.ID;
            admin.IsActive = false;

            dobj.Entry(admin).State = EntityState.Modified;
            dobj.SaveChanges();

            return RedirectToAction("ManageAdministrator");
        }


        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        [Route("ManageSystemConfigurations")]
        public ActionResult ManageSystemConfigurations()
        {

            var emailid = User.Identity.Name.ToString();
            NotesMarketPlace.Context.Users admin = dobj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            SystemConfiguration sct = dobj.SystemConfiguration.FirstOrDefault();

            SystemConfigurationModel model = new SystemConfigurationModel();
            model.EmailID1 = sct.EmailID1;
          
            model.PhoneNumber = sct.PhoneNumber;
            model.EmailID2 = sct.EmailID2;
            model.FacebookURL = sct.FacebookUrl;
            model.TwitterURL = sct.TwitterUrl;
            model.LinkedInURL = sct.LinkedInUrl;

            ViewBag.ProfilePicture = dobj.AdminDetail.Where(x => x.AdminID == admin.ID).Select(x => x.ProfilePicture).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        [Route("ManageSystemConfigurations")]
        public ActionResult ManageSystemConfigurations(SystemConfigurationModel model)
        {
            var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                SystemConfiguration mscobj = dobj.SystemConfiguration.FirstOrDefault();

                mscobj.EmailID1 = model.EmailID1;
               
                mscobj.PhoneNumber = model.PhoneNumber;
                mscobj.EmailID2 = model.EmailID2;
                mscobj.FacebookUrl = model.FacebookURL;
                mscobj.TwitterUrl = model.TwitterURL;
                mscobj.LinkedInUrl = model.LinkedInURL;
                mscobj.CreatedBy = user.ID;
                mscobj.CreatedDate = DateTime.Now;

                if (model.DefaultNotePreview != null)
                {
                    var OldDefaultNoteImage = Server.MapPath(mscobj.DefaultNotePreview);
                    FileInfo file = new FileInfo(OldDefaultNoteImage);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                    var DefaultNoteImageName = "defaultbook" + Path.GetExtension(model.DefaultNotePreview.FileName);
                    var DefaultNoteImageSavePath = Path.Combine(Server.MapPath("~/Content/Default/") + DefaultNoteImageName);
                    model.DefaultNotePreview.SaveAs(DefaultNoteImageSavePath);
                    mscobj.DefaultNotePreview = Path.Combine(("~/Content/Default/") + DefaultNoteImageName);
                }

                if (model.DefaultProfilePicture != null)
                {
                    var OldDefaultProfilePicture = Server.MapPath(mscobj.DefaultProfilePicture);
                    FileInfo file2 = new FileInfo(OldDefaultProfilePicture);
                    if (file2.Exists)
                    {
                        file2.Delete();
                    }
                    var DefaultProfilePictureName = "defaultuser" + Path.GetExtension(model.DefaultProfilePicture.FileName);
                    var DefaultProfilePictureSavePath = Path.Combine(Server.MapPath("~/Content/Default/") + DefaultProfilePictureName);
                    model.DefaultProfilePicture.SaveAs(DefaultProfilePictureSavePath);
                    mscobj.DefaultProfilePicture = Path.Combine(("~/Content/Default/") + DefaultProfilePictureName);
                }
                dobj.Entry(mscobj).State = System.Data.Entity.EntityState.Modified;
                dobj.SaveChanges();

                return RedirectToAction("Dashboard", "Admin");
            }
            return View();
        }


    }
}