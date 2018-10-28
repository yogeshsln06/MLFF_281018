using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.Common
{
    public class SortingPagingInfo
    {
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int CurrentPageIndex { get; set; }
        public String SearchText { get; set; }

        public int RecordCount { get; set; }
    }
}
