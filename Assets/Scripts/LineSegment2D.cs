using UnityEngine;
using System.Collections;

public class LineSegment2D {

    private Vector2 p1;
    private Vector2 p2;

    public Vector2 Point1
    {
        get { return p1; }
        set { p1 = value; }
    }

    public Vector2 Point2
    {
        get { return p2; }
        set { p2 = value; }
    }

    public Vector3 Point1V3
    {
        get { return MathHelpers.ToVector3(p1); }
    }

    public Vector3 Point2V3
    {
        get { return MathHelpers.ToVector3(p2); }
    }

    public LineSegment2D(Vector2 point1, Vector2 point2)
    {
        p1 = point1;
        p2 = point2;
    }
}
