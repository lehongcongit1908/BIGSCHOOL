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
           /* var upcomingCourse = db.Courses.ToList();

            //var upcomingCourse = db.Courses.Where(p => p.DateTime > DateTime.Now).OrderBy(p => p.DateTime).ToList();
            foreach (Course i in upcomingCourse)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(i.LectureId);
                i.Name = user.Name;

            }
            return View(upcomingCourse);*/


           
            var upcommingCourse = db.Courses.Where(p => p.DateTime >
           DateTime.Now).OrderBy(p => p.DateTime).ToList();
            //lấy user login hiện tại 
            var userID = User.Identity.GetUserId();
            foreach (Course i in upcommingCourse)
            {
                //tìm Name của user từ lectureid
                ApplicationUser user =
                System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>(
                ).FindById(i.LectureId);
                i.Name = user.Name;
                //lấy ds tham gia khóa học 
                if (userID != null)
                {
                    i.isLogin = true;
                    //ktra user đó chưa tham gia khóa học 
                    Attendance find = db.Attendances.FirstOrDefault(p =>
                    p.CourseID == i.Id && p.Attendee == userID);
                    if (find == null)
                        i.isShowGoing = true;
                    //ktra user đã theo dõi giảng viên của khóa học ? 
                    Following findFollow = db.Followings.FirstOrDefault(p =>
                    p.FollowerId == userID && p.FolloweeId == i.LectureId);
                    if (findFollow == null)
                        i.isShowFollow = true;
                }
            }
            return View(upcommingCourse);
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