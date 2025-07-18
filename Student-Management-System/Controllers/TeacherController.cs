using Microsoft.AspNetCore.Mvc;
using Student_Management_System.DB_CONNECT;
using Student_Management_System.DB_CONNECT.Interfaces;
using Student_Management_System.Models;
using Student_Management_System.Models.Enums;
using System;
using System.Diagnostics;

namespace Student_Management_System.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ITeacher _iTeacher;

        public TeacherController(ITeacher iTeacher)
        {
            _iTeacher = iTeacher;
        }

        public IActionResult Index()
        {
            try
            {
                var teachers = _iTeacher.GetAllTeachers();
                var departments = _iTeacher.GetAllDepartments();
                var courses = _iTeacher.GetAllCourses();
                var teacherCourses = _iTeacher.GetAllTeacherCourses();

                var viewModelList = teachers.Select(t => new TeacherIndexViewModel
                {
                    TeacherID = t.TeacherID,
                    Name = $"{t.FirstName} {t.LastName}",
                    Gender = t.Gender.ToString(), 
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
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        public IActionResult Details(int id)
        {
            try
            {
                var teacher = _iTeacher.GetTeacherById(id);
                if (teacher == null)
                    return NotFound();

                return View(teacher);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Details: {ex.Message}");
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        public IActionResult Create()
        {
            try
            {
                var viewModel = new TeacherViewModel
                {
                    AllCourses = _iTeacher.GetAllCourses(),
                    AllDepartments = _iTeacher.GetAllDepartments(),
                    SelectedCourseIds = new List<int>()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GET Create: {ex.Message}");
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
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
                    _iTeacher.AddTeacher(viewModel.Teacher);

                    var allTeachers = _iTeacher.GetAllTeachers();
                    var newTeacher = allTeachers.OrderByDescending(t => t.TeacherID).FirstOrDefault();

                    if (newTeacher != null && viewModel.SelectedCourseIds.Any())
                    {
                        _iTeacher.AssignCoursesToTeacher(newTeacher.TeacherID, viewModel.SelectedCourseIds);
                    }

                    return RedirectToAction(nameof(Index));
                }

                viewModel.AllCourses = _iTeacher.GetAllCourses();
                viewModel.AllDepartments = _iTeacher.GetAllDepartments();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in POST Create: {ex.Message}");
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        public IActionResult Edit(int id)
        {
            try
            {
                var teacher = _iTeacher.GetTeacherById(id);
                if (teacher == null)
                    return NotFound();

                var viewModel = new TeacherViewModel
                {
                    Teacher = teacher,
                    AllCourses = _iTeacher.GetAllCourses(),
                    AllDepartments = _iTeacher.GetAllDepartments(),
                    SelectedCourseIds = _iTeacher.GetCoursesByTeacherId(id)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GET Edit: {ex.Message}");
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
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
                    _iTeacher.UpdateTeacher(viewModel.Teacher);
                    _iTeacher.UpdateTeacherCourses(viewModel.Teacher.TeacherID, viewModel.SelectedCourseIds ?? new List<int>());
                    return RedirectToAction(nameof(Index));
                }

                viewModel.AllCourses = _iTeacher.GetAllCourses();
                viewModel.AllDepartments = _iTeacher.GetAllDepartments();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in POST Edit: {ex.Message}");
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var teacher = _iTeacher.GetTeacherById(id);
                if (teacher == null)
                    return NotFound();

                return View(teacher);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GET Delete: {ex.Message}");
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _iTeacher.DeleteTeacher(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in POST Delete: {ex.Message}");
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        public IActionResult SearchById(int id)
        {
            try
            {
                var teacher = _iTeacher.GetTeacherById(id);
                if (teacher == null)
                    return View("Index", new List<TeacherIndexViewModel>());

                var departments = _iTeacher.GetAllDepartments();
                var courses = _iTeacher.GetAllCourses();
                var teacherCourses = _iTeacher.GetAllTeacherCourses();

                var viewModel = new TeacherIndexViewModel
                {
                    TeacherID = teacher.TeacherID,
                    Name = $"{teacher.FirstName} {teacher.LastName}",
                    Gender = teacher.Gender.ToString(),
                    Email = teacher.Email,
                    Phone = teacher.Phone,
                    DepartmentName = departments.FirstOrDefault(d => d.DepartmentID == teacher.DepartmentID)?.DepartmentName ?? "N/A",
                    CoursesTaught = string.Join(", ",
                        teacherCourses
                            .Where(tc => tc.TeacherID == teacher.TeacherID)
                            .Join(courses,
                                  tc => tc.CourseID,
                                  c => c.CourseID,
                                  (tc, c) => c.CourseName))
                };

                return View("Index", new List<TeacherIndexViewModel> { viewModel });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SearchById: {ex.Message}");
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }


        [HttpGet]
        public IActionResult Search(string query)
        {
            try
            {
                var teachers = _iTeacher.SearchTeachers(query ?? "");
                var departments = _iTeacher.GetAllDepartments();
                var courses = _iTeacher.GetAllCourses();
                var teacherCourses = _iTeacher.GetAllTeacherCourses();

                var viewModelList = teachers.Select(t => new TeacherIndexViewModel
                {
                    TeacherID = t.TeacherID,
                    Name = $"{t.FirstName} {t.LastName}",
                    Gender = t.Gender.ToString(),
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

                return View("Index", viewModelList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Search: {ex.Message}");
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

    }
}
