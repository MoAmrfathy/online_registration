using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Entities
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int S_id { get; set; } 
        public int C_id { get; set; }
        public int GraduationPlanId { get; set; } 
        public int GroupId { get; set; } 
        public bool IsCompleted { get; set; }

        // Navigation properties
        public Student Student { get; set; }
        public Course Course { get; set; }
        public GraduationPlan GraduationPlan { get; set; }
        public Group Group { get; set; }
    }

}

