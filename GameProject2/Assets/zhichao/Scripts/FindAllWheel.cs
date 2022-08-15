using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindAllWheel : MonoBehaviour {
    public List<GameObject> AllWheel;
    public Vector3 TotalWheelPos;
    public Vector3 preWheelPos;
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
    void Start () {
        for (int i = 0; i < transform.childCount; i++)//修改成尋找子物件而非用Tag尋找
        {
            if (transform.GetChild(i).GetComponent<wheelLControl>())
            {
                AllWheel.Add(transform.GetChild(i).gameObject);
                continue;
            }
            if (transform.GetChild(i).GetComponent<wheelRControl>())
            {
                AllWheel.Add(transform.GetChild(i).gameObject);
                continue;
            }
        }
        //AllWheel = GameObject.FindGameObjectsWithTag("wheel");
        print(AllWheel.Count);
        for (int i = 0; i < AllWheel.Count; i++)
        {
            TotalWheelPos += AllWheel[i].transform.position;
            //print(AllWheel[i].transform.position.x);
        }
        TotalWheelPos /= AllWheel.Count;
        preWheelPos = TotalWheelPos;
        TotalWheelPos -= this.gameObject.transform.position;
        for (int i = 0; i < AllWheel.Count; i++)
        {
            AllWheel[i].transform.position -= TotalWheelPos;
        }
        gameObject.transform.position = preWheelPos;
	}
	
	// Update is called once per frame
    void Update()
    {
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

