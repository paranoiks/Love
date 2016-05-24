using System.Collections;
using UnityEngine;

public class SoulBehaviour : MonoBehaviour {

    [SerializeField]
    public GameObject Soulmate;

    [SerializeField]
    private GameObject ProjectilePrefab;

    [SerializeField]
    private float MovingSpeed;

    [SerializeField]
    private float MovementTreshold;

    private bool GridSet = false;
    public CameraPosition CurrentCameraPosition { get; set; }
    private GameObject[,] CurrentGrid;
    public GameObject[,] CurrentGridP { get { return CurrentGrid; } set { GridSet = true; CurrentGrid = value; } }
    private GridMember[,] CurrentGridBFS;

    private float LocalX;
    private float LocalY;

	// Use this for initialization
	void Start () {
        CurrentCameraPosition = CameraPosition.CameraFront;
	}

    private void SetInitialGrid()
    {
        GridSet = true;
        CurrentGrid = new GameObject[Globals.WorldSize, Globals.WorldSize];

        var blocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (var block in blocks)
        {
            Vector2 newPosition = MathHelpers.ConvertCoordinatesTo2D(block.transform.position, CurrentCameraPosition);
            CurrentGrid[(int)newPosition.x, (int)newPosition.y] = block;
        }
    }

    public void Clicked()
    {
        SpawnProjectile();
    }    

    public void ProjectileReachedOtherSoul()
    {
        Debug.Log("PROJECTILE REACHED OTHER SOUL");
        Vector3 thisSoulPosition = MathHelpers.GetWorldCoordinatesFlatened(transform.position, CurrentCameraPosition);
        Vector3 soulmatePosition = MathHelpers.GetWorldCoordinatesFlatened(Soulmate.transform.position, CurrentCameraPosition);

        Vector3 meetingPoint = thisSoulPosition + (soulmatePosition - thisSoulPosition) / 2;

        StartMovingTowardsMeetingPoint(meetingPoint);
        Soulmate.GetComponent<SoulBehaviour>().StartMovingTowardsMeetingPoint(meetingPoint);
    }

    private IEnumerator MoveTowardsMeetingPoint(Vector3 meetingPoint)
    {
        while (true)
        {
            Vector3 direction = meetingPoint - transform.position;
            direction.Normalize();
            transform.position += direction * MovingSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, meetingPoint) < MovementTreshold)
            {
                Debug.Log("GG LEVEL SOLVED");
                break;
            }

            yield return null;
        }
    }

    public void StartMovingTowardsMeetingPoint(Vector3 meetingPoint)
    {
        StartCoroutine(MoveTowardsMeetingPoint(meetingPoint));
    }

    public void SpawnProjectile()
    {
        if(!GridSet)
        {
            SetInitialGrid();
        }

        Vector3 thisSoulPosition = MathHelpers.GetWorldCoordinatesFlatened(transform.position, CurrentCameraPosition);
        Vector3 soulmatePosition = MathHelpers.GetWorldCoordinatesFlatened(Soulmate.transform.position, CurrentCameraPosition);

        Projectile projectile = (Instantiate(ProjectilePrefab, thisSoulPosition, Quaternion.identity) as GameObject).GetComponent<Projectile>();
        projectile.SoulParent = this;
        projectile.StartingSoulPositionP = thisSoulPosition;
        projectile.TargetSoulPositionP = soulmatePosition;
    }    


}
