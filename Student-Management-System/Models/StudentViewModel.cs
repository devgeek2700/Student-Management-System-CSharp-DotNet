using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Student_Management_System.Models
{
    public class StudentViewModel
    {
        public Student Student { get; set; } = new Student();
        public List<int> SelectedCourseIds { get; set; } = new List<int>();
        public List<Course> AllCourses { get; set; } = new List<Course>();
        public List<Department> AllDepartments { get; set; } = new List<Department>();

    }
}
