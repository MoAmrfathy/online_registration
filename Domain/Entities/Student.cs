using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Entities
{
    public class Student
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
        public bool isfired { get; set; }

        // Foreign Keys 
        public int D_id { get; set; }
        public int GraduationPlanId { get; set; }

        // Navigation properties
        public Department Department { get; set; }
        public GraduationPlan GraduationPlan { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<SelectedCourse> SelectedCourses { get; set; }


    }


}
