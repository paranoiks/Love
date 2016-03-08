using UnityEngine;
using System.Collections;

public class Point {

    public int x;
    public int y;
    
    public Point()
    {
        x = y = 0;
    }

    public Point(int n)
    {
        x = y = n;
    }

    public Point(int newX, int newY)
    {
        x = newX;
        y = newY;
    }

    public Point(Vector3 v)
    {
        x = (int)v.x;
        y = (int)v.y;
    }

    public Point(Vector2 v)
    {
        x = (int)v.x;
        y = (int)v.y;
    }
}
