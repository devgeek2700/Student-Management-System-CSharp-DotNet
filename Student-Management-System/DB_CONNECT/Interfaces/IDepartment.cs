using Student_Management_System.Models;

namespace Student_Management_System.DB_CONNECT.Interfaces
{
    public interface IDepartment
    {
        public void AddDepartment(Department department);
        public List<Department> GetAllDepartments();
        public Department? GetDepartmentById(int id);
        public void UpdateDepartment(Department department);
        public void DeleteDepartment(int departmentId);

        public List<Department> SearchDepartments(string query);

    }
}
