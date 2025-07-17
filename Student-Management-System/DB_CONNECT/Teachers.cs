using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Student_Management_System.Models;
using System.Data;

namespace Student_Management_System.DB_CONNECT
{
    public class Teachers
    {
        private readonly string _connectionString;

        public Teachers(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        // CREATE: Add a new teacher
        public void AddTeacher(Teacher teacher)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(@"
                INSERT INTO Teachers 
                (FirstName, LastName, Gender, Email, Phone, Address, HireDate, Qualification, SubjectTeacher, DepartmentID) 
                VALUES 
                (@FirstName, @LastName, @Gender, @Email, @Phone, @Address, @HireDate, @Qualification, @SubjectTeacher, @DepartmentID)", connection);

            command.Parameters.AddWithValue("@FirstName", teacher.FirstName);
            command.Parameters.AddWithValue("@LastName", teacher.LastName);
            command.Parameters.AddWithValue("@Gender", teacher.Gender ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Email", teacher.Email ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Phone", teacher.Phone ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Address", teacher.Address ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@HireDate", teacher.HireDate ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Qualification", teacher.Qualification ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@SubjectTeacher", teacher.SubjectTeacher ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@DepartmentID", teacher.DepartmentID);

            connection.Open();
            command.ExecuteNonQuery();
        }

        // READ: Get all teachers
        public List<Teacher> GetAllTeachers()
        {
            List<Teacher> teachers = new();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand("SELECT * FROM Teachers", connection);
            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                teachers.Add(new Teacher
                {
                    TeacherID = Convert.ToInt32(reader["TeacherID"]),
                    FirstName = reader["FirstName"].ToString() ?? string.Empty,
                    LastName = reader["LastName"].ToString() ?? string.Empty,
                    Gender = reader["Gender"].ToString(),
                    Email = reader["Email"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    Address = reader["Address"].ToString(),
                    HireDate = reader["HireDate"] != DBNull.Value ? Convert.ToDateTime(reader["HireDate"]) : null,
                    Qualification = reader["Qualification"].ToString(),
                    SubjectTeacher = reader["SubjectTeacher"].ToString(),
                    DepartmentID = Convert.ToInt32(reader["DepartmentID"])
                });
            }

            return teachers;
        }

        // READ: Get teacher by ID
        public Teacher? GetTeacherById(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand("SELECT * FROM Teachers WHERE TeacherID = @TeacherID", connection);
            command.Parameters.AddWithValue("@TeacherID", id);
            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Teacher
                {
                    TeacherID = Convert.ToInt32(reader["TeacherID"]),
                    FirstName = reader["FirstName"].ToString() ?? string.Empty,
                    LastName = reader["LastName"].ToString() ?? string.Empty,
                    Gender = reader["Gender"].ToString(),
                    Email = reader["Email"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    Address = reader["Address"].ToString(),
                    HireDate = reader["HireDate"] != DBNull.Value ? Convert.ToDateTime(reader["HireDate"]) : null,
                    Qualification = reader["Qualification"].ToString(),
                    SubjectTeacher = reader["SubjectTeacher"].ToString(),
                    DepartmentID = Convert.ToInt32(reader["DepartmentID"])
                };
            }

            return null;
        }

        // UPDATE: Update a teacher
        public void UpdateTeacher(Teacher teacher)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(@"
                UPDATE Teachers 
                SET FirstName = @FirstName, LastName = @LastName, Gender = @Gender, Email = @Email, 
                    Phone = @Phone, Address = @Address, HireDate = @HireDate, 
                    Qualification = @Qualification, SubjectTeacher = @SubjectTeacher, 
                    DepartmentID = @DepartmentID
                WHERE TeacherID = @TeacherID", connection);

            command.Parameters.AddWithValue("@TeacherID", teacher.TeacherID);
            command.Parameters.AddWithValue("@FirstName", teacher.FirstName);
            command.Parameters.AddWithValue("@LastName", teacher.LastName);
            command.Parameters.AddWithValue("@Gender", teacher.Gender ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Email", teacher.Email ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Phone", teacher.Phone ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Address", teacher.Address ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@HireDate", teacher.HireDate ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Qualification", teacher.Qualification ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@SubjectTeacher", teacher.SubjectTeacher ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@DepartmentID", teacher.DepartmentID);

            connection.Open();
            command.ExecuteNonQuery();
        }

        // DELETE: Delete teacher by ID
        public void DeleteTeacher(int teacherId)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand("DELETE FROM Teachers WHERE TeacherID = @TeacherID", connection);
            command.Parameters.AddWithValue("@TeacherID", teacherId);
            connection.Open();
            command.ExecuteNonQuery();
        }

        // SEARCH 
        public List<Teacher> SearchTeachers(string query)
        {
            List<Teacher> result = new();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(@"
        SELECT DISTINCT t.*
        FROM Teachers t
        LEFT JOIN Departments d ON t.DepartmentID = d.DepartmentID
        LEFT JOIN TeacherCourses tc ON t.TeacherID = tc.TeacherID
        LEFT JOIN Courses c ON tc.CourseID = c.CourseID
        WHERE t.FirstName LIKE @q OR t.LastName LIKE @q
           OR d.DepartmentName LIKE @q
           OR c.CourseName LIKE @q
    ", connection);

            command.Parameters.AddWithValue("@q", "%" + query + "%");
            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Teacher
                {
                    TeacherID = Convert.ToInt32(reader["TeacherID"]),
                    FirstName = reader["FirstName"].ToString() ?? "",
                    LastName = reader["LastName"].ToString() ?? "",
                    Email = reader["Email"].ToString() ?? "",
                    Phone = reader["Phone"].ToString() ?? "",
                    Address = reader["Address"].ToString() ?? "",
                    HireDate = Convert.ToDateTime(reader["HireDate"]),
                    Qualification = reader["Qualification"].ToString() ?? "",
                    DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                    SubjectTeacher = reader["SubjectTeacher"].ToString() ?? ""
                });
            }

            return result;
        }



        // Get all courses assigned to a teacher
        public List<int> GetCoursesByTeacherId(int teacherId)
        {
            var courseIds = new List<int>();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(
                "SELECT CourseID FROM TeacherCourses WHERE TeacherID = @TeacherID", connection);
            command.Parameters.AddWithValue("@TeacherID", teacherId);

            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                courseIds.Add(Convert.ToInt32(reader["CourseID"]));
            }

            return courseIds;
        }

