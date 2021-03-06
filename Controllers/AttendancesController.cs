using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
//using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using BigSchool.Models;
using Microsoft.AspNet.Identity;

namespace BigSchool.Controllers
{
    public class AttendancesController : ApiController
    {
        BigSchoolContext data = new BigSchoolContext();
        [System.Web.Mvc.HttpPost]
        public IHttpActionResult Attend(Course attenC)
        {
            
            var userID = User.Identity.GetUserId();
            if (data.Attendances.Any(m => m.Attendee == userID && m.CourseId == attenC.Id))
            {
                return BadRequest("The attendaces already exist");
            }
            Attendance attendance = new Attendance()
            {
                CourseId = attenC.Id,
                Attendee = User.Identity.GetUserId()
            };
            data.Attendances.Add(attendance);
            data.SaveChanges();
            return Ok();
        }

    }
}
