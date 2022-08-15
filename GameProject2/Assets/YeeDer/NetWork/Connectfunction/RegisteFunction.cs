using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RobotMessage : MessageBase
{
    public int connectID;
  //  public int moveCount;
}

public class BodyMessage : MessageBase
{
    public int connectID;
}

public class MoveMessage : MessageBase
{
    public int connectID;
}

public class BodyCubeMessage : MessageBase
{
    public int connectID;

    public int hp;//血量
    public string name;//方塊形狀
    public Vector3 eulerAngle;//方塊旋轉方向
    public Vector3 position;//方塊的位置
}

public class CubeConnectMessage : MessageBase
{
    public int connectID;
}
public class RegisteFunction : MonoBehaviour
{
    public static short ROBOT = MsgType.Highest + 2;
    public static short BODY = MsgType.Highest + 3;
    public static short BODYCUBE = MsgType.Highest + 4;
    public static short CUBECONNECT = MsgType.Highest + 5;
    public static short MOVE = MsgType.Highest + 6;
    //client用
    public static short SENDROBOT= MsgType.Highest + 7;

    public void ServerRegister()
    {
        if (NetworkServer.active)
        {
            NetworkServer.RegisterHandler(ROBOT, CreateRobot);
            NetworkServer.RegisterHandler(BODY, CreateBody);
            NetworkServer.RegisterHandler(BODYCUBE, CreateBodyCube);
            NetworkServer.RegisterHandler(CUBECONNECT, CubeConnect);
            NetworkServer.RegisterHandler(MOVE, CreateMove);
        }
    }

	public void ClientRegister(NetworkClient client)
    {
		//var client = NetworkManager2.singleton.client;
        client.RegisterHandler(SENDROBOT, SendRobotToServer);
    }


    void SendRobotToServer(NetworkMessage msg)//傳送機器人資訊
    {
      //  GameObject[] playerlist = GameObject.FindGameObjectsWithTag("Player");

        CreateMessage createMessage = msg.ReadMessage<CreateMessage>();

        int connectID = createMessage.connectID;

        var client = NetworkLobbyManager.singleton.client;

        RobotMessage RM = new RobotMessage();//改成Robot+body+move
        RM.connectID = connectID;
        client.Send(RegisteFunction.ROBOT, RM);

        int sendtime = 0;
        RobotBodyData RBD = SaveLoadRobot.Load("Save1");
        for (int i = 0; i < RBD.cube_list.Count; i++)
        {
            BodyCubeMessage BCM = new BodyCubeMessage();
            BCM.connectID = connectID;
            BCM.name = RBD.cube_list[i].name;
            BCM.position = RBD.cube_list[i].position;
            BCM.hp = RBD.cube_list[i].hp;
            BCM.eulerAngle = RBD.cube_list[i].eulerAngle;
            client.Send(RegisteFunction.BODYCUBE, BCM);
            sendtime++;
        }
        print(sendtime);//除錯用
    }




    void CreateMove(NetworkMessage msg)
    {
        print("CreateMove");
        MoveMessage mes = msg.ReadMessage<MoveMessage>();
        List<GameObject> objectlist = GetComponent<SpawnPrefabList>().RegisteObject;
        for (int i = 0; i < objectlist.Count; i++)
        {
            if (objectlist[i].GetComponent<ObjectName>())
                if (objectlist[i].GetComponent<ObjectName>().objectname == "move")
                {
                    GameObject g = objectlist[i];
                    g.GetComponent<BodyFindPlayer>().connectID = mes.connectID;

                    g = Instantiate(g);
                    NetworkServer.Spawn(g);
                    break;
                }
        }
    }

    void CubeConnect(NetworkMessage msg)
    {
        CubeConnectMessage CCM= msg.ReadMessage<CubeConnectMessage>();
        GameObject[] bodylist = GameObject.FindGameObjectsWithTag("body");
        for (int i = 0; i < bodylist.Length; i++)
            if (bodylist[i].GetComponent<BodyFindPlayer>().connectID == CCM.connectID)
            {
                bodylist[i].GetComponent<NetobjMainBodyStateControl>().StartConnectSurroundingState();
                print("NetobjMainBodyStateControl");
            }
    }

