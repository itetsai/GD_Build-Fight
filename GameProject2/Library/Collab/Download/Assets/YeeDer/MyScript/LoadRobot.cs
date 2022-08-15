using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadRobot : MonoBehaviour
{
    // Use this for initialization
    public GameObject Cube;
    public Vector3 CreatePosition;
	void Start ()
    {
		
	}

    // Update is called once per frame
    void Update()
    { 

	}

    public void Load()
    {
        GameObject complete = new GameObject();
        complete.AddComponent<Rigidbody>();
        complete.transform.position = CreatePosition;
        complete.name = "Combine";

        PlayerSaveData load = SaveCube.LoadData();
        for (int i = 0; i < load.cube_list.Count; i++)
            for (int j = 0; j < load.cube_list[i].Count; j++)
                for (int k = 0; k < load.cube_list[i][j].Count; k++)
                {
                    if (load.cube_list[i][j][k].exist == true)
                    {
                        Cube.transform.position = new Vector3(i, j, k) + CreatePosition;
                        GameObject c = Instantiate(Cube);
                        c.transform.parent = complete.transform;
                    }
                }

    }
}
