using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Vehicles.Car;

public class NetWheel : NetworkBehaviour
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
    }
}
