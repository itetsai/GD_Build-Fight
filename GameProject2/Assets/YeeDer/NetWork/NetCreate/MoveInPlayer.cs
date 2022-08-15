using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInPlayer : MonoBehaviour {

    public int connectID;

    void Start()
    {
        connectID = transform.parent.GetComponent<RobotProperty>().connectID;
    }
}
