using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anamatorControl : MonoBehaviour {
    private Animator animator;
    public float speed;
    public float speed_=5;
    public float minSpeed=0;
    public float maxSpeed=1;
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
    public Rigidbody rb;
    public GameObject test;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        speed = 0;
    }
    void Update()
    {
        float deltatime = Time.deltaTime;
        HandleKeyInput(deltatime);
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
        //rb.AddForce(transform.forward * deltaMove.z * 250);
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
       // if(Input.GetKeyDown(KeyCode.W))
        //    if (xInput == 0)
          //      gameObject.transform.eulerAngles = new Vector3(gameObject.transform.rotation.eulerAngles.x, test.transform.rotation.eulerAngles.y, gameObject.transform.rotation.eulerAngles.z);

        if (Input.GetKey(KeyCode.W))
        {
            zInput = 1.0f;
            addSpeed = 0.4f;
            speed = 1;
        }
        animator.SetFloat("speed", speed);
        if (Input.GetKeyUp(KeyCode.W))
        {
            zInput = 0.0f;
            speed = 0;
        }
        if (Input.GetKey(KeyCode.S))
        {
            zInput = -1.0f;
            addSpeed = 0.4f;
            speed = 1;
        }
        animator.SetFloat("speed", speed);
        if (Input.GetKeyUp(KeyCode.S))
        {
            zInput = 0.0f;
            speed = 0;
        }
    }
    void UpdateHorForce(float deltatime)
    {

        //float val = Mathf.Lerp(velocity.x, speed * xInput, deltatime * acceleration);
        float val = speed_ * xInput;
        SetForceX(val);
        val = speed_*zInput;
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

