using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }
        public string Name { get; set; } = string.Empty;

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public int Budget { get; set; }


        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        
        public int? InstructorID { get; set; }
        public Instructor? Administrator { get; set; }
        public ICollection<Course> Courses { get; set; } = new List<Course>();

        public string FullName
        {
            get
            {
                return Name + " (" + DepartmentID + ")";
            }
        }
    }
}