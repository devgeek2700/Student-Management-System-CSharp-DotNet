using Student_Management_System.Models;

namespace Student_Management_System.DB_CONNECT.Interfaces
{
    public interface ITeacher
    {
        public void AddTeacher(Teacher teacher);
        public List<Teacher> GetAllTeachers();
        public Teacher? GetTeacherById(int id);
        public void UpdateTeacher(Teacher teacher);
        public void DeleteTeacher(int teacherId);
        public List<Teacher> SearchTeachers(string query);
        public List<int> GetCoursesByTeacherId(int teacherId);
        public void AssignCoursesToTeacher(int teacherId, List<int> courseIds);

        public void UpdateTeacherCourses(int teacherId, List<int> courseIds);
        public List<TeacherCourse> GetAllTeacherCourses();
        public List<Course> GetAllCourses();

        public List<Department> GetAllDepartments();
    }
}
