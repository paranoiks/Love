using UnityEngine;
using System.Collections;

public enum CameraPosition
{
    CameraFront,
    CameraRight,
    CameraTop,
    CameraZoomedOut
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

    private CameraPosition CurrentCameraPosition;

    private GameObject[,] CurrentGrid;

	// Use this for initialization
	void Start () {       
        CamerasParent.transform.position = new Vector3(Globals.WorldSize / 2, Globals.WorldSize / 2, Globals.WorldSize / 2);
        CurrentCameraPosition = CameraPosition.CameraFront;
        CurrentGrid = new GameObject[Globals.WorldSize, Globals.WorldSize];
	}

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W)
            && CurrentCameraPosition == CameraPosition.CameraZoomedOut)
        {            
            CurrentCameraPosition = CameraPosition.CameraTop;
            CalculateNewGrid();
            UpdateSouls();
            StartCoroutine(MoveCamera());
        }
        if (Input.GetKeyDown(KeyCode.S)
            && CurrentCameraPosition == CameraPosition.CameraZoomedOut)
        {
            CurrentCameraPosition = CameraPosition.CameraFront;
            CalculateNewGrid();
            UpdateSouls();
            StartCoroutine(MoveCamera());
        }
        if (Input.GetKeyDown(KeyCode.D)
            && CurrentCameraPosition == CameraPosition.CameraZoomedOut)
        {
            CurrentCameraPosition = CameraPosition.CameraRight;
            CalculateNewGrid();
            UpdateSouls();
            StartCoroutine(MoveCamera());
        }
        if (Input.GetKeyDown(KeyCode.Z)
            && CurrentCameraPosition != CameraPosition.CameraZoomedOut)
        {
            CurrentCameraPosition = CameraPosition.CameraZoomedOut;
            StartCoroutine(MoveCamera());
        }
    }

    private IEnumerator MoveCamera()
    {
        Vector3 startingPosition = Camera.main.transform.localPosition;
        Vector3 startingRotation = Camera.main.transform.localRotation.eulerAngles;
        
        float frames = 60f;
        Transform targetTransform = null;
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
        Vector3 targetRotation = targetTransform.rotation.eulerAngles;

        Debug.Log(startingPosition + " " + targetTransform.localPosition);

        for (int i = 0; i < frames; i++)
        {
            float ratio = ((float)i) / frames;            
            Vector3 position = startingPosition + (targetTransform.localPosition - startingPosition) * ratio;
            //Vector3 newEulerAngles = new Vector3(targetRotation.x - startingRotation.x, targetRotation.y - startingRotation.y, targetRotation.z - startingRotation.z) * ratio;

            Camera.main.transform.localPosition = position;                      
            //Camera.main.transform.localRotation = Quaternion.Euler(newEulerAngles);

            yield return null;
        }

        Camera.main.transform.localPosition = targetTransform.localPosition;

        StartCoroutine(RotateCamera());
    }

    private IEnumerator RotateCamera()
    {
        Vector3 startingPosition = Camera.main.transform.localPosition;
        Vector3 startingRotation = Camera.main.transform.localRotation.eulerAngles;

        float frames = 60f;
        Transform targetTransform = null;
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
        Vector3 targetRotation = targetTransform.localRotation.eulerAngles;

        //Debug.Log(startingPosition + " " + targetTransform.localPosition);

        for (int i = 0; i < frames; i++)
        {
            float ratio = ((float)i) / frames;
            //Vector3 position = startingPosition + (targetTransform.localPosition - startingPosition) * ratio;
            Vector3 newEulerAngles = new Vector3(targetRotation.x - startingRotation.x, targetRotation.y - startingRotation.y, targetRotation.z - startingRotation.z) * ratio;

            //Camera.main.transform.localPosition = position;
            Camera.main.transform.localRotation = Quaternion.Euler(newEulerAngles);

            yield return null;
        }

        Camera.main.transform.localRotation = targetTransform.localRotation;
    }

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
            CurrentGrid[(int)newPosition.x, (int)newPosition.y] = block;
        }
    }

    private void UpdateSouls()
    {
        var souls = GameObject.FindGameObjectsWithTag("Soul");
        foreach(var soul in souls)
        {
            soul.GetComponent<SoulBehaviour>().CurrentCameraPosition = CurrentCameraPosition;
            soul.GetComponent<SoulBehaviour>().CurrentGridP = CurrentGrid;
        }
    }

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
        HandleInput();
	}
}
