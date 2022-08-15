using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class NetFindAllWheel : NetworkBehaviour
{
    public int moving = 1;
    public Vector3 velocity = Vector3.zero;
    public Vector3 deltaMove = Vector3.zero;
    public float speed = 5;
    public float acceleration = 1.0f;
    float xInput_ = 0;
    float zInput_ = 0;
    public float envSpeed = 0;
    public float envAcceleration = 0;
    float mouseW;
    public Rigidbody rb;
    // Use this for initialization


    public List<Vector3> AllWheelLocalPosition;
    public List<GameObject> AllWheel;

    public Vector3 startPosition;

    public Vector3 NowPosition;

    private void Start()
    {

    }

    public void FindWheel(GameObject newWheel)
    {
            //完全不會動到localposition的方法
            GameObject Netbody = transform.parent.Find("NetBody").gameObject;
            AllWheel.Add(newWheel);
            for (int i = 0; i < AllWheel.Count; i++)//將所有wheel都移到body裡並做連結
            {
                AllWheel[i].transform.parent = Netbody.transform;
                Netbody.GetComponent<NetobjMainBodyStateControl>().StartConnectSurroundingState();
            }
            Vector3 midPosition = new Vector3(0, 0, 0);
            for (int i = 0; i < AllWheel.Count; i++)
                midPosition += AllWheel[i].transform.position;
            midPosition /= AllWheel.Count;
            print("AllWheel.Count:" + AllWheel.Count);
            print("midPosition:" + midPosition);

            gameObject.transform.position = midPosition;
            for (int i = 0; i < AllWheel.Count; i++)//將wheel從body裡移回move
                AllWheel[i].transform.parent = transform;

            NowPosition = midPosition;//因為不明原因位置會跑掉所以要設這個posiiton

    }

    float time = 0;
    bool k = true;
    void Update()
    {
            if (k)
            {
                if (time < 1)//因為不明原因位置會跑掉所以要設這個
                {
                    time += Time.deltaTime;
                    transform.position = NowPosition;
                }
                else
                {
                    k = false;
                }
                if (k == false)
                {
                    gameObject.AddComponent<FixedJoint>().connectedBody = transform.parent.Find("NetBody").GetComponent<Rigidbody>();
                    GetComponent<Rigidbody>().isKinematic = false;
                    print("testTime");
                }
            }

        mouseW = Input.mousePosition.x;
        float deltatime = Time.deltaTime;
        HandleKeyInput_(deltatime);
    }
    void LateUpdate()
    {
        float deltatime = Time.deltaTime;
        UpdateHorForce_(deltatime);

        UpdateMovement(deltatime);
    }

    void UpdateMovement(float deltatime)
    {
        deltaMove = velocity * deltatime;
        rb.AddForce(transform.forward * deltaMove.z * 200);
        //gameObject.transform.Translate(0, 0, deltaMove.z * 2);
        //wheelR.transform.Rotate(deltaMove.z * 100, 0, 0);
        if (xInput_ != 0)
        {
            gameObject.transform.Rotate(Vector3.up, deltaMove.x * 3);
        }
        /* else
         {
             if (mouseW - Screen.width / 2 > 50)
                 gameObject.transform.Rotate(Vector3.up, 3 * speed * deltatime);
             else if (mouseW - Screen.width / 2 < -50)
                 gameObject.transform.Rotate(Vector3.up, -3 * speed * deltatime);
         }*/
        /*       //recalute velocity by final deltaMove.
               if (deltatime & gt; 0)
               {
                   velocity = deltaMove / deltatime;
               }*/
    }
    void HandleKeyInput_(float deltatime)
    {
        //X axis.
        if (Input.GetKey(KeyCode.A))
        {
            xInput_ = -1.0f;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            xInput_ = 0.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            xInput_ = 1.0f;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            xInput_ = 0.0f;
        }

        //Z axis.
        if (Input.GetKey(KeyCode.W))
        {
            zInput_ = 1.0f;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            zInput_ = 0.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            zInput_ = -1.0f;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            zInput_ = 0.0f;
        }
    }
    void UpdateHorForce_(float deltatime)
    {

        //float val = Mathf.Lerp(velocity_.x, speed_ * xInput_, deltatime * acceleration_);
        float val = speed * xInput_;
        SetForceX_(val);
        val = Mathf.Lerp(velocity.z, (speed + envSpeed) * zInput_, deltatime * (acceleration + envAcceleration));
        SetForceZ_(val);

    }
    public void SetForceX_(float x)
    {
        velocity.x = x;
    }
    public void SetForceZ_(float z)
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
