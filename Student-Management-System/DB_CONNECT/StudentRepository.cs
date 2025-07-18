
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Student_Management_System.Models;
using Student_Management_System.Models.Enums;
using System.Data;
using Student_Management_System.DB_CONNECT.Interfaces;


namespace Student_Management_System.DB_CONNECT
{
    public class StudentRepository : IStudent
    {
        private readonly string _connectionString;

        public StudentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public int AddStudent(Student student)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = new SqlCommand(@"
                    INSERT INTO Students 
                    (FirstName, LastName, Email, Gender, DOB, Phone, Address, DepartmentID, EnrollmentDate) 
                    OUTPUT INSERTED.StudentID
                    VALUES 
                    (@FirstName, @LastName, @Email, @Gender, @DOB, @Phone, @Address, @DepartmentID, @EnrollmentDate)", connection);

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
                return (int)command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AddStudent: " + ex.Message);
                throw;
            }
        }

        public List<Student> GetAllStudents()
        {
            List<Student> studentsList = new();
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = new SqlCommand("SELECT * FROM Students", connection);
                command.CommandTimeout = 180;
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    studentsList.Add(new Student
                    {
                        StudentID = Convert.ToInt32(reader["StudentID"]),
                        FirstName = reader["FirstName"].ToString() ?? "",
                        LastName = reader["LastName"].ToString() ?? "",
                        Email = reader["Email"].ToString() ?? "",
                        Gender = Enum.TryParse(reader["Gender"].ToString(), out Gender g) ? g : Gender.Other,
                        DOB = Convert.ToDateTime(reader["DOB"]),
                        Phone = reader["Phone"].ToString() ?? "",
                        Address = reader["Address"].ToString() ?? "",
                        DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                        EnrollmentDate = Convert.ToDateTime(reader["EnrollmentDate"])
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetAllStudents: " + ex.Message);
                throw;
            }

            return studentsList;
        }

        public Student? GetStudentById(int id)
        {
            try
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
                        FirstName = reader["FirstName"].ToString() ?? "",
                        LastName = reader["LastName"].ToString() ?? "",
                        Email = reader["Email"].ToString() ?? "",
                        Gender = Enum.TryParse(reader["Gender"].ToString(), out Gender g) ? g : Gender.Other,
                        DOB = Convert.ToDateTime(reader["DOB"]),
                        Phone = reader["Phone"].ToString() ?? "",
                        Address = reader["Address"].ToString() ?? "",
                        DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                        EnrollmentDate = Convert.ToDateTime(reader["EnrollmentDate"])
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetStudentById: " + ex.Message);
                throw;
            }

            return null;
        }

        public void UpdateStudent(Student student)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = new SqlCommand(@"
                    UPDATE Students SET 
                        FirstName = @FirstName, LastName = @LastName, Email = @Email, 
                        Gender = @Gender, DOB = @DOB, Phone = @Phone, 
                        Address = @Address, DepartmentID = @DepartmentID, 
                        EnrollmentDate = @EnrollmentDate 
                    WHERE StudentID = @StudentID", connection);

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
            catch (Exception ex)
            {
                Console.WriteLine("Error in UpdateStudent: " + ex.Message);
                throw;
            }
        }

        public void DeleteStudent(int studentId)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();

                // First, delete from StudentCourses
                using (SqlCommand deleteCourses = new SqlCommand("DELETE FROM StudentCourses WHERE StudentID = @StudentID", connection))
                {
                    deleteCourses.Parameters.AddWithValue("@StudentID", studentId);
                    deleteCourses.ExecuteNonQuery();
                }

                // Then, delete from Students
                using (SqlCommand deleteStudent = new SqlCommand("DELETE FROM Students WHERE StudentID = @StudentID", connection))
                {
                    deleteStudent.Parameters.AddWithValue("@StudentID", studentId);
                    deleteStudent.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DeleteStudent: " + ex.Message);
                throw;
            }
        }


        public List<StudentListViewModel> SearchStudents(string query)
        {
            List<StudentListViewModel> result = new();
            try
            {
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
                    result.Add(new StudentListViewModel
                    {
                        StudentID = Convert.ToInt32(reader["StudentID"]),
                        FirstName = reader["FirstName"].ToString() ?? "",
                        LastName = reader["LastName"].ToString() ?? "",
                        Email = reader["Email"].ToString() ?? "",
                        Gender = Enum.TryParse(reader["Gender"].ToString(), out Gender g) ? g : Gender.Other,
                        DOB = Convert.ToDateTime(reader["DOB"]),
                        Phone = reader["Phone"].ToString() ?? "",
                        Address = reader["Address"].ToString() ?? "",
                        //DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                        EnrollmentDate = Convert.ToDateTime(reader["EnrollmentDate"])
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in SearchStudents: " + ex.Message);
                throw;
            }

            return result;
        }

        public List<Course> GetAllCourses()
        {
            List<Course> courses = new();
            try
            {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetAllCourses: " + ex.Message);
                throw;
            }

            return courses;
        }

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

        public List<StudentCourse> GetAllStudentCourses()
        {
            var list = new List<StudentCourse>();
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = new SqlCommand("SELECT * FROM StudentCourses", connection);
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new StudentCourse
                    {
                        StudentID = Convert.ToInt32(reader["StudentID"]),
                        CourseID = Convert.ToInt32(reader["CourseID"])
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetAllStudentCourses: " + ex.Message);
                throw;
            }

            return list;
        }

        public void AddStudentCourses(int studentId, List<int> courseIds)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();

                foreach (var courseId in courseIds)
                {
                    using SqlCommand command = new SqlCommand(
                        "INSERT INTO StudentCourses (StudentID, CourseID) VALUES (@StudentID, @CourseID)", connection);
                    command.Parameters.AddWithValue("@StudentID", studentId);
                    command.Parameters.AddWithValue("@CourseID", courseId);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AddStudentCourses: " + ex.Message);
                throw;
            }
        }

        public void UpdateStudentCourses(int studentId, List<int> courseIds)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();

                using SqlCommand deleteCommand = new SqlCommand(
                    "DELETE FROM StudentCourses WHERE StudentID = @StudentID", connection);
                deleteCommand.Parameters.AddWithValue("@StudentID", studentId);
                deleteCommand.ExecuteNonQuery();

                foreach (var courseId in courseIds)
                {
                    using SqlCommand insertCommand = new SqlCommand(
                        "INSERT INTO StudentCourses (StudentID, CourseID) VALUES (@StudentID, @CourseID)", connection);
                    insertCommand.Parameters.AddWithValue("@StudentID", studentId);
                    insertCommand.Parameters.AddWithValue("@CourseID", courseId);
                    insertCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in UpdateStudentCourses: " + ex.Message);
                throw;
            }
        }
    }
}