        // Assign multiple courses to a teacher
        public void AssignCoursesToTeacher(int teacherId, List<int> courseIds)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            foreach (int courseId in courseIds)
            {
                using SqlCommand command = new SqlCommand(
                    "INSERT INTO TeacherCourses (TeacherID, CourseID) VALUES (@TeacherID, @CourseID)", connection);
                command.Parameters.AddWithValue("@TeacherID", teacherId);
                command.Parameters.AddWithValue("@CourseID", courseId);
                command.ExecuteNonQuery();
            }
        }

        // Update courses assigned to a teacher
        public void UpdateTeacherCourses(int teacherId, List<int> courseIds)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            // Delete old mappings
            using (SqlCommand deleteCommand = new SqlCommand(
                "DELETE FROM TeacherCourses WHERE TeacherID = @TeacherID", connection))
            {
                deleteCommand.Parameters.AddWithValue("@TeacherID", teacherId);
                deleteCommand.ExecuteNonQuery();
            }

            // Insert new mappings
            foreach (var courseId in courseIds)
            {
                using SqlCommand insertCommand = new SqlCommand(
                    "INSERT INTO TeacherCourses (TeacherID, CourseID) VALUES (@TeacherID, @CourseID)", connection);
                insertCommand.Parameters.AddWithValue("@TeacherID", teacherId);
                insertCommand.Parameters.AddWithValue("@CourseID", courseId);
                insertCommand.ExecuteNonQuery();
            }
        }

        // Get all teacher-course mappings (optional for listing/report)
        public List<TeacherCourse> GetAllTeacherCourses()
        {
            List<TeacherCourse> mappings = new();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand("SELECT * FROM TeacherCourses", connection);
            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                mappings.Add(new TeacherCourse
                {
                    TeacherID = Convert.ToInt32(reader["TeacherID"]),
                    CourseID = Convert.ToInt32(reader["CourseID"])
                });
            }

            return mappings;
        }


        // Get all courses
        public List<Course> GetAllCourses()
        {
            List<Course> courses = new();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand("SELECT * FROM Courses", connection);
            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                courses.Add(new Course
                {
                    CourseID = Convert.ToInt32(reader["CourseID"]),
                    CourseName = reader["CourseName"].ToString() ?? ""
                });
            }

            return courses;
        }

        // Get all departments
        public List<Department> GetAllDepartments()
        {
            List<Department> departments = new();

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

            return departments;
        }



    }
}
