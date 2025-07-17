using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Student_Management_System.Models;
using System.Data;

namespace Student_Management_System.DB_CONNECT
{
    public class Courses
    {
        private readonly string _connectionString;

        public Courses(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        // CREATE
        public void AddCourse(Course course)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = new SqlCommand(@"
                    INSERT INTO Courses (CourseName, Description, DepartmentID, Credits) 
                    VALUES (@CourseName, @Description, @DepartmentID, @Credits)", connection);

                command.Parameters.AddWithValue("@CourseName", course.CourseName);
                command.Parameters.AddWithValue("@Description", course.Description);
                command.Parameters.AddWithValue("@DepartmentID", course.DepartmentID);
                command.Parameters.AddWithValue("@Credits", course.Credits);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AddCourse: " + ex.Message);
                throw;
            }
        }

        // READ: Get all courses
        public List<Course> GetAllCourses()
        {
            List<Course> coursesList = new();
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = new SqlCommand("SELECT * FROM Courses", connection);
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Course course = new Course
                    {
                        CourseID = Convert.ToInt32(reader["CourseID"]),
                        CourseName = reader["CourseName"].ToString() ?? string.Empty,
                        Description = reader["Description"].ToString() ?? string.Empty,
                        DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                        Credits = Convert.ToInt32(reader["Credits"]),
                    };
                    coursesList.Add(course);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetAllCourses: " + ex.Message);
                throw;
            }

            return coursesList;
        }

        // READ: Get course by ID
        public Course? GetCourseById(int id)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = new SqlCommand("SELECT * FROM Courses WHERE CourseID = @CourseID", connection);
                command.Parameters.AddWithValue("@CourseID", id);
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Course
                    {
                        CourseID = Convert.ToInt32(reader["CourseID"]),
                        CourseName = reader["CourseName"].ToString() ?? string.Empty,
                        Description = reader["Description"].ToString() ?? string.Empty,
                        DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                        Credits = Convert.ToInt32(reader["Credits"]),
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetCourseById: " + ex.Message);
                throw;
            }

            return null;
        }

        // UPDATE
        public void UpdateCourse(Course course)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = new SqlCommand(@"
                    UPDATE Courses 
                    SET CourseName = @CourseName, 
                        Description = @Description, 
                        DepartmentID = @DepartmentID, 
                        Credits = @Credits 
                    WHERE CourseID = @CourseID", connection);

                command.Parameters.AddWithValue("@CourseID", course.CourseID);
                command.Parameters.AddWithValue("@CourseName", course.CourseName);
                command.Parameters.AddWithValue("@Description", course.Description);
                command.Parameters.AddWithValue("@DepartmentID", course.DepartmentID);
                command.Parameters.AddWithValue("@Credits", course.Credits);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in UpdateCourse: " + ex.Message);
                throw;
            }
        }

        // DELETE
        public void DeleteCourse(int courseId)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = new SqlCommand("DELETE FROM Courses WHERE CourseID = @CourseID", connection);
                command.Parameters.AddWithValue("@CourseID", courseId);
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DeleteCourse: " + ex.Message);
                throw;
            }
        }

        // SEARCH
        public List<Course> SearchCourses(string query)
        {
            List<Course> result = new();
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = new SqlCommand(@"
                    SELECT c.* FROM Courses c
                    LEFT JOIN Departments d ON c.DepartmentID = d.DepartmentID
                    WHERE c.CourseName LIKE @q
                       OR c.Description LIKE @q
                       OR d.DepartmentName LIKE @q", connection);

                command.Parameters.AddWithValue("@q", "%" + query + "%");
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Course course = new Course
                    {
                        CourseID = Convert.ToInt32(reader["CourseID"]),
                        CourseName = reader["CourseName"].ToString() ?? string.Empty,
                        Description = reader["Description"].ToString() ?? string.Empty,
                        DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                        Credits = Convert.ToInt32(reader["Credits"])
                    };
                    result.Add(course);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in SearchCourses: " + ex.Message);
                throw;
            }

            return result;
        }

        // READ: All Departments
        public List<Department> GetAllDepartments()
        {
            List<Department> departments = new();
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = new SqlCommand("SELECT * FROM Departments", connection);
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    departments.Add(new Department
                    {
                        DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                        DepartmentName = reader["DepartmentName"].ToString() ?? ""
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetAllDepartments: " + ex.Message);
                throw;
            }

            return departments;
        }
    }
}
