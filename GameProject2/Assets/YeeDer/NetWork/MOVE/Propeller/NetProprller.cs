using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetProprller : NetworkBehaviour {
    [SyncVar]
    public int connectID;
    [SyncVar]
    public Vector3 localposition;
    [SyncVar]
    public Vector3 localeulerAngle;

    public GameObject collider;
    // Use this for initialization
    void Start()
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

              /*  GameObject c = Instantiate(collider);
                c.transform.position = transform.position;
                c.transform.parent = transform.parent;*/

                transform.parent.GetComponent<NetHelicopterController>().MainRotorController = GetComponent<HeliRotorController>();
                transform.parent.GetComponent<Rigidbody>().drag = 1;//螺旋槳特有
                break;
            }
    }
}
