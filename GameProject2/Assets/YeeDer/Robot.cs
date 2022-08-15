using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;





public class Robot : MonoBehaviour
{
    public List<List<List<GameObject>>> BodyList;
    public string name;
    int team;
}

public class CreateRobot:MonoBehaviour
{
    //讀取未反組譯字串並組譯，對於網路來說會比較方便
    public void Create(string createList, Vector3 createPosition)
    {
        GameObject robot = new GameObject();
        robot.AddComponent<Robot>();
        robot.AddComponent<NetworkIdentity>();
        robot.name = "T1";
        robot = Instantiate(robot);

        RobotBodyData load = SaveLoadRobot.AssembleFileString(createList);//反組譯字串

    }
}