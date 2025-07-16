using Microsoft.AspNetCore.Mvc;
using Student_Management_System.Models;
using Student_Management_System.DB_CONNECT;

namespace Student_Management_System.Controllers
{
    public class CourseController : Controller
    {
        private readonly Courses _db;

        public CourseController(Courses db)
        {
            _db = db;
        }

        // GET: /Course
        public IActionResult Index()
        {
            var courses = _db.GetAllCourses();
            return View(courses);
        }

        // GET: /Course/Details/5
        public IActionResult Details(int id)
        {
            var course = _db.GetCourseById(id);
            if (course == null)
                return NotFound();

            return View(course);
        }

        // GET: /Course/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _db.AddCourse(course);
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: /Course/Edit/5
        public IActionResult Edit(int id)
        {
            var course = _db.GetCourseById(id);
            if (course == null)
                return NotFound();

            return View(course);
        }

        // POST: /Course/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Course course)
        {
            Console.WriteLine($"Updating Course ID: {course.CourseID}");

            if (ModelState.IsValid)
            {
                _db.UpdateCourse(course);
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: /Course/Delete/5
        public IActionResult Delete(int id)
        {
            var course = _db.GetCourseById(id);
            if (course == null)
                return NotFound();

            return View(course);
        }

        // POST: /Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _db.DeleteCourse(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Course/SearchById?id=2
        public IActionResult SearchById(int id)
        {
            var course = _db.GetCourseById(id);
            if (course == null)
                return View("Index", new List<Course>());

            return View("Index", new List<Course> { course });
        }

        // SEARCH
        public IActionResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return RedirectToAction(nameof(Index));

            var results = _db.SearchCourses(query);
            return View("Index", results);
        }

    }
}
