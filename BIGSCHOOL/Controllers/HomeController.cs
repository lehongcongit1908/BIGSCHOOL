﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using BIGSCHOOL.Models;

namespace BIGSCHOOL.Controllers
{
    public class HomeController : Controller
    {
        BigSchoolContext db = new BigSchoolContext();
        

        [Authorize]
        public ActionResult Index()
            
        {
            var upcomingCourse = db.Courses.ToList();

            //var upcomingCourse = db.Courses.Where(p => p.DateTime > DateTime.Now).OrderBy(p => p.DateTime).ToList();
            foreach (Course i in upcomingCourse)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(i.LectureId);
                i.Name = user.Name;

            }
            return View(upcomingCourse);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}