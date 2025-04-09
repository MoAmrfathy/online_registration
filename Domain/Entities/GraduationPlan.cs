using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Domain.Entities
{
    public class GraduationPlan
    {
        public int GraduationPlanId { get; set; }
        public int D_id { get; set; }
        public int RequiredCredits { get; set; }
        public string g_name { get; set; }

        // Navigation properties
        public Department Department { get; set; }
        public ICollection<Term> Terms { get; set; }
        public ICollection<Student> Students { get; set; } 
    }
}

