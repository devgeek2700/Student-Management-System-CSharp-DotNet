using Microsoft.AspNetCore.Mvc;
using Student_Management_System.Models;
using Student_Management_System.DB_CONNECT;
using System;

namespace Student_Management_System.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly Departments _db;

        public DepartmentController(Departments db)
        {
            _db = db;
        }

        // GET: /Department
        public IActionResult Index()
        {
            try
            {
                var departments = _db.GetAllDepartments();
                return View(departments);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Index: {ex.Message}");
                return View("Error");
            }
        }

        // GET: /Department/Details/5
        public IActionResult Details(int id)
        {
            try
            {
                var department = _db.GetDepartmentById(id);
                if (department == null)
                    return NotFound();

                return View(department);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Details: {ex.Message}");
                return View("Error");
            }
        }

        // GET: /Department/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.AddDepartment(department);
                    return RedirectToAction(nameof(Index));
                }
                return View(department);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Create POST: {ex.Message}");
                return View("Error");
            }
        }

        // GET: /Department/Edit/5
        public IActionResult Edit(int id)
        {
            try
            {
                var department = _db.GetDepartmentById(id);
                if (department == null)
                    return NotFound();

                return View(department);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Edit GET: {ex.Message}");
                return View("Error");
            }
        }

        // POST: /Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department department)
        {
            try
            {
                Console.WriteLine($"Updating Department ID: {department.DepartmentID}");

                if (ModelState.IsValid)
                {
                    _db.UpdateDepartment(department);
                    return RedirectToAction(nameof(Index));
                }
                return View(department);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Edit POST: {ex.Message}");
                return View("Error");
            }
        }

        // GET: /Department/Delete/5
        public IActionResult Delete(int id)
        {
            try
            {
                var department = _db.GetDepartmentById(id);
                if (department == null)
                    return NotFound();

                return View(department);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Delete GET: {ex.Message}");
                return View("Error");
            }
        }

        // POST: /Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _db.DeleteDepartment(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Delete POST: {ex.Message}");
                return View("Error");
            }
        }

        // GET: /Department/SearchById?id=2
        public IActionResult SearchById(int id)
        {
            try
            {
                var department = _db.GetDepartmentById(id);
                if (department == null)
                    return View("Index", new List<Department>());

                return View("Index", new List<Department> { department });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SearchById: {ex.Message}");
                return View("Error");
            }
        }

        // GET: /Department/Search?query=chem
        public IActionResult Search(string query)
        {
            try
            {
                var results = _db.SearchDepartments(query);
                return View("Index", results);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Search: {ex.Message}");
                return View("Error");
            }
        }
    }
}
