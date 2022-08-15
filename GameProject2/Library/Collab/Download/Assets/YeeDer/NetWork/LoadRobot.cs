using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoadRobot : MonoBehaviour
{
    // Use this for initialization

    public Vector3 CreatePosition;
	void Start ()
    {
        CreatePosition = new Vector3(0, 5, 0);
	}

    // Update is called once per frame
    void Update()
    { 

	}

    public void Load()
    {
  /*      GameObject complete = Instantiate(Resources.Load("Empty")) as GameObject;
        complete.AddComponent<Rigidbody>();
        complete.transform.position = CreatePosition;
        complete.name = "Combine";*/
        
        PlayerSaveData load = SaveCube.LoadData();

//        NetworkServer.Spawn(complete);

        for (int i = 0; i < load.cube_list.Count; i++)
            for (int j = 0; j < load.cube_list[i].Count; j++)
                for (int k = 0; k < load.cube_list[i][j].Count; k++)
                {
                    if (load.cube_list[i][j][k].exist == true)
                    {
                        //GameObject c = Instantiate(Resources.Load("NetCube")) as GameObject;
                        //c.transform.position = new Vector3(i, j, k) + CreatePosition;
                        Vector3 p = new Vector3();
                        p = new Vector3(i, j, k) + CreatePosition;
                        CmdCreate("NetCube", p);
//                        c.transform.parent = complete.transform;
                    }
                }
    }

    [Command]//function名稱前面一定要有Cmd否則無法bulid
    public void CmdCreate(string CreateName,Vector3 position)
    {
        GameObject c = Instantiate(Resources.Load(CreateName)) as GameObject;
        print("wtf");
        c.transform.position = position;
        NetworkServer.Spawn(c);
    }
}
