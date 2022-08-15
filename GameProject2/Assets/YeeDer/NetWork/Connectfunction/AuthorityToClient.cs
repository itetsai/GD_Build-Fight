using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AuthorityToClient : NetworkBehaviour {

    public NetworkConnection conn;
	void Start ()//賦予Client此物件的控制權力
    {
        GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isServer)
        {
            if (NetworkServer.connections[1]!=null)
            {
                conn = NetworkServer.connections[1];
                GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
            }
        }
    }
}
