using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Point 
{ 
    public float X { get; set; }
    public float Y { get; set; }
    
    public Point(float x, float y)
    {
        this.X = x;
        this.Y = y;
    }

    public Point(Point point)
    {
        this.X = point.X;
        this.Y = point.Y;
    }

    public static bool operator ==(Point first, Point second)
    {
        return first.X == second.X && first.Y == second.Y;
    }

    public static bool operator !=(Point first, Point second)
    {
        return first.X != second.X || first.Y != second.Y;
    }

    public static Point operator -(Point first, Point sencond)
    {
        return new Point(first.X - sencond.X, first.Y - sencond.Y);
    }

    public static Point operator +(Point first, Point sencond)
    {
        return new Point(first.X + sencond.X, first.Y + sencond.Y);
    }

    public string ToString()
    {
        return "x:" + X + " y:" + Y;
    }
}
