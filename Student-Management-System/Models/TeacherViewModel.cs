using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


using Student_Management_System.Models;

public class TeacherViewModel
{
    public Teacher Teacher { get; set; }
    public List<int> SelectedCourseIds { get; set; }
    public List<Course> AllCourses { get; set; }
}
