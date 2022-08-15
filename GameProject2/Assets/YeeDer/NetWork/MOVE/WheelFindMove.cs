using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WheelFindMove : NetworkBehaviour
{
    [SyncVar]
    public int connectID;
    [SyncVar]
    public Vector3 localposition;
    [SyncVar]
    public Vector3 localeulerAngle;

    void Start()
    {

        if (isServer)
            GetComponent<NetworkIdentity>().AssignClientAuthority(NetworkServer.connections[connectID]);

        GameObject[] bodylist = GameObject.FindGameObjectsWithTag("body");
        for (int i = 0; i < bodylist.Length; i++)
            if (bodylist[i].GetComponent<BodyInPlayer>().connectID == connectID)
            {
                transform.parent = bodylist[i].transform;
                transform.localPosition = localposition;
                transform.localEulerAngles = localeulerAngle;

                GameObject midpoint = transform.parent.parent.Find("MiddlePoint").gameObject;
                midpoint.GetComponent<SetMidPoint>().Add(transform.position);
                break;
            }

            GameObject[] movelist = GameObject.FindGameObjectsWithTag("move");
            for (int i = 0; i < movelist.Length; i++)
                if (movelist[i].GetComponent<MoveInPlayer>().connectID == connectID)
                {
                    transform.parent = movelist[i].transform;
                    //transform.localPosition = localposition;//在body中建立好所以註解掉
                    transform.localEulerAngles = localeulerAngle;

                    transform.parent.GetComponent<NetFindAllWheel>().FindWheel(gameObject);
                    break;
                }

    }
}
