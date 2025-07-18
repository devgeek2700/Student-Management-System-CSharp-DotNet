using Student_Management_System.Models;

namespace Student_Management_System.DB_CONNECT.Interfaces
{
    public interface ICourse
    {
        public void AddCourse(Course course);
        public List<Course> GetAllCourses();
        public Course? GetCourseById(int id);

        public void UpdateCourse(Course course);
        public void DeleteCourse(int courseId);
        public List<Course> SearchCourses(string query);
        public List<Department> GetAllDepartments();
    }
}
