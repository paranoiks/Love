using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MatrixBlender))]
public class PerspectiveSwitcher : MonoBehaviour
{
    private Matrix4x4 ortho, perspective;

    [SerializeField]
    public float fov = 60f;
    [SerializeField]
    public float near = .3f;
    [SerializeField]
    public float far = 1000f;
    [SerializeField]
    public float orthographicSize = 50f;

    private float aspect;
    private MatrixBlender blender;
    private bool orthoOn;

    void Start()
    {
        aspect = (float)Screen.width / (float)Screen.height;
        ortho = Matrix4x4.Ortho(-orthographicSize * aspect, orthographicSize * aspect, -orthographicSize, orthographicSize, near, far);
        perspective = Matrix4x4.Perspective(fov, aspect, near, far);
        GetComponent<Camera>().projectionMatrix = ortho;
        orthoOn = true;
        blender = (MatrixBlender)GetComponent(typeof(MatrixBlender));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GetComponent<Camera>().enabled)
        {
            orthoOn = !orthoOn;
            if (orthoOn)
                blender.BlendToMatrix(ortho, 1f);
            else
                blender.BlendToMatrix(perspective, 1f);
        }
    }

    public void BlendToMatrix(bool toOrtho)
    {
        if(toOrtho)
        {
            blender.BlendToMatrix(ortho, 1f);
        }
        else
        {
            blender.BlendToMatrix(perspective, 1f);
        }
    }
}