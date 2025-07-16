using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Student_Management_System.Models
{
    public class Student
    {
        [Key]
        public int StudentID { get; set; }

        [Required]

        [DisplayName("First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Date of Birth")]
        public DateTime DOB { get; set; }    // DATE

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        [DisplayName("Department")]
        public int DepartmentID { get; set; }


        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }    // DATE



        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}
