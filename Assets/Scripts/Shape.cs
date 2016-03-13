using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shape : MonoBehaviour {

    [SerializeField]
    private GameObject PinPrefab;

    public enum ShapeTypeEnum
    {
        Square
    }

    [SerializeField]
    private ShapeTypeEnum ShapeType;

    public List<Vector2> Vertices
    {
        get
        {
            return GetVertices();
        }
    }

    public Vector2 Pos2D { get; set; }

	// Use this for initialization
	void Start () {
        Pos2D = MathHelpers.ToVector2(transform.position);
	}
	
	// Update is called once per frame
	void Update () {
	
	}    

    private List<Vector2> GetVertices()
    {
        List<Vector2> vertices = new List<Vector2>();

        switch (ShapeType)
        {
            case ShapeTypeEnum.Square:
                vertices.Add(new Vector2(-0.5f, -0.5f) + Pos2D);
                vertices.Add(new Vector2(-0.5f, 0.5f) + Pos2D);
                vertices.Add(new Vector2(0.5f, 0.5f) + Pos2D);
                vertices.Add(new Vector2(0.5f, -0.5f) + Pos2D);
                break;
        }

        return vertices;
    }
}
