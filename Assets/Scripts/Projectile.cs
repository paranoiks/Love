using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    [SerializeField]
    private float Speed;

    private float GoalDistanceTreshold = 0.1f;

    private bool ReadyToMove = false;

    private Vector3 StartingSoulPosition;
    private Vector3 TargetSoulPosition;

    public SoulBehaviour SoulParent { get; set; }

    private CameraPosition CurrentCameraPosition;

    public Vector3 StartingSoulPositionP
    {
        get
        {
            return StartingSoulPosition;
        }
        set
        {
            StartingSoulPosition = value;
            ReadyToMove = true;
        }
    }

    public Vector3 TargetSoulPositionP
    {
        get
        {
            return TargetSoulPosition;
        }
        set
        {
            TargetSoulPosition = value;
            ReadyToMove = true;
        }
    }

	// Use this for initialization
	void Start () {        
        
	}

    private void Move()
    {
        if (!ReadyToMove)
        {
            return;
        }
        Vector3 direction = TargetSoulPosition - StartingSoulPosition;
        direction.Normalize();

        transform.position += Speed * direction * Time.deltaTime;

        float distanceToGoal = (TargetSoulPosition - transform.position).magnitude;
        if(distanceToGoal < GoalDistanceTreshold)
        {
            transform.position = TargetSoulPosition;
            GoalReached();
        }
    }

    private void HandleRaycast()
    { 
        if(!ReadyToMove)
        {
        return;
        }

        Ray ray = new Ray(transform.position, MathHelpers.GetRaycastDirectionFromCameraPosition(CurrentCameraPosition));
        RaycastHit hitInfo;
        int layerMask = 1 << LayerMask.NameToLayer("LevelBlocks");
        if(Physics.SphereCast(ray, transform.localScale.x, out hitInfo, 500, layerMask))
        {
            Destroy(gameObject);
        }
    }

    private void GoalReached()
    {
        SoulParent.ProjectileReachedOtherSoul();
        Destroy(gameObject);
    }
    
	// Update is called once per frame
	void Update () {
        HandleRaycast();
        Move();
	}
}
