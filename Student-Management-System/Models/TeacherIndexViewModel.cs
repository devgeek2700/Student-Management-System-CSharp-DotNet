namespace Student_Management_System.Models
{
    public class TeacherIndexViewModel
    {
        public int TeacherID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string DepartmentName { get; set; } = "N/A";
        public string CoursesTaught { get; set; } = string.Empty;
    }


}
