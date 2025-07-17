using Microsoft.AspNetCore.Mvc;
using Student_Management_System.DB_CONNECT;
using Student_Management_System.Models;
using System.Data.Common;

namespace Student_Management_System.Controllers
{
    public class TeacherController : Controller
    {
        private readonly Teachers _db;

        public TeacherController(Teachers db)
        {
            _db = db;
        }

        // GET: /Teacher
        public IActionResult Index()
        {
            // Fetch all necessary data from the database
            var teachers = _db.GetAllTeachers();
            var departments = _db.GetAllDepartments();
            var courses = _db.GetAllCourses();
            var teacherCourses = _db.GetAllTeacherCourses();

            // Create a list of view models
            var viewModelList = teachers.Select(t => new TeacherIndexViewModel
            {
                TeacherID = t.TeacherID,
                Name = $"{t.FirstName} {t.LastName}",
                Gender = t.Gender,
                Email = t.Email,
                Phone = t.Phone,
                DepartmentName = departments
                    .FirstOrDefault(d => d.DepartmentID == t.DepartmentID)?.DepartmentName ?? "N/A",

                CoursesTaught = string.Join(", ",
                    teacherCourses
                        .Where(tc => tc.TeacherID == t.TeacherID)
                        .Join(courses,
                              tc => tc.CourseID,
                              c => c.CourseID,
                              (tc, c) => c.CourseName))
            }).ToList();

            return View(viewModelList);
        }






        // GET: /Teacher/Details/5
        public IActionResult Details(int id)
        {
            var teacher = _db.GetTeacherById(id);
            if (teacher == null)
                return NotFound();

            return View(teacher);
        }

        // GET: /Teacher/Create
        public IActionResult Create()
        {
            var viewModel = new TeacherViewModel
            {
                AllCourses = _db.GetAllCourses(), 
                AllDepartments = _db.GetAllDepartments(),
                SelectedCourseIds = new List<int>()
            };

            return View(viewModel);
        }



        // POST: /Teacher/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TeacherViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _db.AddTeacher(viewModel.Teacher);

                // Get TeacherID of newly added teacher (optional: return ID from AddTeacher)
                var allTeachers = _db.GetAllTeachers();
                var newTeacher = allTeachers.OrderByDescending(t => t.TeacherID).FirstOrDefault();

                if (newTeacher != null && viewModel.SelectedCourseIds.Any())
                {
                    _db.AssignCoursesToTeacher(newTeacher.TeacherID, viewModel.SelectedCourseIds);
                }

                return RedirectToAction(nameof(Index));
            }

            viewModel.AllCourses = _db.GetAllCourses();
            viewModel.AllDepartments = _db.GetAllDepartments();
            return View(viewModel);
        }


        // GET: /Teacher/Edit/5
        public IActionResult Edit(int id)
        {
            var teacher = _db.GetTeacherById(id);
            if (teacher == null)
                return NotFound();

            var viewModel = new TeacherViewModel
            {
                Teacher = teacher,
                AllCourses = _db.GetAllCourses(),
                AllDepartments = _db.GetAllDepartments(),
                SelectedCourseIds = _db.GetCoursesByTeacherId(id)
            };

            return View(viewModel);
        }


        // POST: /Teacher/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TeacherViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _db.UpdateTeacher(viewModel.Teacher);
                _db.UpdateTeacherCourses(viewModel.Teacher.TeacherID, viewModel.SelectedCourseIds ?? new List<int>());
                return RedirectToAction(nameof(Index));
            }

            viewModel.AllCourses = _db.GetAllCourses();
            viewModel.AllDepartments = _db.GetAllDepartments();
            return View(viewModel);
        }


        // GET: /Teacher/Delete/5
        public IActionResult Delete(int id)
        {
            var teacher = _db.GetTeacherById(id);
            if (teacher == null)
                return NotFound();

            return View(teacher);
        }

        // POST: /Teacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _db.DeleteTeacher(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Teacher/SearchById
        public IActionResult SearchById(int id)
        {
            var teacher = _db.GetTeacherById(id);
            if (teacher == null)
                return View("Index", new List<Teacher>());

            return View("Index", new List<Teacher> { teacher });
        }

        // SEARCH BAR

        [HttpGet]
        public IActionResult Search(string query)
        {
            var teachers = _db.SearchTeachers(query ?? "");
            return View("Index", teachers);
        }

    }
}
