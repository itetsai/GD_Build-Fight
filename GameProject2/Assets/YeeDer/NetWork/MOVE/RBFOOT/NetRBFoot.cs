using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetRBFoot : NetworkBehaviour
{
    [SyncVar]
    public int connectID;
    [SyncVar]
    public Vector3 localposition;
    [SyncVar]
    public Vector3 localeulerAngle;
    // Use this for initialization
    void Start ()
    {
        if (isServer)
        {
            GetComponent<NetworkIdentity>().AssignClientAuthority(NetworkServer.connections[connectID]);
        }
        GameObject[] robotlist = GameObject.FindGameObjectsWithTag("Robot");

        for (int i = 0; i < robotlist.Length; i++)
            if (robotlist[i].GetComponent<RobotProperty>().connectID == connectID)
            {
                transform.parent = robotlist[i].transform.Find("NetMove");
                transform.localPosition = localposition;
                transform.localEulerAngles = localeulerAngle;
                break;
            }

        GameObject[] playerlist = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < playerlist.Length; i++)
            if (playerlist[i].GetComponent<NetPlayerController>().connectID == connectID)
            {
                GetComponent<NetTRF>().Mycamera = playerlist[i].transform.Find("Pivot").Find("Main Camera").GetComponent<Camera>();
            }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
