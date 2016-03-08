using UnityEngine;
using System.Collections;

public class GridMember {

    public Point GridPosition;
    public GridMember Parent;
    public int CurrentDistance;
    public GameObject CurrentGameObject;

    public GridMember()
    {

    }

    public GridMember(Point position, GridMember parent, int currentDistance, GameObject currentGameObject)
    {
        GridPosition = position;
        Parent = parent;
        CurrentDistance = currentDistance;
        CurrentGameObject = currentGameObject;
    }
}
