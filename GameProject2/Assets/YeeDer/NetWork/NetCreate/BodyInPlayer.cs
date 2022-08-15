using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BodyInPlayer : MonoBehaviour {

    public int connectID;

    void Start()
    {
        connectID = transform.parent.GetComponent<RobotProperty>().connectID;
    }

}
