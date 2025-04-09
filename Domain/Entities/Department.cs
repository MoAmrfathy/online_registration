using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Department
    {
        public int D_id { get; set; }
        public string D_name { get; set; }

        // Navigation properties
        public ICollection<Student> Students { get; set; }
        public ICollection<Course> Courses { get; set; }
        public GraduationPlan GraduationPlan { get; set; }
    }

}
