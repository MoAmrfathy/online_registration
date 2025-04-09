using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Entities
{
    public class Lecture
    {
        public int LectureId { get; set; }
        public string LectureName { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan LectureStartTime { get; set; }
        public TimeSpan LectureEndTime { get; set; }

        // Foreign Key
        public int C_id { get; set; }

        // Navigation Properties
        public Course Course { get; set; }
    }
}
