using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class StudentViewModel
    {
        public int S_id { get; set; }
        public string S_Name { get; set; }
        public double GPA { get; set; }
        public int TotalCreditAchievement { get; set; }
        public bool IsGraduate { get; set; }
        public string College { get; set; }
        public int Semester { get; set; }
        public long Reg_no { get; set; }
        public long PIN { get; set; }
        public int GraduationPlanId { get; set; }
        public bool isfired { get; set; }
        public int D_id { get; set; }
        public string D_name { get; set; }
        public string GraduationPlanName { get; set; }
        public string TermName { get; set; } 
    }

}
