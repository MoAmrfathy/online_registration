using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SelectedCourse
    {
        [Key]
        public int SelectedCourseId { get; set; }


        public int C_id { get; set; }
        public int S_id { get; set; }
        public int GroupId { get; set; }
        public DateTime? SelectionDate { get; set; }

        // Navigation properties
        public Student Student { get; set; }
        public Course Course { get; set; }
        public Group Group { get; set; }
    }
}
