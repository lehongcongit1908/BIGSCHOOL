using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BIGSCHOOL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BIGSCHOOL.Models;

using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using BIGSCHOOL.DTO;


namespace BIGSCHOOL.Controllers
{
    public class AttendancesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Attendances( AttendancesDTO attendanceDto)
        {
            BigSchoolContext db = new BigSchoolContext();
            var userId = User.Identity.GetUserId();
            if (db.Attendances.Any(a => a.Attendee == userId && a.CourseID == attendanceDto.CourseId))
            {
                return BadRequest("The Attendance already exists !");
            }
            var attendance = new Attendance
            {
                Attendee = userId,
                CourseID = attendanceDto.CourseId

            };
            db.Attendances.Add(attendance);
            db.SaveChanges();
            return Ok();
        }
    }
}
