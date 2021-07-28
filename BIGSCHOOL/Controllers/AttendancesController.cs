using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BIGSCHOOL.Models;
using Microsoft.AspNet.Identity;

namespace BIGSCHOOL.Controllers
{
    public class AttendancesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Attend(Course attendanceDto)
        {
            var userID = User.Identity.GetUserId();
            BigSchoolContext context = new BigSchoolContext();
            if (context.Attendances.Any(p => p.Attendee == userID && p.CourseID == attendanceDto.Id))
            {
                // return BadRequest("The attendance already exists!");
                // xóa thông tin khóa học đã đăng ký tham gia trong bảng Attendances
                context.Attendances.Remove(context.Attendances.SingleOrDefault(p => p.Attendee == userID && p.CourseID == attendanceDto.Id));
                context.SaveChanges();
                return Ok("cancel");
            }
            var attendance = new Attendance()
            {
                CourseID = attendanceDto.Id,
                Attendee =  User.Identity.GetUserId()
            };
            
            context.Attendances.Add(attendance);
            context.SaveChanges();
            return Ok();

        }
    }
}
