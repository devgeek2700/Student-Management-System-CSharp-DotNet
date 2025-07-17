using Microsoft.AspNetCore.Mvc;
using Student_Management_System.DB_CONNECT;
using Student_Management_System.Models;
using System;

namespace Student_Management_System.Controllers
{
    public class TeacherController : Controller
    {
        private readonly Teachers _db;

        public TeacherController(Teachers db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            try
            {
                var teachers = _db.GetAllTeachers();
                var departments = _db.GetAllDepartments();
                var courses = _db.GetAllCourses();
                var teacherCourses = _db.GetAllTeacherCourses();

                var viewModelList = teachers.Select(t => new TeacherIndexViewModel
                {
                    TeacherID = t.TeacherID,
                    Name = $"{t.FirstName} {t.LastName}",
                    Gender = t.Gender,
                    Email = t.Email,
                    Phone = t.Phone,
                    DepartmentName = departments.FirstOrDefault(d => d.DepartmentID == t.DepartmentID)?.DepartmentName ?? "N/A",
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Index: {ex.Message}");
                return View("Error");
            }
        }

        public IActionResult Details(int id)
        {
            try
            {
                var teacher = _db.GetTeacherById(id);
                if (teacher == null)
                    return NotFound();

                return View(teacher);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Details: {ex.Message}");
                return View("Error");
            }
        }

        public IActionResult Create()
        {
            try
            {
                var viewModel = new TeacherViewModel
                {
                    AllCourses = _db.GetAllCourses(),
                    AllDepartments = _db.GetAllDepartments(),
                    SelectedCourseIds = new List<int>()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GET Create: {ex.Message}");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TeacherViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.AddTeacher(viewModel.Teacher);

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
            catch (Exception ex)
            {
                Console.WriteLine($"Error in POST Create: {ex.Message}");
                return View("Error");
            }
        }

        public IActionResult Edit(int id)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GET Edit: {ex.Message}");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TeacherViewModel viewModel)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error in POST Edit: {ex.Message}");
                return View("Error");
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var teacher = _db.GetTeacherById(id);
                if (teacher == null)
                    return NotFound();

                return View(teacher);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GET Delete: {ex.Message}");
                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _db.DeleteTeacher(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in POST Delete: {ex.Message}");
                return View("Error");
            }
        }

        public IActionResult SearchById(int id)
        {
            try
            {
                var teacher = _db.GetTeacherById(id);
                if (teacher == null)
                    return View("Index", new List<Teacher>());

                return View("Index", new List<Teacher> { teacher });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SearchById: {ex.Message}");
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Search(string query)
        {
            try
            {
                var teachers = _db.SearchTeachers(query ?? "");
                return View("Index", teachers);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Search: {ex.Message}");
                return View("Error");
            }
        }
    }
}
