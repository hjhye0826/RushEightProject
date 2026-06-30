using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RushEightProject
{
    internal class Station
    {
        public string Name { get; set; }
        public List<StationNode> Nodes { get; set; } = new List<StationNode>();
    }
}
