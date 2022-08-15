using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelControll : MonoBehaviour {

    // Use this for initialization

    public HingeJoint Straight;
    public HingeJoint Rotate;

    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        JointLimits j = new JointLimits();

        if (Input.GetKey(KeyCode.W))
        {
            Straight.useMotor = true;
        }
        else
            Straight.useMotor = false;


        if (Input.GetKey(KeyCode.Space))
        {
            JointLimits limit = new JointLimits();
            limit.max = Straight.gameObject.transform.eulerAngles.x;
            limit.min = Straight.gameObject.transform.eulerAngles.x;
            limit.bounciness = 100000;
            Straight.limits = limit;
            Straight.useLimits = true;
        }
        else
            Straight.useLimits = false;

    }
}
