using Home_Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Home_Services.Controllers
{
    public class AdminsController : Controller
    {
       
        // GET: Admins
        public ActionResult AdminLogin()
        {
            return View();
        }
        
        

        [HttpPost]
        public ActionResult Authorise(Home_Services.Models.Admin userModel)
        {
            using (Home_ServicesEntities5 db = new Home_ServicesEntities5())
            {
                var userDetails = db.Admins.Where(x => x.Admin_Id == userModel.Admin_Id && x.Admin_Password == userModel.Admin_Password).FirstOrDefault();
                if (userDetails == null)
                {
                    userModel.LoginErrorMessage = "Wrong Email or password.";
                    return View("AdminLogin", userModel);
                }
                else
                {
                    Session["Admin_Id"] = userDetails.Admin_Id;

                    return RedirectToAction("Dashboard", "AdminDashboard");
                }
            }
        }

        public ActionResult LogOut()
        {
            string Admin_Id = (string)Session["Admin_Id"];
            Session.Abandon();
            return RedirectToAction("AdminLogin", "Admins");
        }
    }
}