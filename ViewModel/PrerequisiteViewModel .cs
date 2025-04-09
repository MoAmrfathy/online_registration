using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class PrerequisiteViewModel
    {
        public int C_id { get; set; }
        public int PrerequisiteId { get; set; }
        public int? RequiredCourseId { get; set; } 
        public int? MinimumCredits { get; set; }    
    }

}
