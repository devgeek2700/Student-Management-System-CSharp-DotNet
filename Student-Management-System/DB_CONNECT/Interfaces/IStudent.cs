using Student_Management_System.Models;

namespace Student_Management_System.DB_CONNECT.Interfaces
{
    public interface IStudent
    {
        public int AddStudent(Models.Student student);
        public List<Models.Student> GetAllStudents();
        public Models.Student? GetStudentById(int id);
        public void UpdateStudent(Models.Student student);
        public void DeleteStudent(int studentId);
        public List<Models.StudentListViewModel> SearchStudents(string query);

        public List<Course> GetAllCourses();

        public List<Department> GetAllDepartments();

        public List<StudentCourse> GetAllStudentCourses();

        public void AddStudentCourses(int studentId, List<int> courseIds);

        public void UpdateStudentCourses(int studentId, List<int> courseIds);
    }
}
