using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationChart
{
    public class TreeNode
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double Prelim { get; set; }
        public double Modifier { get; set; }
        public TreeNode Parent { get; set; }
        public List<TreeNode> Offspring { get; set; }
        public bool IsLeaf { get { return Offspring?.Count > 0; } }
        public TreeNode LeftNeighbor { get; set; }
        public TreeNode FirstChild { get { return Offspring?[0]; } }
    }
}
