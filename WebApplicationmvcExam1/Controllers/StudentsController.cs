using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationmvcExam1.Models;

namespace WebApplicationmvcExam1.Controllers
{
    public class StudentsController : Controller
    {
        private StudentContext db = new StudentContext();

        // GET: Students
        public ActionResult Index()
        {
            var data = db.Students.Include(e => e.StudentMarks).ToList();

            if (Request.IsAjaxRequest())

                return PartialView(data);

            return View(data);
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            Student student = new Student();
            return View(student);
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Student student,string command="")
        {
            if (student.ImageUpload != null)
            {
                student.ImageUrl = "/Images/" + Guid.NewGuid() + Path.GetExtension(student.ImageUpload.FileName);

                student.ImageUpload.SaveAs(Server.MapPath(student.ImageUrl));

                TempData["ImageUrl"] = student.ImageUrl;
            }
            else
            {
                student.ImageUrl = TempData["ImageUrl"]?.ToString();
            }
            if (command == "add")
            {
                //if (employee.Experiences is null)
                //    employee.Experiences = new List<Experience>();
                student.StudentMarks.Add(new StudentMark());
                return View(student);
            }

            else if (command.StartsWith("delete"))
            {
                int index = Convert.ToInt32(command.Replace("delete-", string.Empty));
                student.StudentMarks.RemoveAt(index);
                return View(student);
            }
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Address,Email,Class,Regular,ImageUrl, ImageUpload,StudentMarks")] Student student, string command = "")
        {
            if (command == "add")
            {
                if (student.StudentMarks is null)
                    student.StudentMarks = new List<StudentMark>();
                student.StudentMarks.Add(new StudentMark());
                return View(student);
            }

            else if (command.StartsWith("delete"))
            {
                int index = Convert.ToInt32(command.Replace("delete-", string.Empty));
                student.StudentMarks.RemoveAt(index);
                return View(student);
            }
            if (ModelState.IsValid)
            {
                if (student.ImageUpload != null)
                {
                    student.ImageUrl = "/Images/" + Guid.NewGuid() + Path.GetExtension(student.ImageUpload.FileName);

                    student.ImageUpload.SaveAs(Server.MapPath(student.ImageUrl));
                }


                try
                {

                    db.StudentMarks.RemoveRange(db.StudentMarks.Where(ex => ex.StudentID == student.ID));
                    db.SaveChanges();

                    foreach (var exp in student.StudentMarks)
                    {
                        StudentMark exEntry = new StudentMark();
                        exEntry.StudentID = student.ID;
                        exEntry.SubjectName = exp.SubjectName;
                        exEntry.TotalNumber = exp.TotalNumber;
                        exEntry.ObtainedNumber = exp.ObtainedNumber;
                        exEntry.StartDate = exp.StartDate;
                        exEntry.EndDate = exp.EndDate;

                        db.StudentMarks.Add(exEntry);
                        //exp.EmployeeID = employee.ID;
                        //db.Entry(exp).State = EntityState.Added;
                        db.SaveChanges();
                    }

                    Student studentEdit = db.Students.Find(student.ID);
                    studentEdit.Name = student.Name;
                    studentEdit.Address = student.Address;
                    studentEdit.Email = student.Email;
                    studentEdit.Class = student.Class;
                    studentEdit.Regular = student.Regular;
                    studentEdit.ImageUrl = student.ImageUrl;

                    db.Entry(studentEdit).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception err)
                {
                    ModelState.AddModelError("save", err.Message);
                    return View(student);
                }
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpDelete]
        public ActionResult AjaxDelete(int id)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            db.Students.Remove(student);
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
    }
}
