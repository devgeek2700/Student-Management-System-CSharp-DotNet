using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Student_Management_System.Models
{
    public class TeacherViewModel
    {
        public Teacher Teacher { get; set; } = new();
        public List<Course> AllCourses { get; set; } = new();
        public List<Department> AllDepartments { get; set; } = new();
        public List<int> SelectedCourseIds { get; set; } = new();


    }
}
