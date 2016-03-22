using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

    private LevelEditor LevelEditorScript;
    private CameraController CameraControllerScript;
    private UIController UIControllerScript;

    private bool EditMode = false;

	// Use this for initialization
	void Start () {
        LevelEditorScript = GetComponent<LevelEditor>();
        CameraControllerScript = GetComponent<CameraController>();
        UIControllerScript = GetComponent<UIController>();
	}

    private void HandleMouseInput()
    {
        if(Input.GetMouseButtonDown(0))
        {            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << LayerMask.NameToLayer("Soul");
            RaycastHit hitInfo;
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
            HandleMouseInput();
            HandleKeyboardInput();
        }
	}
}
