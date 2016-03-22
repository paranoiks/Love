using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    [SerializeField]
    private Text EditModeOnOffText;

    [SerializeField]
    private Text LevelDepthText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void EditMode(bool on)
    {
        string editModeString = "Edit Mode ";
        editModeString += on ? "on" : "off";
        EditModeOnOffText.text = editModeString;
    }

    public void NewLeveDepth(float newValue)
    {
        LevelDepthText.text = newValue.ToString();
    }
}
