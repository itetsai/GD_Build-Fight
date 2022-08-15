using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RobotMessage : MessageBase
{
    public int connectID;
    public uint PlayerNetworkID;
}

public class BodyMessage : MessageBase
{
    public int connectID;
    public uint PlayerNetworkID;
}

public class MoveMessage : MessageBase
{
    public int connectID;
    public uint PlayerNetworkID;
}

public class BodyCubeMessage : MessageBase
{
    public int connectID;
    public uint PlayerNetworkID;

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

    public void ServeRegite()
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
                    g.GetComponent<BodyFindPlayer>().PlayerNetworkID = mes.PlayerNetworkID;

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
            if (objectlist[i].GetComponent<ObjectName>().objectname == "robot")
            {
                GameObject g = objectlist[i];

                g.GetComponent<RobotProperty>().connectID = mes.connectID;
                g.GetComponent<RobotProperty>().PlayerNetworkID = mes.PlayerNetworkID;

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
            if (objectlist[i].GetComponent<ObjectName>().objectname == "body")
            {
                GameObject g = objectlist[i];
                g.GetComponent<BodyFindPlayer>().connectID = mes.connectID;
                g.GetComponent<BodyFindPlayer>().PlayerNetworkID = mes.PlayerNetworkID;

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
                if (objectlist[i].GetComponent<ObjectName>().objectname == mes.name)
                {
                    GameObject g = objectlist[i];

                    g.GetComponent<FindBody>().connectID = mes.connectID;
                    g.GetComponent<FindBody>().PlayerNetworkID = mes.PlayerNetworkID;
                    g.GetComponent<FindBody>().localposition = mes.position;
                    g.GetComponent<FindBody>().localeulerAngle = mes.eulerAngle;

                    g = Instantiate(g);
                    NetworkServer.Spawn(g);
                    break;
                }
        }
    }
}
