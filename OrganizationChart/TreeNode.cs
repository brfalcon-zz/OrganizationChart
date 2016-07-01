using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationChart
{
    public class TreeNode
    {
        public TreeNode Parent { get; set; }
        public List<TreeNode> Offspring { get; set; }
        public TreeNode LeftSibling { get; set; }
        public TreeNode RightSbling { get; set; }
        public double XCoordinate { get; set; }
        public double YCoordinate { get; set; }

        public TreeNode FirstChild {
            get
            {
                return (Offspring.Count > 0 ? Offspring[0] : null);
            }
        }
        public TreeNode LeftNeighbor { get; set; }
        public double Prelim { get; set; }
        public double Modifier { get; set; }

        public string Info { get; set; }

        public bool IsLeaf { get { return Offspring == null; } }
        public bool HasChild { get { return Offspring != null; } }
        public bool HasLeftSibling { get { return LeftSibling != null; } }
        public bool HasRightSibling { get { return RightSbling != null; } }

        public TreeNode(string info)
        {
            this.Info = info;
            this.Offspring = new List<TreeNode>();
        }
    }
}
