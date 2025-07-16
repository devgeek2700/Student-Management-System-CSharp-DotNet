using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Student_Management_System.Models;
using System.Data;

namespace Student_Management_System.DB_CONNECT
{
    public class Departments
    {
        private readonly string _connectionString;

        public Departments(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        // Create 
        public void AddDepartment(Department department)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(@"INSERT INTO Departments  
        ( DepartmentName, HeadOfDepartment) 
        VALUES 
        (@DepartmentName, @HeadOfDepartment)", connection);
            command.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
            command.Parameters.AddWithValue("@HeadOfDepartment", department.HeadOfDepartment);

            connection.Open();
            command.ExecuteNonQuery();
        }

        // READ: Get all Department from the database
        public List<Department> GetAllDepartments()
        {
            List<Department> departmentList = new List<Department>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM Departments", connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Department department = new Department
                            {
                                DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                                DepartmentName = reader["DepartmentName"].ToString() ?? string.Empty,
                                HeadOfDepartment = reader["HeadOfDepartment"].ToString() ?? string.Empty,
                            };

                            departmentList.Add(department);
                        }
                    }
                }
            }

            return departmentList;
        }

        // READ: Get a student by ID
        public Department? GetDepartmentById(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand("SELECT * FROM Departments WHERE DepartmentID = @DepartmentID", connection);
            command.Parameters.AddWithValue("@DepartmentID", id);
            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Department
                {
                    DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                    DepartmentName = reader["DepartmentName"].ToString() ?? string.Empty,
                    HeadOfDepartment = reader["HeadOfDepartment"].ToString() ?? string.Empty,
                };
            }
            return null;
        }


        // UPDATE: Update a Department
        public void UpdateDepartment(Department department)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(@"
        UPDATE Departments 
        SET DepartmentName = @DepartmentName, 
            HeadOfDepartment = @HeadOfDepartment
        WHERE DepartmentID = @DepartmentID", connection);

            command.CommandType = CommandType.Text;

            command.Parameters.AddWithValue("@DepartmentID", department.DepartmentID);
            command.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
            command.Parameters.AddWithValue("@HeadOfDepartment", department.HeadOfDepartment);

            connection.Open();
            command.ExecuteNonQuery();
        }


        // DELETE: Delete Department by id

        public void DeleteDepartment(int departmentId)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand("DELETE FROM Departments WHERE DepartmentID = @DepartmentID", connection);
            command.Parameters.AddWithValue("@DepartmentID", departmentId);
            connection.Open();
            command.ExecuteNonQuery();
        }

        // SERACH BAR

        public List<Department> SearchDepartments(string query)
        {
            List<Department> result = new List<Department>();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(@"
        SELECT DISTINCT d.*
        FROM Departments d
        LEFT JOIN Courses c ON d.DepartmentID = c.DepartmentID
        LEFT JOIN Teachers t ON d.DepartmentID = t.DepartmentID
        WHERE 
            d.DepartmentName LIKE @q OR
            d.HeadOfDepartment LIKE @q OR
            c.CourseName LIKE @q OR
            (t.FirstName + ' ' + t.LastName) LIKE @q", connection);

            command.Parameters.AddWithValue("@q", "%" + query + "%");
            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Department dept = new Department
                {
                    DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                    DepartmentName = reader["DepartmentName"].ToString() ?? string.Empty,
                    HeadOfDepartment = reader["HeadOfDepartment"].ToString() ?? string.Empty
                };
                result.Add(dept);
            }

            return result;
        }



    }
}