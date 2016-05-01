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

    public static Vector3 GetWorldCoordinatesFlatened(Vector3 point, CameraPosition cameraPosition)
    {
        float worldSize = Globals.WorldSize;
        switch(cameraPosition)
        {
            case CameraPosition.CameraFront:
                point.z = 0;
                break;
            case CameraPosition.CameraRight:
                point.x = worldSize;
                break;
            case CameraPosition.CameraTop:
                point.y = worldSize;
                break;
        }

        return point;
    }

    public static Vector3 GetRaycastDirectionFromCameraPosition(CameraPosition cameraPosition)
    {
        switch (cameraPosition)
        {
            case CameraPosition.CameraFront:
                return new Vector3(0, 0, 1);
            case CameraPosition.CameraRight:
                return new Vector3(-1, 0, 0);
            case CameraPosition.CameraTop:
                return new Vector3(0, -1, 0);
            default:
                return Vector3.zero;
        }
    }
}
