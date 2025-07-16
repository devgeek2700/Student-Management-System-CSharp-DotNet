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
            var departments = _db.GetAllDepartments();
            var studentCourses = _db.GetAllStudentCourses(); // New method
            var courses = _db.GetAllCourses();

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
            var viewModel = new StudentViewModel
            {
                Student = new Student
                {
                    EnrollmentDate = DateTime.Now
                },
                AllCourses = _db.GetAllCourses(),
                AllDepartments = _db.GetAllDepartments(),
                SelectedCourseIds = new List<int>()
            };

            return View(viewModel);
        }


        // POST: /Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Student,SelectedCourseIds")] StudentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                int newStudentId = _db.AddStudent(viewModel.Student);

                if (viewModel.SelectedCourseIds != null && viewModel.SelectedCourseIds.Any())
                {
                    _db.AddStudentCourses(newStudentId, viewModel.SelectedCourseIds);
                }

                return RedirectToAction(nameof(Index));
            }

            viewModel.AllCourses = _db.GetAllCourses();
            viewModel.AllDepartments = _db.GetAllDepartments();
            return View(viewModel);
        }




        // GET: /Student/Edit/5
        public IActionResult Edit(int id)
        {
            var student = _db.GetStudentById(id);
            if (student == null)
                return NotFound();

            var viewModel = new StudentViewModel
            {
                Student = student,
                AllCourses = _db.GetAllCourses(),
                AllDepartments = _db.GetAllDepartments(), // ✅ Add this line
                SelectedCourseIds = new List<int>()
            };


            return View(viewModel);
        }


        // POST: /Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StudentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _db.UpdateStudent(viewModel.Student);

                _db.UpdateStudentCourses(viewModel.Student.StudentID, viewModel.SelectedCourseIds ?? new List<int>());

                return RedirectToAction(nameof(Index));
            }

            viewModel.AllCourses = _db.GetAllCourses();
            viewModel.AllDepartments = _db.GetAllDepartments();
            return View(viewModel);
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
