using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain
{
    public class RepositertyReturn<T>
    {
        public int Count { get; set; }

        public T Data { get; set; }
    }
}
