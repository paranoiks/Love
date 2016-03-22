using UnityEngine;
using System.Collections;

public class LevelEditor : MonoBehaviour {

    private UIController UIControllerScript;

    [SerializeField]
    private GameObject BlockPrefab;

    private float LevelDepth;

    public float LevelDepthP
    {
        get
        {
            return LevelDepth;
        }
        set
        {
            LevelDepth = Mathf.Clamp(value, 0, Globals.WorldSize);
            UIControllerScript.NewLeveDepth(LevelDepth);
        }
    }

	// Use this for initialization
	void Start () {
        LevelDepth = 1;
        UIControllerScript = FindObjectOfType<UIController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MouseClicked(Vector3 clickPosition)
    {
        Debug.Log(Camera.main.ScreenToViewportPoint(clickPosition));

        Ray ray = Camera.main.ScreenPointToRay(clickPosition);
        int layerMask = 1 << LayerMask.NameToLayer("LevelEditorBlocks");
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 10000, layerMask))
        {
            Vector3 position = hitInfo.collider.transform.position;
            position.y = LevelDepth;
            Instantiate(BlockPrefab, position, Quaternion.identity);
        }
    }    
}
