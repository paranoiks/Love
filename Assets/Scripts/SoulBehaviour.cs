﻿using UnityEngine;

public class SoulBehaviour : MonoBehaviour {

    [SerializeField]
    public GameObject Soulmate;
    private bool GridSet = false;
    public CameraPosition CurrentCameraPosition { get; set; }
    private GameObject[,] CurrentGrid;
    public GameObject[,] CurrentGridP { get { return CurrentGrid; } set { GridSet = true; CurrentGrid = value; } }
    private GridMember[,] CurrentGridBFS;

    private float LocalX;
    private float LocalY;
    
	// Use this for initialization
	void Start () {
        CurrentCameraPosition = CameraPosition.CameraFront;
	}

    private void SetInitialGrid()
    {
        GridSet = true;
        CurrentGrid = new GameObject[Globals.WorldSize, Globals.WorldSize];

        var blocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (var block in blocks)
        {
            Vector2 newPosition = ConvertCoordinatesTo2D(block.transform.position);
            CurrentGrid[(int)newPosition.x, (int)newPosition.y] = block;
        }
    }

    private Vector2 ConvertCoordinatesTo2D(Vector3 position)
    {
        Vector2 newPosition = Vector2.zero;
        switch (CurrentCameraPosition)
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

    private void FindPath()
    {

    }

    // Update is called once per frame
    void Update () {
	    if(Input.GetKeyDown(KeyCode.G))
        {
            CallMate();
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            PrintCurrentGrid();
        }
	}

    private void PrintCurrentGrid()
    {
        string row = "";

        for (int i=0;i<Globals.WorldSize;i++)
        {
            for(int j=0;j<Globals.WorldSize;j++)
            {
                if(CurrentGrid[j,i] != null)
                {
                    row += "o";
                }
                else
                {
                    row += " ";
                }
            }
            row += "\n";
        }

        Debug.Log(row);
    }

    public void Call()
    {
        if(!GridSet)
        {
            SetInitialGrid();
        }
        Vector2 position2D = ConvertCoordinatesTo2D(transform.position);
        position2D = new Vector2(Mathf.Round(position2D.x), Mathf.Round(position2D.y));
        Vector2 soulmatePosition2D = ConvertCoordinatesTo2D(Soulmate.transform.position);
        soulmatePosition2D = new Vector2(Mathf.Round(soulmatePosition2D.x), Mathf.Round(soulmatePosition2D.y));

        bool match = false;

        
        if(position2D.x != soulmatePosition2D.x && position2D.y != soulmatePosition2D.y)
        {

        }
        else
        {
            bool xMatch = true;
            if (position2D.x != soulmatePosition2D.x)
            {
                //check on X axis                
                for (int i = (int)Mathf.Min(position2D.x, soulmatePosition2D.x) + 1; i < (int)Mathf.Max(position2D.x, soulmatePosition2D.x); i++)
                {
                    if (CurrentGrid[i, (int)position2D.y] != null)
                    {
                        xMatch = false;
                        break;
                    }
                }
            }
            else
            {
                xMatch = false;
            }

            bool yMatch = true;
            if (position2D.y != soulmatePosition2D.y)
            {                
                //check on Y axis
                for (int i = (int)Mathf.Min(position2D.y, soulmatePosition2D.y) + 1; i < (int)Mathf.Max(position2D.y, soulmatePosition2D.y); i++)
                {
                    if (CurrentGrid[(int)position2D.x, i] != null)
                    {
                        yMatch = false;
                        break;
                    }
                }
            }
            else
            {
                yMatch = false;
            }
            match = xMatch || yMatch;
        }

        if(match)
        {
            Debug.Log("YEY");
        }
        else
        {
            Debug.Log("SAD");
        }
    }

    private void CallMate()
    {
        Soulmate.GetComponent<SoulBehaviour>().Call();
    }
}
