using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class Student
    {
        public long Id { get; set; }
        [Required, StringLength(50, MinimumLength = 2)]
        [Column("FirstName")]
        public string FirstName { get; set; } = string.Empty;


        [Required, StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();


        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }
    }
}