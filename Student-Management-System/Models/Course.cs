using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Student_Management_System.Models
{
    public class Course
    {

        [Key]
        public int CourseID { get; set; }

        [Required]
        [DisplayName("Course Name")]
        public string CourseName { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [DisplayName("Department")]
        public int DepartmentID { get; set; }

        [Required]
        public int Credits { get; set; }
    }
}