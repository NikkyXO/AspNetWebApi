using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class Instructor
    {

        [Required, StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; } = String.Empty;

        [Required, StringLength(50, MinimumLength = 2),]
        public string FirstMidName { get; set; } = String.Empty;


        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        public string FullName
        {
            get { return LastName + ", " + FirstMidName; }
        }

        public ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();
        public ICollection<OfficeAssignment> OfficeAssignment { get; set; } = new List<OfficeAssignment>();

    }
}