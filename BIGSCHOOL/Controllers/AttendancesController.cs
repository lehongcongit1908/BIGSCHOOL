using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BIGSCHOOL.Models;



using Microsoft.AspNet.Identity;

using System.Web.Routing;
namespace BIGSCHOOL.Controllers
{
    public class AttendancesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Attend(Course  attendanceDto)
        {
            BigSchoolContext db = new BigSchoolContext();
            var userId = User.Identity.GetUserId();
            if (db.Attendances.Any(a => a.Attendee == userId && a.CourseID == attendanceDto.Id))
            {
                return BadRequest("The Attendance already exists !");
            }
            var attendance = new Attendance
            {
                Attendee = userId,
                CourseID = attendanceDto.Id

            };
            db.Attendances.Add(attendance);
            db.SaveChanges();
            return Ok();
        }
    }
}
