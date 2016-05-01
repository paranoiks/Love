using UnityEngine;

public class SoulBehaviour : MonoBehaviour {

    [SerializeField]
    public GameObject Soulmate;

    [SerializeField]
    private GameObject ProjectilePrefab;

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

    // Update is called once per frame
    void Update () {
	   
	}

    public void SpawnProjectile()
    {
        if(!GridSet)
        {
            SetInitialGrid();
        }

        Vector3 thisSoulPosition = MathHelpers.GetWorldCoordinatesFlatened(transform.position, CurrentCameraPosition);
        Vector3 soulmatePosition = MathHelpers.GetWorldCoordinatesFlatened(Soulmate.transform.position, CurrentCameraPosition);

        GameObject projectile = Instantiate(ProjectilePrefab, thisSoulPosition, Quaternion.identity) as GameObject;
        projectile.GetComponent<Projectile>().StartingSoulPositionP = thisSoulPosition;
        projectile.GetComponent<Projectile>().TargetSoulPositionP = soulmatePosition;
    }    


}
