using System;
using System.Collections.Generic;

namespace Student_Management_System.Models
{
    public class TeacherListViewModel
    {
        public int TeacherID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public string DepartmentName { get; set; } = string.Empty; 

        public List<string> CoursesIncharge { get; set; } = new(); 
    }
}
