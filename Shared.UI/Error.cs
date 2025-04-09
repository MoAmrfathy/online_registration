using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.UI
{
    public class Error
    {
        public string Message { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }

        public Error()
        {
        }

        public Error(string message)
        {
            Message = message;
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
