using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Astar {

    private static Dictionary<Point, Node> nodes;

    private static void CreateNodes()
    {
        nodes = new Dictionary<Point, Node>();

        foreach (TileScript tile in LevelManager.Instance.Tiles.Values)
        {
            nodes.Add(tile.GridPosition, new Node(tile));
            //Debug.Log(tile.GridPosition.X + " " + tile.GridPosition.Y);
        }
        //foreach(TileScript til int )
    }

    public static Stack<Node> GetPath(Point start, Point goal)
    {
        if(nodes == null)
        {
            CreateNodes();
        }

        HashSet<Node> openList = new HashSet<Node>();
        HashSet<Node> closeList = new HashSet<Node>();
        Stack<Node> finalPath = new Stack<Node>();


        Node currentNode = nodes[start];

        openList.Add(currentNode);

        while (openList.Count > 0)
        {

            Point tempToSouth = new Point(-1, 0);
            for (int y = -1; y <= 1; y++)
            {
                Point tempToNorth = new Point(tempToSouth);
                for (int x = -1; x <= 1; x++)
                {
                    Point neighbourPos = new Point(currentNode.GridPostion.X + tempToNorth.X, currentNode.GridPostion.Y + tempToNorth.Y);
                    if (!LevelManager.Instance.Tiles[neighbourPos].IsCorner && LevelManager.Instance.Tiles[neighbourPos].IsWalkable && neighbourPos != currentNode.GridPostion)
                    {
                        int gCost = 0;
                        if(x == y)
                        {
                            if (!ConnentedDiagonaly(currentNode, nodes[neighbourPos]))
                            {
                                break;
                            }
                            gCost = 5;
                        }
                        else if (Math.Abs(x) == Math.Abs(y))
                        {
                            if (!ConnentedDiagonaly(currentNode, nodes[neighbourPos]))
                            {
                                break;
                            }
                            gCost = 10;
                        }
                        else 
                        {
                            gCost = 7;
                        }
                        Node neighbour = nodes[neighbourPos];

                        if (openList.Contains(neighbour))
                        {
                            if (currentNode.G + gCost < neighbour.G)
                            {
                                neighbour.CalcValues(currentNode, nodes[goal], gCost);
                            }
                        }
                        else if (!closeList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                            neighbour.CalcValues(currentNode, nodes[goal], gCost);
                        }
                    }
                    tempToNorth.X += (float)0.5;
                    tempToNorth.Y += (float)0.25;
                }
                tempToSouth.X += (float)0.5;
                tempToSouth.Y -= (float)0.25;
            }
            openList.Remove(currentNode);
            closeList.Add(currentNode);

            if (openList.Count > 0)
            {
                currentNode = openList.OrderBy(n => n.F).First();
            }

            if (currentNode == nodes[goal])
            {
                while (currentNode.GridPostion != start)
                {
                    finalPath.Push(currentNode);
                    currentNode = currentNode.Parent;
                }
                break;
            }
        }

        //debuging only
        //GameObject.Find("AstarDebugger").GetComponent<AstarDebugger>().DebugPath(openList, closeList, finalPath);
        return finalPath;
    }

    private static bool ConnentedDiagonaly(Node currentNode, Node neighbor)
    {
        Point direction = neighbor.GridPostion - currentNode.GridPostion;
        Point first = new Point(currentNode.GridPostion.X + direction.X/2, currentNode.GridPostion.Y + direction.X/4);
        Point third = new Point(currentNode.GridPostion.X + direction.X/2, currentNode.GridPostion.Y - direction.X/4);
        Point second = new Point(currentNode.GridPostion.X + direction.Y, currentNode.GridPostion.Y + direction.Y/2);
        Point fourth = new Point(currentNode.GridPostion.X - direction.Y, currentNode.GridPostion.Y + direction.Y/2);

        if(!LevelManager.Instance.Tiles[first].IsCorner && !LevelManager.Instance.Tiles[first].IsWalkable)
        {
            return false;
        }

        if (!LevelManager.Instance.Tiles[second].IsCorner && !LevelManager.Instance.Tiles[second].IsWalkable)
        {
            return false;
        }
        if (!LevelManager.Instance.Tiles[third].IsCorner && !LevelManager.Instance.Tiles[third].IsWalkable)
        {
            return false;
        }
        if (!LevelManager.Instance.Tiles[fourth].IsCorner && !LevelManager.Instance.Tiles[fourth].IsWalkable)
        {
            return false;
        }

        return true;
    }
    
}
