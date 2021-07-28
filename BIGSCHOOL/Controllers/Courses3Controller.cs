using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BIGSCHOOL.Models;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BIGSCHOOL.Controllers
{
    [Authorize]
    public class Courses3Controller : Controller
    {
        private BigSchoolContext db = new BigSchoolContext();

        // GET: Courses3
        public ActionResult Index()
        {
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

        // GET: Courses3/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
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







        // GET: Courses3/Create
        /* public ActionResult Create()
         {
             ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
             return View();
         }

         // POST: Courses3/Create
         // To protect from overposting attacks, enable the specific properties you want to bind to, for 
         // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Create( Course course)
         {
             Course c2 = new Course();
             if (ModelState.IsValid)
             {
                 c2.LectureId = User.Identity.GetUserId();
                 c2.Place = course.Place;
                 c2.CategoryId = course.CategoryId;
                 c2.DateTime = course.DateTime;
                 c2.LectureName = User.Identity.Name;

                 db.Courses.Add(c2);
                 db.SaveChanges();
                 return RedirectToAction("Index");
             }

             ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", course.CategoryId);
             return View(course);
         }*/

        // GET: Courses3/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", course.CategoryId);
            return View(course);
        }

        // POST: Courses3/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,LectureId,Place,DateTime,CategoryId,LectureName")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", course.CategoryId);
            return View(course);
        }

        // GET: Courses3/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses3/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        public ActionResult Mine()
        {

            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            // var listAttendances = context.Attendances.Where(p => p.Attendee == currentUser.Id).ToList();
            //var courses = new List<Course>();
            var courses = context.Courses.Where(c => c.LectureId == currentUser.Id
            && c.DateTime > DateTime.Now).ToList();

            foreach (Course i in courses)
            {
                i.LectureName = currentUser.Name;

            }
            return View(courses);
        }
        public ActionResult LectureIamGoing()
        {
            ApplicationUser currentUser =
            System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
            .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            //danh sách giảng viên được theo dõi bởi người dùng (đăng nhập) hiện tại
            var listFollwee = context.Followings.Where(p => p.FollowerId ==
            currentUser.Id).ToList();


            //danh sách các khóa học mà người dùng đã đăng ký
            var listAttendances = context.Attendances.Where(p => p.Attendee ==
            currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach (var course in listAttendances)
            {
                foreach (var item in listFollwee)
                {
                    if (item.FolloweeId == course.Course.LectureId)
                    {
                        Course objCourse = course.Course;
                        objCourse.LectureName =
                       System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                        .FindById(objCourse.LectureId).Name;
                        courses.Add(objCourse);
                    }
                }

            }
            return View(courses);
        }


        public ActionResult Attending()
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendances = context.Attendances.Where(p => p.Attendee == currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach (Attendance temp in listAttendances)
            {
                Course objCourse = temp.Course;
                objCourse.LectureName = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objCourse.LectureId).Name;
                courses.Add(objCourse);

            }
            return View(courses);
        }

    }
}
