using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackControll : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public float forntMotor;
    public float backMotor;
    public float brake;

    public WheelCollider frontWheel;
    public WheelCollider backWheel;

    public float steerAngle;

    public Renderer track;
    // Update is called once per frame
    void Update()
    {
        WheelCollider[] wc = GetComponentsInChildren<WheelCollider>();

        if (Input.GetKey(KeyCode.W))//往前
        {
            for (int i = 0; i < wc.Length; i++)
                wc[i].motorTorque = forntMotor;
        }
        else if (Input.GetKey(KeyCode.S))//往後
        {
            for (int i = 0; i < wc.Length; i++)
                wc[i].motorTorque = -1*backMotor;
        }
        else
        {
            for (int i = 0; i < wc.Length; i++)
                wc[i].motorTorque = 0;
        }

        if (Input.GetKey(KeyCode.Space))//煞車
        {
            for (int i = 0; i < wc.Length; i++)
                wc[i].brakeTorque = brake;
        }
        else
        {
            for (int i = 0; i < wc.Length; i++)
                wc[i].brakeTorque = 0;
        }

        if (Input.GetKey(KeyCode.D))
        {
            frontWheel.steerAngle = steerAngle;
            backWheel.steerAngle = -steerAngle;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            frontWheel.steerAngle = -steerAngle;
            backWheel.steerAngle = steerAngle;
        }
        else
        {
            frontWheel.steerAngle = 0;
            backWheel.steerAngle = 0;
        }

        AnimationControll();
    }

    void AnimationControll()
    {
        Vector2 trackLOffset = track.material.mainTextureOffset;
        float rpm = GetComponentInChildren<WheelCollider>().rpm;
        if (rpm < 1 && rpm > -1)
            rpm = 0;

        trackLOffset.x -= rpm*0.00001f;
        print(rpm);
        track.material.mainTextureOffset = trackLOffset;
    }
}
