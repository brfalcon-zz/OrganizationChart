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
        private const int SUBTREE_SEPARATION = 5;
        private const int SIBLING_SEPARATION = 4;
        private const int LEVEL_SEPARATION = 5;
        private const int MAXIMUM_DEPTH = 10;

        public enum FRAME_TYPE { FRAME, NO_FRAME }
        public enum ROOT_ORIENTATION { NORTH, SOUTH, EAST, WEST }

        private List<TreeNode> levelZero = new List<TreeNode>();

        private FRAME_TYPE frameType = FRAME_TYPE.FRAME;
        private ROOT_ORIENTATION rootOrientation = ROOT_ORIENTATION.NORTH;

        private double xTopAjustment;
        private double yTopAjustment;
        private double meanWidth;

        private double modsum;

        public Tree() { }
        
        private TreeNode GetPrevNodeAtLevel(int levelNbr)
        {
            if (levelZero == null)
            {
                return null;
            }

            int i = 0;

            if(i < levelZero.Count)
            {
                return levelZero[i];
            }
            else
            {
                return null;
            }
        }
        private bool SetPrevNodeAtLevel(int levelNbr, TreeNode thisNode)
        {
            if(levelZero == null)
            {
                levelZero = new List<TreeNode>();
                levelZero.Add(thisNode);

                return true;
            }
            else
            {
                if (levelNbr < levelZero.Count)
                {
                    levelZero[levelNbr--] = thisNode;

                    return true;
                }
                else
                {
                    levelZero.Add(thisNode);

                    return true;
                }
            }
        }
        private void InitPrevNodeAtLevel()
        {
            for (int i = 0; i < levelZero.Count; i++)
            {
                levelZero[i] = null;
            }
        }
        private bool CheckExtentsRange(double xTemp, double yTemp)
        {
            return !(Math.Abs(xTemp) > 32000 || Math.Abs(xTemp) > 32000);
        }
        private void TreeMeanNodeSize(TreeNode leftNode, TreeNode rightNode)
        {
            meanWidth = 0;

            switch (rootOrientation)
            {
                case ROOT_ORIENTATION.NORTH:
                case ROOT_ORIENTATION.SOUTH:
                    if(leftNode != null)
                    {
                        meanWidth += NODE_WIDTH / 2;
                        if(frameType != FRAME_TYPE.NO_FRAME)
                        {
                            meanWidth += FRAME_THICKNESS;
                        }
                    }

                    if(rightNode != null)
                    {
                        meanWidth += NODE_WIDTH / 2;
                        if (frameType != FRAME_TYPE.NO_FRAME)
                        {
                            meanWidth += FRAME_THICKNESS;
                        }
                    }

                    break;
                case ROOT_ORIENTATION.EAST:
                case ROOT_ORIENTATION.WEST:
                    if (leftNode != null)
                    {
                        meanWidth += NODE_HEIGHT / 2;
                        if (frameType != FRAME_TYPE.NO_FRAME)
                        {
                            meanWidth += FRAME_THICKNESS;
                        }
                    }

                    if (rightNode != null)
                    {
                        meanWidth += NODE_HEIGHT / 2;
                        if (frameType != FRAME_TYPE.NO_FRAME)
                        {
                            meanWidth += FRAME_THICKNESS;
                        }
                    }

                    break;
            }
        }
        private TreeNode TreeGetLeftMost(TreeNode thisNode, int currentLevel, int searchDepth)
        {
            TreeNode leftMost;
            TreeNode rightMost;

            if(currentLevel == searchDepth)
            {
                leftMost = thisNode;
            }
            else if (thisNode.IsLeaf)
            {
                leftMost = null;
            }
            else
            {
                for(leftMost = TreeGetLeftMost(rightMost = thisNode.FirstChild, currentLevel + 1, searchDepth);
                    leftMost == null && rightMost.HasRightSibling;
                    leftMost = TreeGetLeftMost(rightMost = rightMost.RightSbling, currentLevel + 1, searchDepth))
                {

                }
            }

            return leftMost;
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
            double distance;
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

                distance = (neighbor.Prelim + leftModsum + SUBTREE_SEPARATION + meanWidth) - leftMost.Prelim + rightModsum;

                if(distance > 0)
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
                        portion = distance / leftSiblings;
                        for(tempPtr = thisNode;
                            tempPtr != ancestorNeighbor;
                            tempPtr = tempPtr.LeftSibling)
                        {
                            tempPtr.Prelim   += distance;
                            tempPtr.Modifier += distance;

                            distance -= portion;
                        }
                    }
                    else
                    {
                        return;
                    }
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

            neighbor = leftMost.LeftNeighbor;
        }
        private bool TreeFirstWalk(TreeNode thisNode, int currentLevel)
        {
            TreeNode leftMost;
            TreeNode rightmost;
            double midPoint;
            
            if (thisNode != null)
            {
                thisNode.LeftNeighbor = GetPrevNodeAtLevel(currentLevel);

                if (!(SetPrevNodeAtLevel(currentLevel, thisNode)))
                {
                    return false;
                }

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
                    if (TreeFirstWalk(leftMost = rightmost = thisNode.FirstChild, currentLevel + 1))
                    {
                        while (rightmost.HasRightSibling)
                        {
                            if (TreeFirstWalk(rightmost = rightmost.RightSbling, currentLevel + 1))
                            {

                            }
                            else
                            {
                                return false;
                            }
                        }

                        midPoint = (leftMost.Prelim + rightmost.Prelim) / 2;

                        TreeMeanNodeSize(thisNode.LeftSibling, thisNode);

                        if (thisNode.HasLeftSibling)
                        {
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
                
            }
            else
            {
                return false;
            }
            return true;
        }
        private bool TreeSecondWalk(TreeNode thisNode, int currentLevel)
        {
            bool result = true;
            double xTemp = 0, yTemp = 0;
            double newModsum;
            modsum = 0;

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
                        modsum = newModsum += thisNode.Modifier;
                        result = TreeSecondWalk(thisNode.FirstChild, currentLevel + 1);
                        newModsum -= thisNode.Modifier;
                    }

                    if(thisNode.HasRightSibling && result == true)
                    {
                        modsum = newModsum;
                        result = TreeSecondWalk(thisNode.RightSbling, currentLevel);
                    }
                }
                else
                {
                    result = false;
                }
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
                    return TreeSecondWalk(apexNode, 0);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        } 


    }
}
