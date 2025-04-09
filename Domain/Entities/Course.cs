using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Course
    {
        public int C_id { get; set; }
        public string C_code { get; set; }
        public string C_Title { get; set; }
        public int Credits { get; set; }

        // Foreign Key 
        public int D_id { get; set; }
        public int TermId { get; set; }



        // Navigation properties
        public Term Term { get; set; }
        public Department Department { get; set; }
        public ICollection<Prerequisite> Prerequisites { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Lecture> Lectures { get; set; }
        public ICollection<Section> Sections { get; set; }
        public ICollection<Group> Groups { get; set; }
        public ICollection<SelectedCourse> SelectedCourses { get; set; }


    }

}
