using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetTrack : NetworkBehaviour {

    [SyncVar]
    public int connectID;
    [SyncVar]
    public Vector3 localposition;
    [SyncVar]
    public Vector3 localeulerAngle;
    // Use this for initialization


    public float forntMotor;
    public float backMotor;
    public float brake;

    public WheelCollider frontWheel;
    public WheelCollider backWheel;

    public float steerAngle;
    public float RotateMotor;

    public Renderer track;

    void Start ()
    {
        if (isServer)
        {
            GetComponent<NetworkIdentity>().AssignClientAuthority(NetworkServer.connections[connectID]);
        }
        GameObject[] robotlist = GameObject.FindGameObjectsWithTag("Robot");

        for (int i = 0; i < robotlist.Length; i++)
            if (robotlist[i].GetComponent<RobotProperty>().connectID == connectID)
            {
                transform.parent = robotlist[i].transform.Find("NetMove");
                transform.localPosition = localposition;
                transform.localEulerAngles = localeulerAngle;
                break;
            }

        if (transform.localEulerAngles.y > 170)
        {
            WheelCollider w = frontWheel;
            frontWheel = backWheel;
            backWheel = w;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (hasAuthority)
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
                    wc[i].motorTorque = -1 * backMotor;
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
                frontWheel.motorTorque *= RotateMotor;
                backWheel.motorTorque *= RotateMotor;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                frontWheel.steerAngle = -steerAngle;
                backWheel.steerAngle = steerAngle;
                frontWheel.motorTorque *= RotateMotor;
                backWheel.motorTorque *= RotateMotor;
            }
            else
            {
                frontWheel.steerAngle = 0;
                backWheel.steerAngle = 0;
            }

            AnimationControll();
        }
	}

    void AnimationControll()
    {
        Vector2 trackLOffset = track.material.mainTextureOffset;
        float rpm = GetComponentInChildren<WheelCollider>().rpm;
        if (rpm < 1 && rpm > -1)
            rpm = 0;

        if (transform.localEulerAngles.y > 170)
            trackLOffset.x += rpm * 0.00001f;
        else
            trackLOffset.x -= rpm * 0.00001f;

        track.material.mainTextureOffset = trackLOffset;
    }
}
