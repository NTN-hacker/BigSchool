using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BigSchool.Controllers
{
    public class CourceController : Controller
    {
        BigSchoolContext data = new BigSchoolContext();
        // GET: Cource
        public ActionResult Create()
        {
            BigSchoolContext data = new BigSchoolContext();
            Course objCource = new Course();
            objCource.listCategory = data.Categories.ToList();

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
                objCourse.listCategory = data.Categories.ToList();
                return View("Create", objCourse);
            }
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;

            data.Courses.Add(objCourse);
            data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        public ActionResult Attending()
        {
            BigSchoolContext data = new BigSchoolContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                                                 .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendances = data.Attendances.Where(p => p.Attendee == currentUser.Id).ToList();
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
            var courses = context.Courses.Where(c => c.LecturerId == curentUser.Id && c.Datetime > DateTime.Now).ToList();
            foreach (Course i in courses)
            {
                i.LectureName = curentUser.Name;
            }
            return View(courses);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = data.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(data.Categories, "Id", "Name", course.CategoryId);
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LecturerId,Place,DateTime,CategoryId")] Course course)
        {
            if (ModelState.IsValid)
            {
                data.Entry(course).State = EntityState.Modified;
                data.SaveChanges();
                return RedirectToAction("Mine");
            }
            ViewBag.CategoryId = new SelectList(data.Categories, "Id", "Name", course.CategoryId);
            return View(course);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = data.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Test/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = data.Courses.Find(id);
            data.Courses.Remove(course);
            data.SaveChanges();
            return RedirectToAction("Mine");
        }

    }
}