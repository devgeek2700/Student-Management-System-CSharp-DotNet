using Microsoft.AspNetCore.Mvc;
using Student_Management_System.Models;
using Student_Management_System.DB_CONNECT;

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
            var teachers = _db.GetAllTeachers();
            return View(teachers);
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
            return View();
        }

        // POST: /Teacher/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _db.AddTeacher(teacher);
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: /Teacher/Edit/5
        public IActionResult Edit(int id)
        {
            var teacher = _db.GetTeacherById(id);
            if (teacher == null)
                return NotFound();

            return View(teacher);
        }

        // POST: /Teacher/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _db.UpdateTeacher(teacher);
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
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
