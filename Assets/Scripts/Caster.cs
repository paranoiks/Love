using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Caster : MonoBehaviour {

    [SerializeField]
    private GameObject PinPrefab;

    Shape[] Shapes;

    List<GameObject> Pins;

    private float CircleRadius = 0.1f;

    [SerializeField]
    private AnimationCurve ExpandAnimationCurve;

    [SerializeField]
    private float TimerTotal;

    private float TimerCurrent = 0;

    [SerializeField]
    private float CircleRadiusMax;    

	// Use this for initialization
	void Start () {
        Shapes = FindObjectsOfType<Shape>();
    }

    private void CreateCircleMesh()
    {
        //generate vertices
        List<Vector3> meshVertices = new List<Vector3>();
        Vector3 firstVertex = new Vector3(0, 0, -transform.position.z);
        meshVertices.Add(firstVertex);
        List<int> meshTriangles = new List<int>();
        Mesh mesh = new Mesh();
        for (int i = 0; i < 360; i+=3)
        {
            float iRad = i * Mathf.Deg2Rad;
            Vector3 currentVertexPosition = new Vector3(Mathf.Cos(iRad), Mathf.Sin(iRad), 0) * CircleRadius;

            //raycast to that position to check if there is something in the way
            Vector2 hitResultPoint;
            LineSegment2D hitResultLineSegment;
            if (RaycastAgainstAllShapes(currentVertexPosition + transform.position, out hitResultPoint, out hitResultLineSegment))
            {
                if (Vector3.Distance(transform.position, hitResultLineSegment.Point1V3) < CircleRadius)
                {
                    meshVertices.Add(hitResultLineSegment.Point1V3 - transform.position);
                }

                meshVertices.Add(MathHelpers.ToVector3(hitResultPoint) - transform.position);

                if (Vector3.Distance(transform.position, hitResultLineSegment.Point2V3) < CircleRadius)
                {
                    meshVertices.Add(hitResultLineSegment.Point2V3 - transform.position);
                }
            }
            else
            {
                currentVertexPosition.z = 0;
                meshVertices.Add(currentVertexPosition);
            }
        }
        
        for (int i = meshVertices.Count - 1; i >= 1; i--)
        {
            meshTriangles.Add(0);
            meshTriangles.Add(i);
            if (i == 1)
            {
                meshTriangles.Add(meshVertices.Count - 1);
            }
            else
            {
                meshTriangles.Add(i - 1);
            }
        }

        mesh.vertices = meshVertices.ToArray();
        mesh.triangles = meshTriangles.ToArray();
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private bool RaycastAgainstAllShapes(Vector3 target, out Vector2 resultPoint, out LineSegment2D resultLineSegment)
    {
        Vector2 target2D = new Vector2(target.x, target.y);
        Vector2 pos2D = new Vector2(transform.position.x, transform.position.y);

        List<Vector2> intersectionPoints = new List<Vector2>();
        List<LineSegment2D> intersectionLineSegments = new List<LineSegment2D>();
        foreach(var shape in Shapes)
        {
            List<Vector2> currentShapeVertices = shape.Vertices;
            for (int i = 0; i < currentShapeVertices.Count; i++)
            {
                Vector2 firstPoint = currentShapeVertices[i];
                Vector2 secondPoint = i == currentShapeVertices.Count - 1 ? currentShapeVertices[0] : currentShapeVertices[i + 1];
                Vector2 intersectionPoint = CheckIfSegmentsIntersect(pos2D, target2D, firstPoint, secondPoint);
                if(intersectionPoint != Vector2.zero)
                {                    
                    intersectionPoints.Add(intersectionPoint);
                    intersectionLineSegments.Add(new LineSegment2D(firstPoint, secondPoint));
                }
            }
        }
               
        if(intersectionPoints.Count > 0)
        {
            Vector2 closestIntersectionPoint = intersectionPoints[0];
            float minDistance = Vector2.Distance(closestIntersectionPoint, pos2D);
            int i = -1;            
            foreach(var ip in intersectionPoints)
            {
                if(Vector2.Distance(ip, pos2D) < minDistance)
                {
                    minDistance = Vector2.Distance(ip, pos2D);
                    closestIntersectionPoint = ip;
                }
                i++;
            }

            resultPoint = closestIntersectionPoint;
            resultLineSegment = intersectionLineSegments[i];
            return true;
        }
        else
        {
            resultPoint = Vector2.zero;
            resultLineSegment = null;
            return false;
        }
    }

    private Vector2 CheckIfSegmentsIntersect(Vector2 s1p1, Vector2 s1p2, Vector2 s2p1, Vector2 s2p2)
    {
        float s1x, s1y, s2x, s2y;
        s1x = s1p2.x - s1p1.x;
        s1y = s1p2.y - s1p1.y;
        s2x = s2p2.x - s2p1.x;
        s2y = s2p2.y - s2p1.y;

        float s, t;
        s = (-s1y * (s1p1.x - s2p1.x) + s1x * (s1p1.y - s2p1.y)) / (-s2x * s1y + s1x * s2y);
        t = (s2x * (s1p1.y - s2p1.y) - s2y * (s1p1.x - s2p1.x)) / (-s2x * s1y + s1x * s2y);

        if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
        {
            Vector2 intersectionPoint = new Vector2(s1p1.x + t * s1x, s1p1.y + t * s1y);
            return intersectionPoint;
        }

        return Vector2.zero;
    }   

    private IEnumerator ExpandCircle()
    {
        CircleRadius = TimerCurrent = 0;

        while(TimerCurrent < TimerTotal)
        {
            TimerCurrent += Time.deltaTime;
            float ratio = ExpandAnimationCurve.Evaluate(TimerCurrent / TimerTotal);
            CircleRadius = ratio * CircleRadiusMax;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void StartExpanding()
    {
        StartCoroutine(ExpandCircle());
    }

    private void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ExpandCircle());
        }
    }
	
	// Update is called once per frame
	void Update () {
        HandleInput();
        //HandleTimer();
    }
}
