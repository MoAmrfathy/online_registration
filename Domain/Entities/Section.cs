using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Section
    {
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan sectionStartTime { get; set; } 
        public TimeSpan sectionEndTime { get; set; }    

        // Foreign Key
        public int C_id { get; set; }


        // Navigation Properties
        public Course Course { get; set; }
        
    }
}
