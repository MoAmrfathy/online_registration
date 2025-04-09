using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Group
    {
        public int GroupId { get; set; } 
        public string GroupName { get; set; }
        public int Capacity { get; set; }
        public int MaxCapacity { get; set; }

        // Foreign keys
        public int LectureId { get; set; } 
        public int SectionId { get; set; }
        public int C_id { get; set; } 
      



        // Navigation properties
        public Lecture Lecture { get; set; } 
        public Section Section { get; set; } 
        public Course Course { get; set; } 
    }
}
