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
   
    public class FollowingsController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Follow(Following follow)
        {
            var userID = User.Identity.GetUserId();
            if(userID == null)
            {
                return BadRequest("Please login first");

            }
            if (userID == follow.FolloweeId)
                return BadRequest("Không thể tự theo dõi chính mình");


            BigSchoolContext db = new BigSchoolContext();
            Following find = db.Followings.FirstOrDefault(p => p.FollowerId == userID && p.FolloweeId == follow.FolloweeId);
            if(find != null)
            {
                return BadRequest("The already following exists!");
            }

            // set object follow
            follow.FollowerId = userID;
            db.Followings.Add(follow);
            db.SaveChanges();
            return Ok();
        }

    }
}
