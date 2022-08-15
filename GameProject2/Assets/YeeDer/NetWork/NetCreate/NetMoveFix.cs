using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetMoveFix : MonoBehaviour {
    
	// Use this for initialization
	void Start ()
    {
        int connectID = GetComponent<BodyFindPlayer>().connectID;
        GameObject[] bodylist = GameObject.FindGameObjectsWithTag("body");
        for (int i = 0; i < bodylist.Length; i++)
        {
            if (bodylist[i].GetComponent<BodyFindPlayer>().connectID == connectID)
            {
                transform.position = bodylist[i].transform.position;
                transform.rotation = bodylist[i].transform.rotation;
                GetComponent<FixedJoint>().connectedBody = bodylist[i].GetComponent<Rigidbody>();
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
