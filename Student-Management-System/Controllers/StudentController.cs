using Microsoft.AspNetCore.Mvc;
using Student_Management_System.DB_CONNECT;
using Student_Management_System.DB_CONNECT.Interfaces;
using Student_Management_System.Models;
using System;
using System.Diagnostics;

namespace Student_Management_System.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudent _iStudent;

        public StudentController(IStudent iStudent)
        {
            _iStudent = iStudent;
        }

        public IActionResult Index()
        {
            try
            {
                var students = _iStudent.GetAllStudents();
                var departments = _iStudent.GetAllDepartments();
                var studentCourses = _iStudent.GetAllStudentCourses();
                var courses = _iStudent.GetAllCourses();

                var studentList = students.Select(s => new StudentListViewModel
                {
                    StudentID = s.StudentID,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Gender = s.Gender,
                    DOB = s.DOB,
                    Email = s.Email,
                    Phone = s.Phone,
                    Address = s.Address,
                    DepartmentName = departments.FirstOrDefault(d => d.DepartmentID == s.DepartmentID)?.DepartmentName ?? "N/A",
                    EnrollmentDate = s.EnrollmentDate,
                    CoursesTaken = studentCourses
                        .Where(sc => sc.StudentID == s.StudentID)
                        .Join(courses,
                              sc => sc.CourseID,
                              c => c.CourseID,
                              (sc, c) => c.CourseName)
                        .ToList()
                }).ToList();

                return View(studentList);
            }
            catch (Exception ex)
            {
                // Log error (can use ILogger)
                Console.WriteLine(ex.Message);
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
                var student = _iStudent.GetStudentById(id);
                if (student == null)
                    return NotFound();

                return View(student);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                var viewModel = new StudentViewModel
                {
                    Student = new Student
                    {
                        EnrollmentDate = DateTime.Now
                    },
                    AllCourses = _iStudent.GetAllCourses(),
                    AllDepartments = _iStudent.GetAllDepartments(),
                    SelectedCourseIds = new List<int>()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Student,SelectedCourseIds")] StudentViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int newStudentId = _iStudent.AddStudent(viewModel.Student);

                    if (viewModel.SelectedCourseIds != null && viewModel.SelectedCourseIds.Any())
                    {
                        _iStudent.AddStudentCourses(newStudentId, viewModel.SelectedCourseIds);
                    }

                    return RedirectToAction(nameof(Index));
                }

                viewModel.AllCourses = _iStudent.GetAllCourses();
                viewModel.AllDepartments = _iStudent.GetAllDepartments();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                var student = _iStudent.GetStudentById(id);
                if (student == null)
                    return NotFound();

                var viewModel = new StudentViewModel
                {
                    Student = student,
                    AllCourses = _iStudent.GetAllCourses(),
                    AllDepartments = _iStudent.GetAllDepartments(),
                    SelectedCourseIds = new List<int>()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StudentViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _iStudent.UpdateStudent(viewModel.Student);
                    _iStudent.UpdateStudentCourses(viewModel.Student.StudentID, viewModel.SelectedCourseIds ?? new List<int>());

                    return RedirectToAction(nameof(Index));
                }

                viewModel.AllCourses = _iStudent.GetAllCourses();
                viewModel.AllDepartments = _iStudent.GetAllDepartments();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                var student = _iStudent.GetStudentById(id);
                if (student == null)
                    return NotFound();

                return View(student);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                _iStudent.DeleteStudent(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                var student = _iStudent.GetStudentById(id);
                if (student == null)
                    return View("Index", new List<Student>());

                return View("Index", new List<Student> { student });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        public IActionResult Search(string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                    return RedirectToAction("Index");

                var results = _iStudent.SearchStudents(query);
                return View("Index", results);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }
    }
}