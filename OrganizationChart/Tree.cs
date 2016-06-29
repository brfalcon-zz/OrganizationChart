using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationChart
{
    public class Tree
    {
        private int _maxX;
        private int _maxY;

        private int xTopAjustment;
        private int yTopAjustment;

        private int maxDepth;
        private int siblingSeparation;

        private TreeNode levelZeroPtr;

        private bool PositionTree(TreeNode node)
        {
            if(node != null)
            {
                InitPrevNodeList();

                FirstWalk(node, 0);

                xTopAjustment = node.X + node.Prelim;
                yTopAjustment = node.Y;

                return SecondWalk(node, 0, 0);
            }
            else
            {
                return false;
            }
        }

        private bool FirstWalk(TreeNode node, int level)
        {
            node.LeftNeighbor = GetPrevNodeAtLevel(level);
            SetPrevNodeAtLevel(level, node);

            node.Modifier = 0;

            if(node.IsLeaf || level == maxDepth)
            {
                if (HasLeftSibling(node))
                {
                    node.Prelim = LeftSibling(node).Prelim + 
                                  siblingSeparation + 
                                  MeanNodeSize(LeftSibling(node), node);
                }
                else
                {
                    node.Prelim = 0;
                }
            }
            else
            {
                TreeNode leftMost, rightMost;
                leftMost = rightMost = node.FirstChild;

                FirstWalk(leftMost, level + 1);

                while (HasRightSibling(rightMost)){
                    rightMost = RightSibling(rightMost);
                    FirstWalk(rightMost, level + 1);
                }

                double midPoint = (leftMost.Prelim + rightMost.Prelim) / 2;

                if (HasLeftSibling(node))
                {
                    node.Prelim = LeftSibling(node).Prelim +
                                  siblingSeparation +
                                  MeanNodeSize(LeftSibling(node), node);
                    node.Modifier = node.Prelim - midPoint;
                    ApPortion(node, level);
                }
            }

            return false;
        }


        private bool SecondWalk(TreeNode node, int level, int modSum)
        {
            //TO-DO

            return false;
        }

        private void ApPortion(TreeNode node, int level)
        {
            //TO DO
        }

        private TreeNode GetLeftMost(TreeNode node, int level, int depth)
        {
            if(level >= depth)
            {
                return node;
            }
            else if (node.IsLeaf)
            {
                return null;
            }
            else
            {
                TreeNode rightMost = node.FirstChild;
                TreeNode leftMost = GetLeftMost(rightMost, level + 1, depth);

                while(leftMost != null && HasRightSibling(rightMost))
                {
                    rightMost = RightSibling(rightMost);
                    leftMost = GetLeftMost(rightMost, level + 1, depth);
                }

                return leftMost;
            }
        }

        private double MeanNodeSize(TreeNode leftNode, TreeNode rightNode)
        {
            int nodeSize = 0;

            if(leftNode != null)
            {
                nodeSize += RightSize(leftNode);
            }
            if(rightNode != null)
            {
                nodeSize += LeftSize(rightNode);
            }
            return nodeSize;
        }

        private int RightSize(TreeNode leftNode)
        {
            //To do
            return 0;
        }

        private int LeftSize(TreeNode rightNode)
        {
            //to do
            return 0;
        }

        private bool CheckExtentsRange(int xValue, int yValue)
        {
            return (xValue <= _maxX && yValue <= _maxY);
        }

        private void InitPrevNodeList()
        {
            //TO-DO
            var tempPtr = levelZeroPtr;
            while(tempPtr != null)
            {
                Prev d
            }
        }

        private TreeNode GetPrevNodeAtLevel(int level)
        {
            //TO-DO
            return null;
        }

        private void SetPrevNodeAtLevel(int level, TreeNode node)
        {

        }
        
        private bool HasLeftSibling(TreeNode node)
        {
            //TO-DO
            return false;
        }

        private TreeNode LeftSibling(TreeNode node)
        {
            //TO-DO
            return null;
        }

        private TreeNode RightSibling(TreeNode rightMost)
        {
            //TO-DO
            return null;
        }

        private bool HasRightSibling(TreeNode rightMost)
        {
            //TO-DO
            return false;
        }
    }
}
