using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;

public class RobotProperty : NetworkBehaviour {
    [SyncVar]
    public int connectID;

	void Start ()
    {
        if (isServer)
            GetComponent<NetworkIdentity>().AssignClientAuthority(NetworkServer.connections[connectID]);

        GameObject[] playerlist = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < playerlist.Length; i++)
            if (playerlist[i].GetComponent<NetPlayerController>().connectID == connectID)
            {
                playerlist[i].GetComponent<FreeLookCam>().SetTarget(transform.Find("MiddlePoint"));
            }
    }

    public void CheckPosition()
    {
        Vector3 position = new Vector3();
        Vector3 prePosition = new Vector3();
        List<GameObject> childlist = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
            childlist.Add(transform.GetChild(i).gameObject);

        for (int i = 0; i < childlist.Count; i++)
        {
            position += childlist[i].transform.position;
        }
        position /= childlist.Count;
        prePosition = position;
        position -= this.gameObject.transform.position;
        for (int i = 0; i < childlist.Count; i++)
        {
            childlist[i].transform.position -= position;
        }
        gameObject.transform.position = prePosition;
    }
}
