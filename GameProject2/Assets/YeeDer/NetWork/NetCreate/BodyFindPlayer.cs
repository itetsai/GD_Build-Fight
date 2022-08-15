using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class BodyFindPlayer : NetworkBehaviour {
    [SyncVar]
    public int connectID;

	void Start ()
    {
        if (isServer)
            GetComponent<NetworkIdentity>().AssignClientAuthority(NetworkServer.connections[connectID]);


        GameObject[]robotlist= GameObject.FindGameObjectsWithTag("Robot");
        for (int i = 0; i < robotlist.Length; i++)
            if (robotlist[i].GetComponent<RobotProperty>().connectID ==  connectID)
                transform.parent = robotlist[i].transform;
	}
}
