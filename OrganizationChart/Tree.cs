using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationChart
{
    public class Tree
    {
        private const int NODE_WIDTH = 20;
        private const int NODE_HEIGHT = 10;
        private const int FRAME_THICKNESS = 1;
        private const int SUBTREE_SEPARATION = 4;
        private const int SIBLING_SEPARATION = 4;
        private const int LEVEL_SEPARATION = 5;
        private const int MAXIMUM_DEPTH = 10;

        public enum FRAME_TYPE { FRAME, NO_FRAME }
        public enum ROOT_ORIENTATION { NORTH, SOUTH, EAST, WEST }

        private List<TreeNode> prevNodes = new List<TreeNode>();

        private FRAME_TYPE frameType = FRAME_TYPE.FRAME;
        private ROOT_ORIENTATION rootOrientation = ROOT_ORIENTATION.NORTH;

        private double xTopAjustment;
        private double yTopAjustment;
        private double meanWidth;

        public Tree() { }
        
        private TreeNode GetPrevNodeAtLevel(int levelNbr)
        {
            if (prevNodes == null || (prevNodes != null && prevNodes.Count == 0))
            {
                return null;
            }

            if(0 <= levelNbr && levelNbr < prevNodes.Count)
            {
                return prevNodes[levelNbr];
            }
            else
            {
                return null;
            }

            
        }
        private void SetPrevNodeAtLevel(int levelNbr, TreeNode thisNode)
        {
            if(levelNbr + 1 > prevNodes.Count)
            {
                prevNodes.Add(thisNode);
            }else
            {
                prevNodes[levelNbr] = thisNode;
            }
        }
        private void InitPrevNodeAtLevel()
        {
            for (int i = 0; i < prevNodes.Count; i++)
            {
                prevNodes[i] = null;
            }
        }
        private bool CheckExtentsRange(double xTemp, double yTemp)
        {
            return !(Math.Abs(xTemp) > 32000 || Math.Abs(xTemp) > 32000);
        }
        private void TreeMeanNodeSize(TreeNode leftNode, TreeNode rightNode)
        {
            meanWidth = 2;

            //switch (rootOrientation)
            //{
            //    case ROOT_ORIENTATION.NORTH:
            //    case ROOT_ORIENTATION.SOUTH:
            //        if(leftNode != null)
            //        {
            //            meanWidth += NODE_WIDTH / 2;
            //            //if(frameType != FRAME_TYPE.NO_FRAME)
            //            //{
            //            //    meanWidth += FRAME_THICKNESS;
            //            //}
            //        }

            //        if(rightNode != null)
            //        {
            //            meanWidth += NODE_WIDTH / 2;
            //            //if (frameType != FRAME_TYPE.NO_FRAME)
            //            //{
            //            //    meanWidth += FRAME_THICKNESS;
            //            //}
            //        }

            //        break;
            //    case ROOT_ORIENTATION.EAST:
            //    case ROOT_ORIENTATION.WEST:
            //        if (leftNode != null)
            //        {
            //            meanWidth += NODE_HEIGHT / 2;
            //            //if (frameType != FRAME_TYPE.NO_FRAME)
            //            //{
            //            //    meanWidth += FRAME_THICKNESS;
            //            //}
            //        }

            //        if (rightNode != null)
            //        {
            //            meanWidth += NODE_HEIGHT / 2;
            //            //if (frameType != FRAME_TYPE.NO_FRAME)
            //            //{
            //            //    meanWidth += FRAME_THICKNESS;
            //            //}
            //        }

            //        break;
            //}
        }
        private TreeNode TreeGetLeftMost(TreeNode thisNode, int currentLevel, int searchDepth)
        {
            TreeNode leftMost;
            TreeNode rightMost;

            if(currentLevel >= searchDepth)
            {
                return thisNode;
            }
            else if (thisNode.IsLeaf)
            {
                return null;
            }
            else
            {
                rightMost = thisNode.FirstChild;
                leftMost = TreeGetLeftMost(rightMost, currentLevel + 1, searchDepth);

                while(leftMost != null && rightMost.HasRightSibling)
                {
                    rightMost = rightMost.RightSbling;
                    leftMost = TreeGetLeftMost(rightMost, currentLevel + 1, searchDepth);
                }

                return leftMost;
            }
        }
        private void TreeApportion(TreeNode thisNode, int currentLevel)
        {
            TreeNode leftMost;
            TreeNode neighbor;
            TreeNode ancestorLeftmost;
            TreeNode ancestorNeighbor;
            TreeNode tempPtr;

            int compareDepth;
            int depthToStop;
            int leftSiblings;

            double leftModsum;
            double rightModsum;
            double moveDistance;
            double portion;

            leftMost = thisNode.FirstChild;
            neighbor = leftMost.LeftNeighbor;

            compareDepth = 1;
            depthToStop = MAXIMUM_DEPTH - currentLevel;

            while(leftMost != null && neighbor != null && (compareDepth <= depthToStop))
            {
                rightModsum = leftModsum = 0;
                ancestorLeftmost = leftMost;
                ancestorNeighbor = neighbor;

                for (int i = 0; i < compareDepth; i++)
                {
                    ancestorLeftmost = ancestorLeftmost.Parent;
                    ancestorNeighbor = ancestorNeighbor.Parent;

                    rightModsum += ancestorLeftmost.Modifier;
                    leftModsum  += ancestorNeighbor.Modifier;
                }

                TreeMeanNodeSize(leftMost, neighbor);

                moveDistance = (neighbor.Prelim + leftModsum + SUBTREE_SEPARATION + meanWidth) - (leftMost.Prelim + rightModsum);

                if(moveDistance > 0)
                {
                    leftSiblings = 0;
                    for(tempPtr = thisNode;
                        tempPtr != null && tempPtr != ancestorNeighbor;
                        tempPtr = tempPtr.LeftNeighbor)
                    {
                        leftSiblings++;
                    }

                    if(tempPtr != null)
                    {
                        portion = moveDistance / leftSiblings;
                        for(tempPtr = thisNode;
                            tempPtr != null && tempPtr != ancestorNeighbor;
                            tempPtr = tempPtr.LeftSibling)
                        {
                            tempPtr.Prelim   += moveDistance;
                            tempPtr.Modifier += moveDistance;

                            moveDistance -= portion;
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                compareDepth++;
                if (leftMost.IsLeaf)
                {
                    leftMost = TreeGetLeftMost(thisNode, 0, compareDepth);
                }
                else
                {
                    leftMost = leftMost.FirstChild;
                }
            }
        }
        private bool TreeFirstWalk(TreeNode thisNode, int currentLevel)
        {
            TreeNode leftMost;
            TreeNode rightmost;
            double midPoint;
            
            thisNode.LeftNeighbor = GetPrevNodeAtLevel(currentLevel);

            SetPrevNodeAtLevel(currentLevel, thisNode);
                
            thisNode.Modifier = 0;

            if (thisNode.IsLeaf || currentLevel == MAXIMUM_DEPTH)
            {
                if (thisNode.HasLeftSibling)
                {
                    TreeMeanNodeSize(thisNode.LeftSibling, thisNode);

                    thisNode.Prelim = thisNode.LeftSibling.Prelim +
                                        SIBLING_SEPARATION +
                                        meanWidth;
                }
                else
                {
                    thisNode.Prelim = 0;
                }
            }
            else
            {
                leftMost = rightmost = thisNode.FirstChild;

                if (TreeFirstWalk(leftMost, currentLevel + 1))
                {
                    while (rightmost.HasRightSibling)
                    {
                        rightmost = rightmost.RightSbling;

                        if (TreeFirstWalk(rightmost, currentLevel + 1))
                        {

                        }
                        else
                        {
                            return false;
                        }
                    }

                    midPoint = (leftMost.Prelim + rightmost.Prelim) / 2;

                    if (thisNode.HasLeftSibling)
                    {
                        TreeMeanNodeSize(thisNode.LeftSibling, thisNode);

                        thisNode.Prelim = thisNode.LeftSibling.Prelim +
                                            SIBLING_SEPARATION +
                                            meanWidth;
                        thisNode.Modifier = thisNode.Prelim - midPoint;

                        TreeApportion(thisNode, currentLevel);
                    }
                    else
                    {
                        thisNode.Prelim = midPoint;
                    }
                }
                else
                {
                    return false;
                }
            } 
                
            
            return true;
        }
        private bool TreeSecondWalk(TreeNode thisNode, int currentLevel, double modsum)
        {
            bool result = true;
            double xTemp = 0, yTemp = 0;
            double newModsum;

            if(currentLevel <= MAXIMUM_DEPTH)
            {
                newModsum = modsum;
                switch (rootOrientation)
                {
                    case ROOT_ORIENTATION.NORTH:
                        xTemp = xTopAjustment +
                                (thisNode.Prelim + modsum);
                        yTemp = yTopAjustment +
                                (currentLevel * LEVEL_SEPARATION);
                        break;
                    case ROOT_ORIENTATION.SOUTH:
                        xTemp = xTopAjustment +
                                (thisNode.Prelim + modsum);
                        yTemp = yTopAjustment -
                                (currentLevel * LEVEL_SEPARATION);
                        break;
                    case ROOT_ORIENTATION.EAST:
                        xTemp = xTopAjustment +
                                (currentLevel * LEVEL_SEPARATION);
                        yTemp = yTopAjustment -
                                (thisNode.Prelim + modsum);
                        break;
                    case ROOT_ORIENTATION.WEST:
                        xTemp = xTopAjustment -
                                (currentLevel * LEVEL_SEPARATION);
                        yTemp = yTopAjustment -
                                (thisNode.Prelim + modsum);
                        break;
                }

                if(CheckExtentsRange(xTemp, yTemp))
                {
                    thisNode.XCoordinate = xTemp;
                    thisNode.YCoordinate = yTemp;

                    if (thisNode.HasChild)
                    {
                        result = TreeSecondWalk(thisNode.FirstChild, 
                                                currentLevel + 1,
                                                modsum + thisNode.Modifier);
                    }

                    if(result == true && thisNode.HasRightSibling)
                    {
                        result = TreeSecondWalk(thisNode.RightSbling, currentLevel + 1, modsum);
                    }
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = true;
            }

            return result;
        }
        public bool TreePosition(TreeNode apexNode)
        {
            if(apexNode != null)
            {
                InitPrevNodeAtLevel();

                if(TreeFirstWalk(apexNode, 0))
                {
                    switch (rootOrientation)
                    {
                        case ROOT_ORIENTATION.NORTH:
                        case ROOT_ORIENTATION.SOUTH:
                            xTopAjustment = apexNode.XCoordinate - apexNode.Prelim;
                            yTopAjustment = apexNode.YCoordinate;

                            break;
                        case ROOT_ORIENTATION.EAST:
                        case ROOT_ORIENTATION.WEST:
                            xTopAjustment = apexNode.XCoordinate;
                            yTopAjustment = apexNode.YCoordinate + apexNode.Prelim;

                            break;
                    }
                    return TreeSecondWalk(apexNode, 0, 0);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        } 
    }
}
