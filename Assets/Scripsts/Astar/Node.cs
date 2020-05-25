using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public Point GridPostion { get; private set; }

    public TileScript TileRef { get; private set; }

    public Node Parent { get; private set; }

    public Vector2 WorldPosition { get; set; }

    public float G { get; set; }
    public float H { get; set; }
    public float F { get; set; }

    public Node(TileScript tileRef)
    {
        this.TileRef = tileRef;
        this.GridPostion = tileRef.GridPosition;
        this.WorldPosition = tileRef.WorldPosition;
    }
   
    public void CalcValues(Node parent,Node goal, int gCost)
    {
        this.Parent = parent;
        this.G = parent.G + gCost;
        this.H = (Math.Abs(goal.GridPostion.X - GridPostion.X) + Math.Abs(goal.GridPostion.Y - GridPostion.Y)) * 7;
        this.F = G + H;
    }
}
