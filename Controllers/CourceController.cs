using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BigSchool.Controllers
{
    public class CourceController : Controller
    {
        // GET: Cource
        public ActionResult Create()
        {
            BigSchoolContext data = new BigSchoolContext();
            Course objCource = new Course();
            objCource.listCategory = data.Category.ToList();

            return View(objCource);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course objCourse)
        {

            BigSchoolContext data = new BigSchoolContext();

            ModelState.Remove("LecturerId");
            if (!ModelState.IsValid)
            {
                objCourse.listCategory = data.Category.ToList();
                return View("Create", objCourse);
            }
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;

            data.Course.Add(objCourse);
            data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        public ActionResult Attending()
        {
            BigSchoolContext data = new BigSchoolContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                                                 .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendances = data.Attendance.Where(p => p.Attendee == currentUser.Id).ToList();
            var courses = new List<Course>();
       
            foreach (Attendance temp in listAttendances)
            {
                Course ObjCourse = temp.Course;
                ObjCourse.LectureName = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                    .FindById(ObjCourse.LecturerId).Name;
                courses.Add(ObjCourse);
            }
            return View(courses);
        }
        [Authorize]
        public ActionResult Mine()
        {
            ApplicationUser curentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            var courses = context.Course.Where(c => c.LecturerId == curentUser.Id && c.Datetime > DateTime.Now).ToList();
            foreach (Course i in courses)
            {
                i.LectureName = curentUser.Name;
            }
            return View(courses);
        }



    }
}