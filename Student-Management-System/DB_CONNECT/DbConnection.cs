using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Student_Management_System.Models;

namespace Student_Management_System.DB_CONNECT
{
    public class DbConnection
    {
        private readonly string _connectionString;

        public DbConnection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        // CREATE: Add a new student
        public void AddStudent(Student student)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(@"
        INSERT INTO Students 
        (FirstName, LastName, Email, Gender, DOB, Phone, Address, DepartmentID, EnrollmentDate) 
        VALUES 
        (@FirstName, @LastName, @Email, @Gender, @DOB, @Phone, @Address, @DepartmentID, @EnrollmentDate)", connection);

            command.CommandType = CommandType.Text;

            command.Parameters.AddWithValue("@FirstName", student.FirstName);
            command.Parameters.AddWithValue("@LastName", student.LastName);
            command.Parameters.AddWithValue("@Email", student.Email);
            command.Parameters.AddWithValue("@Gender", student.Gender);
            command.Parameters.AddWithValue("@DOB", student.DOB);
            command.Parameters.AddWithValue("@Phone", student.Phone);
            command.Parameters.AddWithValue("@Address", student.Address);
            command.Parameters.AddWithValue("@DepartmentID", student.DepartmentID);
            command.Parameters.AddWithValue("@EnrollmentDate", student.EnrollmentDate);

            connection.Open();
            command.ExecuteNonQuery();
        }


        // READ: Get all students from the database
        public List<Student> GetAllStudents()
        {
            List<Student> studentsList = new List<Student>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM Students", connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Student student = new Student
                            {
                                StudentID = Convert.ToInt32(reader["StudentID"]),
                                FirstName = reader["FirstName"].ToString() ?? string.Empty,
                                LastName = reader["LastName"].ToString() ?? string.Empty,
                                Email = reader["Email"].ToString() ?? string.Empty,
                                Gender = reader["Gender"].ToString() ?? string.Empty,
                                DOB = Convert.ToDateTime(reader["DOB"]),
                                Phone = reader["Phone"].ToString() ?? string.Empty,
                                Address = reader["Address"].ToString() ?? string.Empty,
                                DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                                EnrollmentDate = Convert.ToDateTime(reader["EnrollmentDate"])
                            };

                            studentsList.Add(student);
                        }
                    }
                }
            }

            return studentsList;
        }

        // READ: Get a student by ID
        public Student? GetStudentById(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand("SELECT * FROM Students WHERE StudentID = @StudentID", connection);
            command.Parameters.AddWithValue("@StudentID", id);
            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Student
                {
                    StudentID = Convert.ToInt32(reader["StudentID"]),
                    FirstName = reader["FirstName"].ToString() ?? string.Empty,
                    LastName = reader["LastName"].ToString() ?? string.Empty,
                    Email = reader["Email"].ToString() ?? string.Empty,
                    Gender = reader["Gender"].ToString() ?? string.Empty,
                    DOB = Convert.ToDateTime(reader["DOB"]),
                    Phone = reader["Phone"].ToString() ?? string.Empty,
                    Address = reader["Address"].ToString() ?? string.Empty,
                    DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                    EnrollmentDate = Convert.ToDateTime(reader["EnrollmentDate"])

                };
            }
            return null;
        }

        // UPDATE: Update a student
        public void UpdateStudent(Student student)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand("UPDATE Students SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Gender = @Gender, DOB = @DOB, Phone = @Phone, Address = @Address, DepartmentID = @DepartmentID, EnrollmentDate = @EnrollmentDate WHERE StudentID = @StudentID", connection);
            command.CommandType = CommandType.Text;

            command.Parameters.AddWithValue("@StudentID", student.StudentID);
            command.Parameters.AddWithValue("@FirstName", student.FirstName);
            command.Parameters.AddWithValue("@LastName", student.LastName);
            command.Parameters.AddWithValue("@Email", student.Email);
            command.Parameters.AddWithValue("@Gender", student.Gender);
            command.Parameters.AddWithValue("@DOB", student.DOB);
            command.Parameters.AddWithValue("@Phone", student.Phone);
            command.Parameters.AddWithValue("@Address", student.Address);
            command.Parameters.AddWithValue("@DepartmentID", student.DepartmentID);
            command.Parameters.AddWithValue("@EnrollmentDate", student.EnrollmentDate);

            connection.Open();
            command.ExecuteNonQuery();
        }

        // DELETE: Delete student by id

        public void DeleteStudent(int studentId) 
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand("DELETE FROM Students WHERE StudentID = @StudentID", connection);
            command.Parameters.AddWithValue("@StudentID", studentId);
            connection.Open();
            command.ExecuteNonQuery();
        }

        // SEARCH: Delete student by query

        public List<Student> SearchStudents(string query)
        {
            List<Student> result = new List<Student>();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(@"
        SELECT DISTINCT s.*
        FROM Students s
        LEFT JOIN Departments d ON s.DepartmentID = d.DepartmentID
        LEFT JOIN StudentCourses sc ON s.StudentID = sc.StudentID
        LEFT JOIN Courses c ON sc.CourseID = c.CourseID
        WHERE
            s.FirstName LIKE @q OR
            s.LastName LIKE @q OR
            s.Gender LIKE @q OR
            s.Address LIKE @q OR
            d.DepartmentName LIKE @q OR
            c.CourseName LIKE @q", connection);

            command.Parameters.AddWithValue("@q", $"%{query}%");

            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Student
                {
                    StudentID = Convert.ToInt32(reader["StudentID"]),
                    FirstName = reader["FirstName"].ToString() ?? string.Empty,
                    LastName = reader["LastName"].ToString() ?? string.Empty,
                    Email = reader["Email"].ToString() ?? string.Empty,
                    Gender = reader["Gender"].ToString() ?? string.Empty,
                    DOB = Convert.ToDateTime(reader["DOB"]),
                    Phone = reader["Phone"].ToString() ?? string.Empty,
                    Address = reader["Address"].ToString() ?? string.Empty,
                    DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                    EnrollmentDate = Convert.ToDateTime(reader["EnrollmentDate"])
                });
            }

            return result;
        }








    }
}
