using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Stud.DAL.Data;
using Stud.DAL.Repository.IRepository;
using Stud.Model.ForEmail;
using Stud.Model.Models;
using System.Security.Cryptography;

namespace StudM.Controllers
{
    public class StudMController : Controller
    {
        private readonly ApplicationDbContext _db;
        private IEmailSender _emailSender;

        public StudMController(ApplicationDbContext db, IEmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
        }
        
        // Listing And Join

        public IActionResult Index()
        {
            var q = (from s in _db.student
                     join sc in _db.course on s.StudentId equals sc.Sid into scs
                     from scsresult in scs.DefaultIfEmpty()
                     join c in _db.course on scsresult.CourseId equals c.CourseId into scsc
                     from courseresult in scsc.DefaultIfEmpty()
                     group new { s, courseresult } by new { s.StudentId, s.Name, s.Email, s.ContactNo, s.RollNo, s.Address, s.State, s.City, s.ZipCode } into grp
                     select new StudVM
                     {
                         StudentId = grp.Key.StudentId,
                         Name = grp.Key.Name,
                         Email = grp.Key.Email,
                         ContactNo = grp.Key.ContactNo.ToString(),
                         RollNo = grp.Key.RollNo,
                         Address = grp.Key.Address,
                         State = grp.Key.State,
                         City = grp.Key.City,
                         Zipcode = grp.Key.ZipCode.ToString(),
                         CourseTotalPrice = grp.Sum(x => x.courseresult.CoursePrice).ToString(),
                     });

            return View(q);
        }

        //=============================================================
        // Admin Login

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

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

        //=============================================================
        // Student Side CRUD

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

            var message = new Message(new string[] { stud.Email }, "Add Student", "Congratulations , You are added successfully");
           _emailSender.SendEmail(message);

            return RedirectToAction("Index");
        }

        public IActionResult DeleteStud(int id) 
        {
            var data = _db.student.Find(id);
            _db.student.Remove(data);
            _db.SaveChanges();
            TempData["success"] = "Student Deleted Successfully";
            return RedirectToAction("Index");
        }

        //==============================================================
        // Excel File Enter Data

        [HttpPost]
        public async Task<IActionResult> studfile(IFormFile file)
        {
            var list = new List<student>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = file.OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {

                    while (reader.Read()) //Each row of the file
                    {
                        list.Add(new student
                        {
                            Name = reader.GetValue(0).ToString(),
                            Email = reader.GetValue(1).ToString(),
                            ContactNo = reader.GetValue(2).ToString(),
                            RollNo = reader.GetValue(3).ToString(),
                            Address = reader.GetValue(4).ToString(),
                            City = reader.GetValue(5).ToString(),
                            State = reader.GetValue(6).ToString(),
                            ZipCode = reader.GetValue(7).ToString(),
                        });

                    }
                }

            }
            _db.student.AddRange(list);
            await _db.SaveChangesAsync();
            TempData["success"] = "File Uploaded Successfully";
            return RedirectToAction("Index");
        }

        //==============================================================
        // Course side CRUD

        [HttpGet]
        public IActionResult AddCourse() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCourse(int id, course obj) 
        {
            if (id != 0)
            {
                course data = new course()
                {
                    CourseName = obj.CourseName,
                    CoursePrice = obj.CoursePrice,
                    Sid = id,
                    CourseId= obj.CourseId,
                };
                _db.Add(data);
                _db.SaveChanges();
                _db.SaveChanges();
                TempData["success"] = "Course Added Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        
        [HttpGet]
        public IActionResult Views(int? StudentId)
        {
            if (StudentId != 0)
            {
                var data = _db.course.Where(x => x.Sid == StudentId).ToList();
                List<List<course>> data1 = new List<List<course>>();
                foreach (var course in data)
                {
                    data1.Add(_db.course.Where(x => x.CourseId == course.CourseId).ToList());
                }
                return View(data1);
            }
            else
            {

            return View();
            }
        }
       
        public IActionResult DeleteCourse(int? Id)
        {
            var dc = _db.course.Find(Id);
            _db.course.Remove(dc);
            _db.SaveChanges();
            return RedirectToAction("Views", new { StudentId = dc.Sid });
        }

        [HttpGet]
        public IActionResult EditCourse(int Id)
        {
            var data = _db.course.Find(Id);
            return View(data);
        }

        [HttpPost]
        public IActionResult EditCourse(course course)
        {

            course pc = _db.course.Where(x=> x.CourseId == course.CourseId).FirstOrDefault();
            pc.CourseName = course.CourseName;
            pc.CourseId = course.CourseId;
            pc.CoursePrice= course.CoursePrice;
            _db.course.Update(pc);
            _db.SaveChanges();
            return RedirectToAction("Views", new { StudentId = course.Sid});
        }

    }
}
