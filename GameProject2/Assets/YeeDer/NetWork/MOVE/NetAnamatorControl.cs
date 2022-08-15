using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetAnamatorControl : NetworkBehaviour
{
    [SyncVar]
    public int connectID;
    [SyncVar]
    public Vector3 localposition;
    [SyncVar]
    public Vector3 localeulerAngle;

    private Animator animator;
    public float speed;
    public float speed_ = 5;
    public float minSpeed = 0;
    public float maxSpeed = 1;
    public float addSpeed;
    public int moving = 1;
    public Vector3 velocity = Vector3.zero;
    public Vector3 deltaMove = Vector3.zero;
    public float acceleration = 1.0f;
    float xInput = 0;
    float zInput = 0;
    public float envSpeed = 0;
    public float envAcceleration = 0;
    float mouseW;
    static public bool pause;
    public GameObject test;
    void Start()
    {
        if (isServer)
        {
            GetComponent<NetworkIdentity>().AssignClientAuthority(NetworkServer.connections[connectID]);
        }

        GameObject[] robotlist = GameObject.FindGameObjectsWithTag("Robot");

        for (int i = 0; i < robotlist.Length; i++)
            if (robotlist[i].GetComponent<RobotProperty>().connectID == connectID)
            {
                transform.parent = robotlist[i].transform.Find("NetBody");
                transform.localPosition = localposition;
                transform.localEulerAngles = localeulerAngle;

                GameObject midpoint = transform.parent.parent.Find("MiddlePoint").gameObject;
                midpoint.GetComponent<SetMidPoint>().Add(transform.position);
                break;
            }

        for (int i = 0; i < robotlist.Length; i++)
        {
            print(robotlist[i].GetComponent<RobotProperty>().connectID);
            print(connectID);
            if (robotlist[i].GetComponent<RobotProperty>().connectID == connectID)
            {
                transform.parent = robotlist[i].transform;
                transform.localEulerAngles = localeulerAngle;
                break;
            }
            print(transform.parent.name);
        }

        //GameObject Netbody = transform.parent.parent.Find("NetBody").gameObject;
        //Netbody.GetComponent<NetobjMainBodyStateControl>().StartConnectSurroundingState();//建立連結(會連鎖爆炸)

        animator = GetComponent<Animator>();
        speed = 0;
        gameObject.AddComponent<FixedJoint>().connectedBody = transform.parent.Find("NetBody").GetComponent<Rigidbody>();

    }
    void Update()
    {
        if (hasAuthority)
        {
            float deltatime = Time.deltaTime;
            HandleKeyInput(deltatime);
        }
    }
    void LateUpdate()
    {
        float deltatime = Time.deltaTime;
        UpdateHorForce(deltatime);

        UpdateMovement(deltatime);
    }
    void UpdateMovement(float deltatime)
    {
        deltaMove = velocity * deltatime;
        gameObject.transform.Translate(0, 0, deltaMove.z * 2);
        if (xInput != 0)
        {
            gameObject.transform.Rotate(Vector3.up, deltaMove.x * 3);
            // test.transform.rotation = new Quaternion(test.transform.rotation.x, gameObject.transform.rotation.y, test.transform.rotation.z, test.transform.rotation.w);
            //gameObject.transform.rotation = new Quaternion(gameObject.transform.rotation.x, test.transform.rotation.y, gameObject.transform.rotation.z, gameObject.transform.rotation.w);

        }
        else
        {
            //test.transform.rotation = new Quaternion(test.transform.rotation.x, gameObject.transform.rotation.y, test.transform.rotation.z,test.transform.rotation.w);

        }
        /*
        else
        {
            if (mouseW - Screen.width / 2 > 3)
                gameObject.transform.Rotate(Vector3.up, 3 * speed_ * deltatime);
            else if (mouseW - Screen.width / 2 < -3)
                gameObject.transform.Rotate(Vector3.up, -3 * speed_ * deltatime);
        }
        */

    }
    void HandleKeyInput(float deltatime)
    {
        //X axis.
        if (Input.GetKey(KeyCode.A))
        {
            xInput = -1.0f;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            xInput = 0.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            xInput = 1.0f;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            xInput = 0.0f;
        }

        //Z axis.
        if (Input.GetKeyDown(KeyCode.W))
            if (xInput == 0)
                gameObject.transform.eulerAngles = new Vector3(gameObject.transform.rotation.eulerAngles.x, test.transform.rotation.eulerAngles.y, gameObject.transform.rotation.eulerAngles.z);

        if (Input.GetKey(KeyCode.W))
        {
            zInput = 1.0f;
            addSpeed = 0.4f;
            speed = Mathf.MoveTowards(speed, maxSpeed, addSpeed * Time.deltaTime);
        }
        else
        {
            speed = Mathf.MoveTowards(speed, minSpeed, addSpeed * Time.deltaTime * 2);
        }
        animator.SetFloat("speed", speed);
        if (Input.GetKeyUp(KeyCode.W))
        {
            zInput = 0.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            zInput = -1.0f;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            zInput = 0.0f;
        }
    }
    void UpdateHorForce(float deltatime)
    {

        //float val = Mathf.Lerp(velocity.x, speed * xInput, deltatime * acceleration);
        float val = speed_ * xInput;
        SetForceX(val);
        val = Mathf.Lerp(velocity.z, (speed_ + envSpeed) * zInput, deltatime * (acceleration + envAcceleration));
        SetForceZ(val);

    }
    public void SetForceX(float x)
    {
        velocity.x = x;
    }
    public void SetForceZ(float z)
    {
        velocity.z = z;
    }

    public void AddEnvSpeed(float val)
    {
        envSpeed += val;
    }
    public void AddEnvAcc(float val)
    {
        envAcceleration += val;
    }
}

