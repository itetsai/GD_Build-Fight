using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FindBody : NetworkBehaviour
{
    [SyncVar]
    public uint PlayerNetworkID;
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

        if (GetComponent<ObjectName>().kind == "move")//如果是載具的話則尋找move
        {
            if (GetComponent<ObjectName>().objectname == "wheel")
            {
                GameObject[] bodylist = GameObject.FindGameObjectsWithTag("move");
                for (int i = 0; i < bodylist.Length; i++)
                    if (bodylist[i].GetComponent<BodyFindPlayer>().PlayerNetworkID == PlayerNetworkID)
                    {
                        transform.parent = bodylist[i].transform;
                        transform.localPosition = localposition;
                        transform.localEulerAngles = localeulerAngle;

                       // transform.parent.GetComponent<NetFindAllWheel>().FindWheel();

                        GameObject midpoint = transform.parent.parent.Find("MiddlePoint").gameObject;
                        midpoint.GetComponent<SetMidPoint>().Add(transform.position);
                        break;
                    }

                print("wheel");
            }
        }
        else
        {
            GameObject[] bodylist = GameObject.FindGameObjectsWithTag("body");
            for (int i = 0; i < bodylist.Length; i++)
                if (bodylist[i].GetComponent<BodyFindPlayer>().PlayerNetworkID == PlayerNetworkID)
                {
                    transform.parent = bodylist[i].transform;
                    transform.localPosition = localposition;
                    transform.localEulerAngles = localeulerAngle;

                    GameObject midpoint = transform.parent.parent.Find("MiddlePoint").gameObject;
                    midpoint.GetComponent<SetMidPoint>().Add(transform.position);
                    break;
                }
            transform.parent.GetComponent<NetobjMainBodyStateControl>().StartConnectSurroundingState();//建立連結

        }



    }

    void ChangeLocalPosition(Vector3 position)
    {
        transform.localPosition = localposition;
    }
}
