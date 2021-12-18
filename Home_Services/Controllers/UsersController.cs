using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Home_Services.Models;
using SolrNet.Utils;
using System.ComponentModel.DataAnnotations;

namespace Home_Services.Controllers
{
    public class UsersController : Controller
    {

        
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            User_Registration userModel = new User_Registration();
            return View(userModel);
        }

        [HttpPost]
        public ActionResult AddOrEdit(User_Registration userModel)
        {
            using (Home_ServicesEntities5 obj = new Home_ServicesEntities5())
            {

                if (obj.User_Registration.Any(x => x.Email == userModel.Email))
                {
                    ViewBag.DuplicateMessage = "Email Already Exist";
                    return View("AddOrEdit", userModel);
                }
                
                obj.User_Registration.Add(userModel);
                obj.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration successfull";
            return View("AddOrEdit",new User_Registration());
        }




        public ActionResult UserLogin()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Authorise(Home_Services.Models.User_Registration userModel)
        {
            using (Home_ServicesEntities5 db = new Home_ServicesEntities5())
            {
                var userDetails = db.User_Registration.Where(x => x.Email == userModel.Email && x.Password == userModel.Password).FirstOrDefault();
                if (userDetails == null)
                {
                    userModel.LoginErrorMessage = "Wrong Email or password.";
                    return View("UserLogin", userModel);
                }
                else
                {
                    Session["First_Name"] = userDetails.First_Name;
                    
                    return RedirectToAction("Dashboard", "UserDashboard");
                }
            }
        }

        public ActionResult LogOut()
        {
            string First_Name = Session["First_Name"].ToString();
            Session.Abandon();
            return RedirectToAction("UserLogin", "Users");
        }




    }
}








