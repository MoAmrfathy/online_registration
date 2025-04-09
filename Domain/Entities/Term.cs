using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Term
    {
        public int TermId { get; set; }
        public string Term_name { get; set; }
        public int GraduationPlanId { get; set; }
       

        // Navigation Properties
        public GraduationPlan GraduationPlan { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}

