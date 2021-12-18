using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Home_Services.Models;

namespace Home_Services.Controllers
{
    public class VendorsController : Controller
    {
 
        // GET: Vendors
        [HttpGet]
        public ActionResult VendorRegister()
        {
            Vendor_Registration vendorModel = new Vendor_Registration();
            return View(vendorModel);
        }

        [HttpPost]
        public ActionResult VendorRegister(Vendor_Registration vendorModel)
        {
            using (Home_ServicesEntities5 obj = new Home_ServicesEntities5())
            {

                if (obj.Vendor_Registration.Any(x => x.Email == vendorModel.Email))
                {
                    ViewBag.DuplicateMessage = "Email Already Exist";
                    return View("VendorRegister", vendorModel);
                }

                obj.Vendor_Registration.Add(vendorModel);
                obj.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration successfull and Your User ID is "+vendorModel.Vendor_Id;
            return View("VendorRegister", new Vendor_Registration());
        }


        public ActionResult VendorLogin()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Authorise(Home_Services.Models.Vendor_Registration vendorModel)
        {
            using (Home_ServicesEntities5 db = new Home_ServicesEntities5())
            {
                var vendorDetails = db.Vendor_Registration.Where(x => x.Email == vendorModel.Email && x.Password == vendorModel.Password).FirstOrDefault();
                if (vendorDetails == null)
                {
                    vendorModel.LoginErrorMessage = "Wrong Email or password.";
                    return View("VendorLogin", vendorModel);
                }
                else
                {
                    Session["First_Name"] = vendorDetails.First_Name;

                    return RedirectToAction("Dashboard", "VendorDashboard");
                }
            }
        }
        public ActionResult LogOut()
        {
            string First_Name = Session["First_Name"].ToString();
            Session.Abandon();
            return RedirectToAction("VendorLogin", "Vendors");
        }
    }


}