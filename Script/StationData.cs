using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RushEightProject
{
    internal class StationData
    {
        public string Name { get; set; }
        public List<StationNode> NextStations { get; set; } = new List<StationNode>();
    }
}
