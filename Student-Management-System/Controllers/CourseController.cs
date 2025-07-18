using Microsoft.AspNetCore.Mvc;
using Student_Management_System.DB_CONNECT;
using Student_Management_System.DB_CONNECT.Interfaces;
using Student_Management_System.Models;
using System;
using System.Diagnostics;

namespace Student_Management_System.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourse _iCourse;

        public CourseController(ICourse iCourse)
        {
            _iCourse = iCourse;
        }

        // GET: /Course
        public IActionResult Index()
        {
            try
            {
                var courses = _iCourse.GetAllCourses();
                return View(courses);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Index: " + ex.Message);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        // GET: /Course/Details/5
        public IActionResult Details(int id)
        {
            try
            {
                var course = _iCourse.GetCourseById(id);
                if (course == null)
                    return NotFound();

                return View(course);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Details: " + ex.Message);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        // GET: /Course/Create
        public IActionResult Create()
        {
            try
            {
                ViewBag.Departments = _iCourse.GetAllDepartments();
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Create GET: " + ex.Message);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        // POST: /Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _iCourse.AddCourse(course);
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Departments = _iCourse.GetAllDepartments();
                return View(course);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Create POST: " + ex.Message);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        // GET: /Course/Edit/5
        public IActionResult Edit(int id)
        {
            try
            {
                var course = _iCourse.GetCourseById(id);
                if (course == null)
                    return NotFound();

                ViewBag.Departments = _iCourse.GetAllDepartments();
                return View(course);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Edit GET: " + ex.Message);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        // POST: /Course/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Course course)
        {
            try
            {
                Console.WriteLine($"Updating Course ID: {course.CourseID}");

                if (ModelState.IsValid)
                {
                    _iCourse.UpdateCourse(course);
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Departments = _iCourse.GetAllDepartments();
                return View(course);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Edit POST: " + ex.Message);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        // GET: /Course/Delete/5
        public IActionResult Delete(int id)
        {
            try
            {
                var course = _iCourse.GetCourseById(id);
                if (course == null)
                    return NotFound();

                return View(course);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Delete GET: " + ex.Message);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        // POST: /Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _iCourse.DeleteCourse(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Delete POST: " + ex.Message);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        // GET: /Course/SearchById?id=2
        public IActionResult SearchById(int id)
        {
            try
            {
                var course = _iCourse.GetCourseById(id);
                if (course == null)
                    return View("Index", new List<Course>());

                return View("Index", new List<Course> { course });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in SearchById: " + ex.Message);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        // SEARCH
        public IActionResult Search(string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                    return RedirectToAction(nameof(Index));

                var results = _iCourse.SearchCourses(query);
                return View("Index", results);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Search: " + ex.Message);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }
    }
}
