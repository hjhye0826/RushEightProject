using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RushEightProject
{ 
    internal class SearchResult
    {
        public string Route;
        public int TotalTime;

        public SearchResult()
        {
            Route = string.Empty;
            TotalTime = 0;
        }

        public SearchResult(string route, int totalTime)
        {
            Route = route;
            TotalTime = totalTime;
        }
    }
}
