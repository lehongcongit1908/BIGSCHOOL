using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BIGSCHOOL.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
// sử dụng cho lấy user id
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;

namespace BIGSCHOOL.Controllers
{
    public class Courses_2Controller : Controller
    {
        

        BigSchoolContext db = new BigSchoolContext();
        // GET: Courses_2
        public ActionResult Index()
        {
            
            var upcomingCourse = db.Courses.Where(p => p.DateTime > DateTime.Now).OrderBy(p => p.DateTime).ToList();
            foreach (Course i in upcomingCourse)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(i.LectureId);
                i.Name = user.Name;

            }
            return View(upcomingCourse);
        }

        public ActionResult Create()
        {
            Course obj_course = new Course();
            //ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            obj_course.ListCategory = db.Categories.ToList();
            return View(obj_course);
        }
        [Authorize]
        [HttpPost]

        public ActionResult Create(Course obj_course)
        {

            //
            ModelState.Remove("LectureId");
            if (!ModelState.IsValid)
            {
                obj_course.ListCategory = db.Categories.ToList();
                return View("Create", obj_course);
            }
            // lấy login user id
            ApplicationUser user = new ApplicationUser();
            user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            obj_course.LectureId = user.Id;

            // add vào csdl
            db.Courses.Add(obj_course);
            db.SaveChanges();


            //ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return RedirectToAction("Index", "Home");
        }


    }
}