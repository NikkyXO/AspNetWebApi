using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class OfficeAssignment
    {
        // Primary key: one to one relationship with Instructor
        // The InstructorId is the primary key and foreign key
        // It is required and must be unique
        // only one office role/assignment per instructor
        [Key]
        public long InstructorId { get; set; }
        public string Location { get; set; } = string.Empty;

        // Navigation property
        public Instructor Instructor { get; set; } = null!;
        
    }
}