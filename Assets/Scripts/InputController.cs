using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
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
	
	// Update is called once per frame
	void Update () {
        HandleMouseInput();
	}
}
