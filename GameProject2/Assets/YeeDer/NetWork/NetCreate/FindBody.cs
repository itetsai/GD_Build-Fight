using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FindBody : NetworkBehaviour
{
    [SyncVar]
    public int connectID;
    [SyncVar(hook ="ChangeLocalPosition")]
    public Vector3 localposition;
    [SyncVar]
    public Vector3 localeulerAngle;
    // Use this for initialization
    void Start()
    {
        if (isServer)
            GetComponent<NetworkIdentity>().AssignClientAuthority(NetworkServer.connections[connectID]);

        GameObject[] robotlist = GameObject.FindGameObjectsWithTag("Robot");

        for (int i = 0; i < robotlist.Length; i++)
                if (robotlist[i].GetComponent<RobotProperty>().connectID == connectID)
                {
                transform.parent = robotlist[i].transform.Find("NetBody");
                transform.localPosition = localposition;
                    transform.localEulerAngles = localeulerAngle;

                    GameObject midpoint = transform.parent.parent.Find("MiddlePoint").gameObject;
                    midpoint.GetComponent<SetMidPoint>().Add(transform.position);
                    break;
                }
            transform.parent.GetComponent<NetobjMainBodyStateControl>().StartConnectSurroundingState();//建立連結
    }

    void ChangeLocalPosition(Vector3 position)
    {
        transform.localPosition = localposition;
    }
}
