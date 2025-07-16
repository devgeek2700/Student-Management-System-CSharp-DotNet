using Microsoft.AspNetCore.Mvc;
using Student_Management_System.Models;
using Student_Management_System.DB_CONNECT;

namespace Student_Management_System.Controllers
{
    public class StudentController : Controller
    {
        private readonly DbConnection _db;

        public StudentController(DbConnection db)
        {
            _db = db;
        }


        // GET: /Student
        public IActionResult Index()
        {
            var students = _db.GetAllStudents();
            return View(students);
        }

        // GET: /Student/Details/5
        public IActionResult Details(int id)
        {
            var student = _db.GetStudentById(id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        // GET: /Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _db.AddStudent(student);
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: /Student/Edit/5
        public IActionResult Edit(int id)
        {
            var student = _db.GetStudentById(id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        // POST: /Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                _db.UpdateStudent(student);
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: /Student/Delete/5
        public IActionResult Delete(int id)
        {
            var student = _db.GetStudentById(id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        // POST: /Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _db.DeleteStudent(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Student/SearchById
        public IActionResult SearchById(int id)
        {
            var student = _db.GetStudentById(id);
            if (student == null)
                return View("Index", new List<Student>());

            return View("Index", new List<Student> { student });
        }

        // Search by name, gender, courses
        // GET: /Student/Search
        public IActionResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return RedirectToAction("Index");

            var results = _db.SearchStudents(query);
            return View("Index", results); // reuse Index view
        }


    }
}
