using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public StudentViewModel Student { get; set; }
    }
}
