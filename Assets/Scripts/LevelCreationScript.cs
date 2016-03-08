using UnityEngine;
using System.Collections;

public class LevelCreationScript : MonoBehaviour {

    [SerializeField]
    private GameObject BlockPrefab;

    [SerializeField]
    private GameObject SoulPrefab;

    private int WorldSize;
    
    // Use this for initialization
    void Start()
    {
        WorldSize = Globals.WorldSize;
        for (int i = 0; i < WorldSize; i++)
        {
            for (int j = 0; j < WorldSize; j++)
            {
                for (int k = 0; k < WorldSize; k++)
                {
                    if(WtfCounter(i, j, k) >= 2)
                    {
                        Vector3 blockPosition = new Vector3(i, j, k);
                        Instantiate(BlockPrefab, blockPosition, Quaternion.identity);
                    }
                }
            }
        }

        CreateSoulmates();
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
