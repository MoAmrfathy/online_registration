using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ViewModel
{
    public class EnrollmentViewModel
    {
        public int EnrollmentId { get; set; }
        public int S_id { get; set; }
        public int C_id { get; set; }
        public int GraduationPlanId { get; set; }
        public int GroupId { get; set; }
        public List<CourseGroupPair> SelectedCourses { get; set; } = new List<CourseGroupPair>();
    }

}
