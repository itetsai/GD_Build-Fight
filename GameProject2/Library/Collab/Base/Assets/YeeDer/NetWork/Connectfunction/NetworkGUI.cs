using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkGUI : MonoBehaviour
{
    bool isHaveNetworkRole = false;


    public GameObject Player;
    public GameObject gun;
    public InputField msg;
    void Start()
    {
        GetComponent<NetworkManager>().spawnPrefabs.AddRange(GetComponent<SpawnPrefabList>().RegisteObject);//註冊物件
        isHaveNetworkRole = false;
    }

    private void OnDisconnected(NetworkMessage msg)
    {
        isHaveNetworkRole = false;
        Application.LoadLevel(Application.loadedLevel);
    }


    void OnGUI()
    {
        if (isHaveNetworkRole)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 80, Screen.height / 2 - 12, 160, 24), "Stop"))
            {
                NetworkManager2.singleton.StopServer();
                NetworkManager2.singleton.StopClient();
                OnDisconnected(null);
            }

            if (GUI.Button(new Rect(Screen.width / 2f - 80, Screen.height / 2 + 60, 160, 24), "Send"))
            {
                GameObject[] playerlist = GameObject.FindGameObjectsWithTag("Player");
                int connectID = 0;
                uint playerNetworkID = 0;

                for (int i = 0; i < playerlist.Length; i++)
                {
                    if (playerlist[i].GetComponent<NetPlayerController>().local == true)
                    {
                        connectID = playerlist[i].GetComponent<NetPlayerController>().connectID;
                        playerNetworkID = playerlist[i].GetComponent<NetworkIdentity>().netId.Value;
                        break;
                    }
                }


                var client = NetworkManager2.singleton.client;

                RobotMessage RM = new RobotMessage();//改成Robot+body+move
                RM.connectID = connectID;
                RM.PlayerNetworkID = playerNetworkID;
                client.Send(RegisteFunction.ROBOT, RM);


                /*BodyMessage BM = new BodyMessage();//身體(包括槍)
                BM.connectID = connectID;
                BM.PlayerNetworkID = playerNetworkID;
                client.Send(RegisteFunction.BODY, BM);

                MoveMessage MM = new MoveMessage();//載具母體
                MM.connectID = connectID;
                MM.PlayerNetworkID = playerNetworkID;
                client.Send(RegisteFunction.MOVE, MM);*/

                int sendtime = 0;
                RobotBodyData RBD = SaveLoadRobot.Load("Save1");
                for (int i = 0; i < RBD.cube_list.Count; i++)
                {
                    BodyCubeMessage BCM = new BodyCubeMessage();
                    BCM.connectID = connectID;
                    BCM.PlayerNetworkID = playerNetworkID;
                    BCM.name = RBD.cube_list[i].name;
                    BCM.position = RBD.cube_list[i].position;
                    BCM.hp = RBD.cube_list[i].hp;
                    BCM.eulerAngle = RBD.cube_list[i].eulerAngle;
                    client.Send(RegisteFunction.BODYCUBE, BCM);
                    sendtime++;
                }
                print(sendtime);//除錯用

            }
            return;
        }

        if (GUI.Button(new Rect(Screen.width / 2f - 80, Screen.height / 2 - 12, 160, 24), "Start Server"))
        {
            isHaveNetworkRole = NetworkManager2.singleton.StartServer();
            GetComponent<RegisteFunction>().ServeRegite();
        }
        
        if (GUI.Button(new Rect(Screen.width / 2f - 80, Screen.height / 2 + 24, 160, 24), "Start Client"))
        {
            var client = NetworkManager2.singleton.StartClient();
            client.RegisterHandler(MsgType.Disconnect, OnDisconnected);
            isHaveNetworkRole = true;
        }


    }
}
