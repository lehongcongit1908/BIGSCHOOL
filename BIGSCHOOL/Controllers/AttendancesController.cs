using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using BIGSCHOOL.Models;

using System.Web.Routing;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace BIGSCHOOL.Controllers
{
    public class AttendancesController : ApiController
    {
       [HttpPost]
       public IHttpActionResult Attend(Course attendanceDto)
        {
            var userID = User.Identity.GetUserId();
            BigSchoolContext context = new BigSchoolContext();
            if( context.Attendances.Any(p=>p.Attendee==userID && p.CourseID== attendanceDto.Id))
            {
                return BadRequest("The attendance already exists!");
            }

            var attendance = new Attendance()
            {
                CourseID = attendanceDto.Id,
                Attendee = User.Identity.GetUserId()
            };
            context.Attendances.Add(attendance);
            context.SaveChanges();
            return Ok();
        }
       
    }
}
