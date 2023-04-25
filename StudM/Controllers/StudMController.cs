using Microsoft.AspNetCore.Mvc;
using Stud.DAL.Data;
using Stud.Model.Models;

namespace StudM.Controllers
{
    public class StudMController : Controller
    {
        private readonly ApplicationDbContext _db;
        
        public StudMController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public IActionResult Index()
        {
            List<student> StudList = _db.student.ToList();
            return View(StudList);
        }
        //get admin
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        //post admin
        [HttpPost]
        public IActionResult Login(admin obj)
        {
            var acp = _db.admin.Where(login => login.AdminName == obj.AdminName && login.AdminPassword == obj.AdminPassword).FirstOrDefault();
            if (acp != null)
            {
                TempData["success"] = "Login successfully:)";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult AddStud(int? id)
        {
            student stu = new student();
            if (id == null)
            {
                return View(stu);
            }
            else
            {
                var data = _db.student.Find(id);
                return View(data);
            }
           
        }

        [HttpPost]
        public IActionResult AddStud(student stud)
        {
            if (stud.StudentId == 0)
            {
                _db.student.Add(stud);
                TempData["success"] = "Student Add Successfully";
            }
            else
            {
                _db.student.Update(stud);
                TempData["success"] = "Student Updated Successfully";
            }
            
            _db.SaveChanges();
            return View("Index");
        }

       
        public IActionResult DeleteStud(int id) 
        {
            var data = _db.student.Find(id);
            _db.student.Remove(data);
            _db.SaveChanges();
            TempData["success"] = "Student Deleted Successfully";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult AddCourse(int id) 
        {
            ViewBag.CourseId = id;
            return View();
        }
        [HttpPost]
        public IActionResult AddCourse(course course) 
        {
            
            _db.course.Add(course);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Views(int id)
        {
            ViewBag.CourseId = id;
            List<course> CourseList = _db.course.ToList();
            return View(CourseList);
        }
        //public IActionResult Views(course cor) 
        //{
        //    return View();
        //}  
    }
}
