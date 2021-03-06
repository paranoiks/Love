﻿using UnityEngine;
using System.Collections;

public enum CameraPosition
{
    CameraFront,
    CameraRight,
    CameraTop,
    CameraZoomedOut
}

public enum CameraAction
{
    ZoomOut,
    ZoomInTop,
    ZoomInFront,
    ZoomInRight
}

public class CameraController : MonoBehaviour {

    [SerializeField]
    Transform CameraFrontTransform;

    [SerializeField]
    Transform CameraRightTransform;

    [SerializeField]
    Transform CameraTopTransform;

    [SerializeField]
    Transform CameraZoomedOutTransform;

    [SerializeField]
    private GameObject CamerasParent;
    
    [SerializeField]
    private GameObject MainCamera;

    private CameraPosition CurrentCameraPosition;

    public CameraPosition CurrentCameraPositionP
    {
        get
        {
            return CurrentCameraPosition;
        }
    }

    private GameObject[,] CurrentGrid;

    private bool CameraMoving = false;

	// Use this for initialization
	void Start () {       
        //CamerasParent.transform.position = new Vector3(Globals.WorldSize / 2, Globals.WorldSize / 2, Globals.WorldSize / 2);
        Vector3 cameraPositionOffset = new Vector3(Globals.WorldSize / 2, Globals.WorldSize / 2, Globals.WorldSize / 2);
        CameraTopTransform.transform.position += cameraPositionOffset;
        CameraRightTransform.transform.position += cameraPositionOffset;
        CameraFrontTransform.transform.position += cameraPositionOffset;
        CameraZoomedOutTransform.transform.position += cameraPositionOffset;
        CurrentCameraPosition = CameraPosition.CameraFront;
        MainCamera.transform.localPosition = CameraFrontTransform.localPosition;        
        CurrentGrid = new GameObject[Globals.WorldSize, Globals.WorldSize];
	}

    public void TakeCameraAction(CameraAction action)
    {
        if(CameraMoving)
        {
            return;
        }

        switch(action)
        {
            case CameraAction.ZoomInFront:
                if(CurrentCameraPosition == CameraPosition.CameraZoomedOut)
                {
                    CurrentCameraPosition = CameraPosition.CameraFront;
                    CalculateNewGrid();
                    UpdateSouls();
                    MainCamera.GetComponent<PerspectiveSwitcher>().BlendToMatrix(true);
                    StartCoroutine(MoveCamera());
                }
                break;
            case CameraAction.ZoomInRight:
                if (CurrentCameraPosition == CameraPosition.CameraZoomedOut)
                {
                    CurrentCameraPosition = CameraPosition.CameraRight;
                    CalculateNewGrid();
                    UpdateSouls();
                    MainCamera.GetComponent<PerspectiveSwitcher>().BlendToMatrix(true);
                    StartCoroutine(MoveCamera());
                }
                break;
            case CameraAction.ZoomInTop:
                if (CurrentCameraPosition == CameraPosition.CameraZoomedOut)
                {
                    CurrentCameraPosition = CameraPosition.CameraTop;
                    CalculateNewGrid();
                    UpdateSouls();
                    MainCamera.GetComponent<PerspectiveSwitcher>().BlendToMatrix(true);
                    StartCoroutine(MoveCamera());
                }
                break;
            case CameraAction.ZoomOut:
                if(CurrentCameraPosition != CameraPosition.CameraZoomedOut)
                {
                    CurrentCameraPosition = CameraPosition.CameraZoomedOut;
                    MainCamera.GetComponent<PerspectiveSwitcher>().BlendToMatrix(false);
                    StartCoroutine(MoveCamera());
                }
                break;
        }
    }
    
    /// <summary>
    /// Coroutine to smoothly change the camera position and rotation
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveCamera()
    {
        CameraMoving = true;
        float frames = 60f;
        Transform targetTransform = null;
        //find the target transform depending on the new CameraPosition
        switch (CurrentCameraPosition)
        {
            case CameraPosition.CameraFront:
                targetTransform = CameraFrontTransform;
                break;
            case CameraPosition.CameraRight:
                targetTransform = CameraRightTransform;
                break;
            case CameraPosition.CameraTop:
                targetTransform = CameraTopTransform;
                break;
            case CameraPosition.CameraZoomedOut:
                targetTransform = CameraZoomedOutTransform;
                break;
        }

        Vector3 startingPosition = MainCamera.transform.localPosition;

        Vector3 targetRotation = targetTransform.rotation.eulerAngles;
        
        float degreesPerFrame = Quaternion.Angle(MainCamera.transform.localRotation, targetTransform.localRotation) / frames;

        for (int i = 0; i < frames; i++)
        {
            float ratio = ((float)i) / frames;        
            Vector3 position = startingPosition + (targetTransform.localPosition - startingPosition) * ratio;

            //rotate the camera towards the desired rotation
            Quaternion newRotation = Quaternion.RotateTowards(MainCamera.transform.localRotation, targetTransform.localRotation, degreesPerFrame);

            MainCamera.transform.localPosition = position;
            MainCamera.transform.localRotation = newRotation;

            yield return null;
        }

        //set the position and rotation to the final ones to remove any errors and offsets
        MainCamera.transform.localPosition = targetTransform.localPosition;
        MainCamera.transform.localRotation = targetTransform.localRotation;

        CameraMoving = false;
    }

    /// <summary>
    /// Calculate the new 2D grid, based on the new perspective
    /// </summary>
    private void CalculateNewGrid()
    {
        for(int i=0;i<Globals.WorldSize;i++)
        {
            for(int j=0;j<Globals.WorldSize;j++)
            {
                CurrentGrid[i, j] = null;
            }
        }
        //find the two souls first
        var souls = GameObject.FindGameObjectsWithTag("Soul");
        foreach(var soul in souls)
        {
            Vector2 newPosition = ConvertCoordinatesTo2D(soul.transform.position);
            CurrentGrid[(int)newPosition.x, (int)newPosition.y] = soul;
        }

        var blocks = GameObject.FindGameObjectsWithTag("Block");
        foreach(var block in blocks)
        {
            Vector2 newPosition = ConvertCoordinatesTo2D(block.transform.position);
            block.GetComponent<Shape>().Pos2D = newPosition;
            CurrentGrid[(int)newPosition.x, (int)newPosition.y] = block;
        }
    }

    /// <summary>
    /// Send the new grid and CameraPosition to the two souls
    /// </summary>
    private void UpdateSouls()
    {
        var souls = GameObject.FindGameObjectsWithTag("Soul");
        foreach(var soul in souls)
        {
            soul.GetComponent<SoulBehaviour>().CurrentCameraPosition = CurrentCameraPosition;
            soul.GetComponent<SoulBehaviour>().CurrentGridP = CurrentGrid;
        }
    }

    /// <summary>
    /// Simple function to get 2D coordinates from 3D vectors, based on the current point of view
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private Vector2 ConvertCoordinatesTo2D(Vector3 position)
    {
        Vector2 newPosition = Vector2.zero;
        switch(CurrentCameraPosition)
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
	
	// Update is called once per frame
	void Update () {
	}
}
