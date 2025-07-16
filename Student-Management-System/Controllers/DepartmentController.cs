using Microsoft.AspNetCore.Mvc;
using Student_Management_System.Models;
using Student_Management_System.DB_CONNECT;

namespace Student_Management_System.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly Departments _db;

        public DepartmentController(Departments db)
        {
            _db = db;
        }

        // GET: /Course
        public IActionResult Index()
        {
            var departments = _db.GetAllDepartments();
            return View(departments);
        }

        // GET: /Course/Details/5
        public IActionResult Details(int id)
        {
            var department = _db.GetDepartmentById(id);
            if (department == null)
                return NotFound();

            return View(department);
        }

        // GET: /Course/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _db.AddDepartment(department);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: /Course/Edit/5
        public IActionResult Edit(int id)
        {
            var department = _db.GetDepartmentById(id);
            if (department == null)
                return NotFound();

            return View(department);
        }

        // POST: /Course/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department department)
        {
            Console.WriteLine($"Updating Department ID: {department.DepartmentID}");

            if (ModelState.IsValid)
            {
                _db.UpdateDepartment(department);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: /Course/Delete/5
        public IActionResult Delete(int id)
        {
            var department = _db.GetDepartmentById(id);
            if (department == null)
                return NotFound();

            return View(department);
        }

        // POST: /Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _db.DeleteDepartment(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Course/SearchById?id=2
        public IActionResult SearchById(int id)
        {
            var department = _db.GetDepartmentById(id);
            if (department == null)
                return View("Index", new List<Department>());

            return View("Index", new List<Department> { department });
        }

        // SERACH BAR
        public IActionResult Search(string query)
        {
            var results = _db.SearchDepartments(query); // You write this method in DbConnection or DepartmentRepo
            return View("Index", results);
        }



    }
}
