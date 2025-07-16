using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Student_Management_System.Models
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }

        [Required]
        [DisplayName("Department Name")]
        public string DepartmentName { get; set; } = string.Empty;

        [Required]
        public string HeadOfDepartment { get; set; } = string.Empty;

    }
}
