using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long CourseId { get; set; }
        public string Title { get; set; } = string.Empty;

        [Range(0, 5)]
        public int Credits { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new Collection<Enrollment>();
        public ICollection<CourseAssignment> CourseAssignments { get; set; } = new Collection<CourseAssignment>();
        public Department Department { get; set; } = null!;

    }
}