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

        // Create 
        public void AddCourse(Course course) 
        { 
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(@"INSERT INTO Courses  
        ( CourseName, Description, DepartmentID, Credits) 
        VALUES 
        (@CourseName, @Description, @DepartmentID, @Credits)", connection);
            command.Parameters.AddWithValue("@CourseName", course.CourseName);
            command.Parameters.AddWithValue("@Description", course.Description);
            command.Parameters.AddWithValue("@DepartmentID", course.DepartmentID);
            command.Parameters.AddWithValue("@Credits", course.Credits);

            connection.Open();
            command.ExecuteNonQuery();
        }

        // READ: Get all courses from the database
        public List<Course> GetAllCourses()
        {
            List<Course> coursesList = new List<Course>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM Courses", connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
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
                }
            }

            return coursesList;
        }

        // READ: Get a student by ID
        public Course? GetCourseById(int id)
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
            return null;
        }


        // UPDATE: Update a Course
        public void UpdateCourse(Course course)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(@"
        UPDATE Courses 
        SET CourseName = @CourseName, 
            Description = @Description, 
            DepartmentID = @DepartmentID, 
            Credits = @Credits 
        WHERE CourseID = @CourseID", connection); command.CommandType = CommandType.Text;

            command.Parameters.AddWithValue("@CourseID", course.CourseID);
            command.Parameters.AddWithValue("@CourseName", course.CourseName);
            command.Parameters.AddWithValue("@Description", course.Description);
            command.Parameters.AddWithValue("@DepartmentID", course.DepartmentID);
            command.Parameters.AddWithValue("@Credits", course.Credits);
            connection.Open();
            command.ExecuteNonQuery();
        }

        // DELETE: Delete course by id

        public void DeleteCourse(int courseId)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand("DELETE FROM Courses WHERE CourseID = @CourseID", connection);
            command.Parameters.AddWithValue("@CourseID", courseId);
            connection.Open();
            command.ExecuteNonQuery();
        }

        // SEARCH BAR
        public List<Course> SearchCourses(string query)
        {
            List<Course> result = new List<Course>();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(@"
        SELECT c.*
        FROM Courses c
        LEFT JOIN Departments d ON c.DepartmentID = d.DepartmentID
        WHERE 
            c.CourseName LIKE @q
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

            return result;
        }

    }
}