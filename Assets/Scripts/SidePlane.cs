using UnityEngine;
using System.Collections;

public enum SidePlaneSide
{
    Top,
    Right,
    Front
}

public class SidePlane : MonoBehaviour {

    [SerializeField]
    public SidePlaneSide Side;

	// Use this for initialization
	void Start () {
	
	}
	
}