    void CreateRobot(NetworkMessage msg)
    {
        RobotMessage mes = msg.ReadMessage<RobotMessage>();
        List<GameObject> objectlist = GetComponent<SpawnPrefabList>().RegisteObject;
        for (int i = 0; i < objectlist.Count; i++)
        {
            if (objectlist[i].GetComponent<ObjectName>())
                if (objectlist[i].GetComponent<ObjectName>().objectname == "robot")
                {
                    GameObject g = objectlist[i];

                    g.GetComponent<RobotProperty>().connectID = mes.connectID;

//                    g.transform.Find("NetMove").GetComponent<MoveInPlayer>().connectID = mes.connectID;

                   // g.transform.Find("NetBody").GetComponent<BodyInPlayer>().connectID = mes.connectID;

                    g.transform.position = new Vector3(mes.connectID * 5, 5, mes.connectID * 5);
                    g = Instantiate(g);
                    NetworkServer.Spawn(g);
                    break;
                }
        }
    }

    void CreateBody(NetworkMessage msg)
    {
        print("CreateBody");
        BodyMessage mes = msg.ReadMessage<BodyMessage>();
        List<GameObject> objectlist = GetComponent<SpawnPrefabList>().RegisteObject;
        for (int i = 0; i < objectlist.Count; i++)
        {
            if (objectlist[i].GetComponent<ObjectName>())
                if (objectlist[i].GetComponent<ObjectName>().objectname == "body")
                {
                    GameObject g = objectlist[i];
                    g.GetComponent<BodyFindPlayer>().connectID = mes.connectID;

                    g.transform.position = new Vector3(mes.connectID * 5, 5, mes.connectID * 5);
                    g = Instantiate(g);
                    NetworkServer.Spawn(g);
                    break;
                }
        }
    }
    int createBodyCubeTime = 0;
    void CreateBodyCube(NetworkMessage msg)
    {
        print("CCcube");
        BodyCubeMessage mes = msg.ReadMessage<BodyCubeMessage>();
        List<GameObject> objectlist = GetComponent<SpawnPrefabList>().RegisteObject;
        for (int i = 0; i < objectlist.Count; i++)
        {
            if (objectlist[i].GetComponent<ObjectName>())
            {
                if (objectlist[i].GetComponent<ObjectName>().objectname == mes.name)
                {
                    GameObject g = objectlist[i];

                    if (mes.name == "straightwheel" || mes.name == "rotatewheel")//之後考慮改成用繼承的
                    {
                        g.GetComponent<NetWheel>().connectID = mes.connectID;
                        g.GetComponent<NetWheel>().localposition = mes.position;
                        g.GetComponent<NetWheel>().localeulerAngle = mes.eulerAngle;
                    }
                    else if (mes.name == "robotfoot")
                    {
                        print("foot");
                        g.GetComponent<NetRBFoot>().connectID = mes.connectID;
                        g.GetComponent<NetRBFoot>().localposition = mes.position;
                        g.GetComponent<NetRBFoot>().localeulerAngle = mes.eulerAngle;
                    }
                    else if (mes.name == "propeller")
                    {
                        g.GetComponent<NetProprller>().connectID = mes.connectID;
                        g.GetComponent<NetProprller>().localposition = mes.position;
                        g.GetComponent<NetProprller>().localeulerAngle = mes.eulerAngle;
                    }
                    else if (mes.name == "track")
                    {
                        g.GetComponent<NetTrack>().connectID = mes.connectID;
                        g.GetComponent<NetTrack>().localposition = mes.position;
                        g.GetComponent<NetTrack>().localeulerAngle = mes.eulerAngle;
                    }
                    else if (mes.name == "invisible")
                    {
                        g.GetComponent<NetInvisible>().connectID = mes.connectID;
                    }
                    else if (mes.name == "protect")
                    {
                        g.GetComponent<NetProtect>().connectID = mes.connectID;
                    }
                    else
                    {
                        g.GetComponent<FindBody>().connectID = mes.connectID;
                        g.GetComponent<FindBody>().localposition = mes.position;
                        g.GetComponent<FindBody>().localeulerAngle = mes.eulerAngle;
                    }
                    g = Instantiate(g);
                    NetworkServer.Spawn(g);
                    break;
                }
            }
        }
    }
}
