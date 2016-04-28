using UnityEngine;
using System.Collections;

public class LevelCreationScript : MonoBehaviour {

    [SerializeField]
    private GameObject BlockPrefab;

    [SerializeField]
    private GameObject LevelEditorInvisibleBlock;

    [SerializeField]
    private GameObject SoulPrefab;

    [SerializeField]
    private GameObject SidePlaneTop;

    [SerializeField]
    private GameObject SidePlaneRight;

    [SerializeField]
    private GameObject SidePlaneFront;

    private int WorldSize;
    
    // Use this for initialization
    void Start()
    {
        WorldSize = Globals.WorldSize;

        //WorldCube.transform.position = new Vector3(WorldSize / 2, WorldSize / 2, WorldSize / 2);
        //WorldCube.transform.localScale = new Vector3(WorldSize, WorldSize, WorldSize);
        CreateCubeFrame();
        CreateLevelEditorBlocks();
        SetUpSidePlanes();
        //CreateSoulmates();
    }

    private void CreateCubeFrame()
    {
        for (int i = 0; i < WorldSize; i++)
        {
            for (int j = 0; j < WorldSize; j++)
            {
                for (int k = 0; k < WorldSize; k++)
                {
                    if (WtfCounter(i, j, k) >= 2)
                    {
                        Vector3 blockPosition = new Vector3(i, j, k);
                        Instantiate(BlockPrefab, blockPosition, Quaternion.identity);
                    }
                }
            }
        }
    }

    private void CreateLevelEditorBlocks()
    {
        for(int i=0;i< WorldSize;i++)
        {
            for (int j = 0; j < WorldSize; j++)
            {
                float y = -1;
                Instantiate(LevelEditorInvisibleBlock, new Vector3(i, y, j), Quaternion.identity);
            }
        }
    }

    private void SetUpSidePlanes()
    {
        Vector3 sidePlanesScale = new Vector3((float)WorldSize / 10, (float)WorldSize / 10, (float)WorldSize / 10);

        SidePlaneTop.transform.localScale = sidePlanesScale;
        SidePlaneFront.transform.localScale = sidePlanesScale;
        SidePlaneRight.transform.localScale = sidePlanesScale;

        float offset = (float)WorldSize / 2;

        SidePlaneTop.transform.position = new Vector3(offset, 2 * offset, offset) - new Vector3(0.5f, 0, 0.5f);
        SidePlaneRight.transform.position = new Vector3(2 * offset, offset, offset) - new Vector3(0, 0.5f, 0.5f);
        SidePlaneFront.transform.position = new Vector3(offset, offset, 0) - new Vector3(0.5f, 0.5f, 1f);
    }

    private void CreateSoulmates()
    {
        GameObject s1 = Instantiate(SoulPrefab, new Vector3(2, 4, 3), Quaternion.identity) as GameObject;
        GameObject s2 = Instantiate(SoulPrefab, new Vector3(6, 4, 3), Quaternion.identity) as GameObject;
        s1.GetComponent<SoulBehaviour>().Soulmate = s2;
        s2.GetComponent<SoulBehaviour>().Soulmate = s1;

        GameObject block = Instantiate(BlockPrefab, new Vector3(4, 4, 2), Quaternion.identity) as GameObject;
        block.name = "ASDASDAS";
    }

    private int WtfCounter(int a,int b, int c)
    {
        int counter = 0;
        if(a==0||a==WorldSize -1)
        {
            counter++;
        }
        if (b == 0 || b == WorldSize - 1)
        {
            counter++;
        }
        if (c == 0 || c == WorldSize - 1)
        {
            counter++;
        }

        return counter;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
