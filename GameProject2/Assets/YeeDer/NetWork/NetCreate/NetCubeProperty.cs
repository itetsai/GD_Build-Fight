using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class NetCubeProperty : NetworkBehaviour {

    [SyncVar]
    public int connectID;
    [SyncVar]
    public Vector3 localposition;

    void SetPosition()
    {
        transform.localPosition = localposition;
    }
}
