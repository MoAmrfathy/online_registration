using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class GroupDetailsViewModel
    {
        public int SelectedCourseId { get; set; }
        public int CourseId { get; set; }
        public int GroupId { get; set; } 
        public string GroupName { get; set; } = string.Empty; 
        public string CourseName { get; set; } = string.Empty; 
        public string CourseCode { get; set; } = string.Empty; 
        public string LectureName { get; set; } = string.Empty; 
        public string SectionName { get; set; } = string.Empty;
        public string LectureTime { get; set; } = string.Empty; 
        public string SectionTime { get; set; } = string.Empty; 
        public string TermName { get; set; } = string.Empty;
        public int CreditHours { get; set; }
        public int Capacity { get; set; }
    }
}
