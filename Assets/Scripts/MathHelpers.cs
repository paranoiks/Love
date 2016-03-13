using UnityEngine;
using System.Collections;

public class MathHelpers {
    
    public static Vector3 ToVector3(Vector2 v)
    {
        return new Vector3(v.x, v.y, 0);
    }

    public static Vector2 ToVector2(Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static Vector2 ConvertCoordinatesTo2D(Vector3 position, CameraPosition cameraPosition)
    {
        Vector2 newPosition = Vector2.zero;
        switch (cameraPosition)
        {
            case CameraPosition.CameraFront:
                newPosition.x = position.x;
                newPosition.y = position.y;
                break;
            case CameraPosition.CameraRight:
                newPosition.x = position.z;
                newPosition.y = position.y;
                break;
            case CameraPosition.CameraTop:
                newPosition.x = position.x;
                newPosition.y = position.z;
                break;
        }

        return newPosition;
    }
}
