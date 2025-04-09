using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Prerequisite
    {
        public int PrerequisiteId { get; set; }
        public int C_id { get; set; }
        public int? RequiredCourseId { get; set; }
        public double? MinimumGPA { get; set; }  // Ensure this line is added

        // Navigation properties
        public Course Course { get; set; }
        public Course RequiredCourse { get; set; }
    }
}