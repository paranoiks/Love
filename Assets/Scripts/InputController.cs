using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputController : MonoBehaviour {

    private LevelEditor LevelEditorScript;
    private CameraController CameraControllerScript;
    private UIController UIControllerScript;

    private bool EditMode = false;

    [SerializeField]
    private GameObject DebugBlock;

	// Use this for initialization
	void Start () {
        LevelEditorScript = GetComponent<LevelEditor>();
        CameraControllerScript = GetComponent<CameraController>();
        UIControllerScript = GetComponent<UIController>();
	}

    private void HandlePinch()
    {
        if (Input.touchCount >= 2
            && Input.touches[0].phase == TouchPhase.Moved
            && Input.touches[1].phase == TouchPhase.Moved)
        {
            Vector3 currentDistance = Input.touches[0].position - Input.touches[1].position;
            Vector3 previousDistance = (Input.touches[0].position - Input.touches[0].deltaPosition) - (Input.touches[1].position - Input.touches[1].deltaPosition);
            if (currentDistance.magnitude < previousDistance.magnitude)
            {
                CameraControllerScript.TakeCameraAction(CameraAction.ZoomOut);
            }
        }
    }

    private void HandleTouch()
    {
        if (Input.touchCount == 1
            && Input.touches[0].phase == TouchPhase.Began)
        {
            if(CameraControllerScript.CurrentCameraPositionP == CameraPosition.CameraZoomedOut)
            {
                
                Ray touchRay = Camera.main.ScreenPointToRay(Input.touches[0].position);

                UIControllerScript.EditModeAsDebug(Input.touches[0].position.ToString() + " " + Camera.main.ScreenToWorldPoint(Input.touches[0].position).ToString());
                                
                RaycastHit hitInfo;
                int layerMask = 1 << LayerMask.NameToLayer("SidePlanes");
                if(Physics.Raycast(touchRay, out hitInfo, 500, layerMask))
                {
                    SidePlaneSide currentSide = hitInfo.collider.gameObject.GetComponent<SidePlane>().Side;
                    switch(currentSide)
                    {
                        case SidePlaneSide.Top:
                            CameraControllerScript.TakeCameraAction(CameraAction.ZoomInTop);                            
                            break;
                        case SidePlaneSide.Right:
                            CameraControllerScript.TakeCameraAction(CameraAction.ZoomInRight);
                            break;
                        case SidePlaneSide.Front:
                            CameraControllerScript.TakeCameraAction(CameraAction.ZoomInFront);
                            break;
                    }
                }
            }
            else
            {

            }
        }
    }

    private void HandleTouchInput()
    {
        HandlePinch();
        HandleTouch();
    }

    private void HandleMouseInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(CameraControllerScript.CurrentCameraPositionP == CameraPosition.CameraZoomedOut)
            {
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.Log(Camera.main.projectionMatrix);
                Instantiate(DebugBlock, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
                RaycastHit mouseHitInfo;
                int mouseLayerMask = 1 << LayerMask.NameToLayer("SidePlanes");
                if (Physics.Raycast(mouseRay, out mouseHitInfo, 500, mouseLayerMask))
                {
                    SidePlaneSide currentSide = mouseHitInfo.collider.gameObject.GetComponent<SidePlane>().Side;
                    switch (currentSide)
                    {
                        case SidePlaneSide.Top:
                            CameraControllerScript.TakeCameraAction(CameraAction.ZoomInTop);
                            break;
                        case SidePlaneSide.Right:
                            CameraControllerScript.TakeCameraAction(CameraAction.ZoomInRight);
                            break;
                        case SidePlaneSide.Front:
                            CameraControllerScript.TakeCameraAction(CameraAction.ZoomInFront);
                            break;
                    }
                }
            }   
            else
            {

            }         
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            int layerMask = 1 << LayerMask.NameToLayer("Soul");
            if (Physics.Raycast(ray, out hitInfo, 10000, layerMask))
            {
                hitInfo.collider.gameObject.GetComponent<SoulBehaviour>().Clicked();
            }
        }
    }

    private void HandleKeyboardInput()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            CameraControllerScript.TakeCameraAction(CameraAction.ZoomOut);
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            CameraControllerScript.TakeCameraAction(CameraAction.ZoomInTop);
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            CameraControllerScript.TakeCameraAction(CameraAction.ZoomInRight);
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            CameraControllerScript.TakeCameraAction(CameraAction.ZoomInFront);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            EditMode = true;
            UIControllerScript.EditMode(true);
        }
    }

    private void HandleMouseInputEdit()
    {
        if(Input.GetMouseButtonDown(0))
        {
            LevelEditorScript.MouseClicked(Input.mousePosition);
        }
    }

    private void HandleKeyboardInputEdit()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            EditMode = false;
            UIControllerScript.EditMode(false);
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            LevelEditorScript.LevelDepthP += 1;
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            LevelEditorScript.LevelDepthP -= 1;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (EditMode)
        {
            HandleMouseInputEdit();
            HandleKeyboardInputEdit();
        }
        else
        {
            HandleTouchInput();
            HandleMouseInput();
            HandleKeyboardInput();
        }
	}
}
