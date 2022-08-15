using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetMidPoint : MonoBehaviour
{
    public Vector3 all;
    public Vector3 now;
    public int count;
	void Start ()
    {
        all = new Vector3(0, 0, 0);
        now = new Vector3(0, 0, 0);
        count = 0;
    }

    public void Add(Vector3 v)
    {
        GetComponent<FixedJoint>().connectedBody = null;
        all += v;
        count++;
        now = all / count;
        transform.position = now+new Vector3(0,2,-2);
        GetComponent<FixedJoint>().connectedBody = transform.parent.Find("NetBody").GetComponent<Rigidbody>();
    }

}
