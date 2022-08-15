using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class FindCube : NetworkBehaviour {

    [SyncVar]
    public int connectID;
    // Use this for initialization
    void Start ()
    {
        if (isServer)
            GetComponent<NetworkIdentity>().AssignClientAuthority(NetworkServer.connections[connectID]);

        Vector3 setPosition = new Vector3(0, 0, 0);
        GameObject[] cubelist = GameObject.FindGameObjectsWithTag("Cube");
        for (int i = 0; i < cubelist.Length; i++)
            if (cubelist[i].GetComponent<NetCubeProperty>().connectID == connectID)
            {
                setPosition += cubelist[i].transform.position;
            }
        setPosition = setPosition / cubelist.Length;

        transform.position = setPosition;//將位置設定到所有方塊中間

        for (int i = 0; i < cubelist.Length; i++)//將方塊設為子物件
            if (cubelist[i].GetComponent<NetCubeProperty>().connectID == connectID)
            {
                cubelist[i].transform.parent = transform;
            }
        GetComponent<NetobjMainBodyStateControl>().StartConnectSurroundingState();//建立方塊連結
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
