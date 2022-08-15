using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class CreateMessage : MessageBase
{
    public int connectID;
    public uint networkID;
}
public class NetworkManager2 : NetworkManager
{
    public GameObject g;
    public GameObject empty;
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)//自訂義加入腳色
    {
        playerPrefab.GetComponent<NetPlayerController>().connectID = conn.connectionId;
        var player = (GameObject)GameObject.Instantiate(playerPrefab, new Vector3(0, 5, 0), Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        CreateMessage mb = new CreateMessage();
        mb.connectID = conn.connectionId;
        mb.networkID = player.GetComponent<NetworkIdentity>().netId.Value;
        NetworkServer.SendToClient(conn.connectionId, RegisteFunction.SENDROBOT,mb);
    }

    // Update is called once per frame
    void Update ()
    {

    }
}
