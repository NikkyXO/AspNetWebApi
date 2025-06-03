using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{

    public enum Grade
    {
        A, B, C, D, F
    }

    /// <summary>
    /// Represents a many to many relationship between students and courses.
    /// Represents an enrollment of a student in a course.
    /// </summary>
    public class Enrollment
    {
        public long Id { get; set; }
        public long CourseId { get; set; }
        public Course? Course { get; set; }
        public long StudentId { get; set; }
        public Student? Student { get; set; }

        public Grade? Grade { get; set; }


        /// <summary>
        /// Gets or sets the grade for the enrollment.
        /// </summary>
        /// <value>The grade of the enrollment.</value>
        public Grade? EnrollmentGrade
        {
            get => Grade;
            set => Grade = value;
        }
        
        /// <summary>
        /// Returns a string representation of the enrollment.
        /// </summary>
        /// <returns>A string that represents the enrollment.</returns>
        public override string ToString()
        {
            return $"{Student?.FirstName} {Student?.LastName} enrolled in {Course?.Title} with grade {Grade}";
        }
    }
}