using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Dtos
{
    public class CourseDto
    {
        public string Title { get; set; } = string.Empty;
        public int Credits { get; set; }
    }
}