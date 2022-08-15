using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetSkill : NetworkBehaviour {

    [SyncVar]
    public int connectID;

    // Use this for initialization
    void Start ()
    {
        GameObject[] robotlist = GameObject.FindGameObjectsWithTag("Robot");

        if (isServer)
        {
            GetComponent<NetworkIdentity>().AssignClientAuthority(NetworkServer.connections[connectID]);
        }

        for (int i = 0; i < robotlist.Length; i++)
            if (robotlist[i].GetComponent<RobotProperty>().connectID == connectID)
            {
                transform.parent = robotlist[i].transform;
            }
    }
}
