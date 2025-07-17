using Student_Management_System.Models.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Student_Management_System.Models
{
    public class Teacher
    {
        public int TeacherID { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        public Gender Gender { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [Phone]
        [StringLength(15)]
        public string? Phone { get; set; }

        [StringLength(100)]
        public string? Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime? HireDate { get; set; }

        [StringLength(100)]
        public string? Qualification { get; set; }

        [Display(Name = "Subject Taught")]
        [StringLength(100)]
        public string? SubjectTeacher { get; set; }

        [Required]
        [DisplayName("Department")]
        public int DepartmentID { get; set; }


    }
}
