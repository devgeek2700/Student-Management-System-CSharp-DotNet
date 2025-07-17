using Student_Management_System.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Student_Management_System.Models
{
    public class StudentListViewModel
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }

        public List<string> CoursesTaken { get; set; } = new();
    }
}