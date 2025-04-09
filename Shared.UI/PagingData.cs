using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.UI
{
   

    public class PagingData
    {
        public PagingData(int totalCount, int pageSize)
        {
            TotalCount = totalCount;
            PageSize = pageSize;
        }

        public PagingData()
        {

        }



        public int TotalCount { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;

        public int PageCount => ((TotalCount - 1) / PageSize) + 1;
    }
}
