using UnityEngine;
using System.Collections;

public class flyControl: MonoBehaviour
{
    public float forceValue=200;//調整上升的力
    public float rotorRotateSpeed = 500;//螺旋槳旋轉速度
    public float statSpeed = 60;//穩定的速度
    public GameObject helicopter;
    public GameObject rotor01;
    public GameObject Camera_;
    public Rigidbody test;
    public Rigidbody body;
    public int moving = 1;
    public Vector3 velocity = Vector3.zero;
    public Vector3 deltaMove = Vector3.zero;
    public float speed = 5;
    public float acceleration = 1.0f;
    float xInput = 0;
    float zInput = 0;
    float yinput = 0;
    float yinput_ = 0;
    public float envSpeed = 0;
    public float envAcceleration = 0;
    float mouseW;
    public bool stat;
    float x_off, y_off, z_off;

    // Use this for initialization
    // Use this for initialization
    void Start()
    {
    }
    void Update()
    {
        mouseW = Input.mousePosition.x;
        float deltatime = Time.deltaTime;
        HandleKeyInput(deltatime);
    }
    void LateUpdate()
    {
        float deltatime = Time.deltaTime;
        UpdateHorForce(deltatime);

        UpdateMovement(deltatime);
    }

    // Update is called once per frame
    void UpdateMovement(float deltatime)
    {
        deltaMove = velocity * deltatime;
        // gameObject.transform.Translate(0, deltaMove.z*1.17f, deltaMove.z * 2);
        test.AddRelativeForce(Vector3.up * deltaMove.y * forceValue);
       // body.AddForce(Vector3.up * 10);
        if (yinput_ != 0)
            gameObject.transform.Rotate(0, -1 * yinput_ * 10 * deltatime, 0);
        if ((helicopter.transform.rotation.x < 30.0f / 114 && deltaMove.z > 0) || (deltaMove.z < 0 && helicopter.transform.rotation.x > -30.0f / 114))
            helicopter.transform.Rotate(deltaMove.z *5, 0, 0);
        if ((helicopter.transform.rotation.z < 30.0f / 114 && deltaMove.x < 0) || (helicopter.transform.rotation.z > -30.0f / 114 && deltaMove.x > 0))
            helicopter.transform.Rotate(0, 0, -1 * deltaMove.x * 5);
        if (deltaMove.y >= 0)
            rotor01.transform.Rotate(0, 0, deltaMove.z * rotorRotateSpeed + deltaMove.y * rotorRotateSpeed);
        else
            rotor01.transform.Rotate(0, 0, (0.1f + deltaMove.y) * rotorRotateSpeed);
        if (stat)
        {
            if (gameObject.transform.rotation.x > 0)
                x_off = -1f;
            else
                x_off = 1f;
            if (gameObject.transform.rotation.y > 0)
                y_off = -1f;
            else
                y_off = 1f;
            if (gameObject.transform.rotation.z > 0)
                z_off = -1f;
            else
                z_off = 1f;
            gameObject.transform.Rotate(x_off * deltatime * statSpeed,0, z_off * deltatime * statSpeed);
        }
        if (Camera_.transform.rotation.y != gameObject.transform.rotation.y)
        {
            if ((gameObject.transform.rotation.y-Camera_.transform.rotation.y)>0)
                y_off = -1f;
            else
                y_off = 1f;
            gameObject.transform.Rotate(0, y_off * deltatime * 20, 0);
        }
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
        if (Input.GetKey(KeyCode.Space))
        {
            yinput = 1.0f;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            yinput = 0.0f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            yinput = -1.0f;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            yinput = 0.0f;
        }
        //Z axis.
        if (Input.GetKey(KeyCode.W))
        {
            zInput = 1.0f;
        }
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
        ////////////////
        if (Input.GetKeyDown(KeyCode.Q))
        {
            yinput_ = 1.0f;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            yinput_ = 0;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            yinput_ = -1.0f;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            yinput_ = 0.0f;
        }
        if (xInput == 0 && zInput == 0)
        {
            stat = true;
        }
        else
            stat = false;
    }
    void UpdateHorForce(float deltatime)
    {

        //float val = Mathf.Lerp(velocity.x, speed * xInput, deltatime * acceleration);
        float val = speed * xInput;
        SetForceX(val);
        val = Mathf.Lerp(velocity.z, (speed + envSpeed) * zInput, deltatime * (acceleration + envAcceleration));
        SetForceZ(val);
        val = Mathf.Lerp(velocity.y, (speed + envSpeed) * yinput, deltatime * (acceleration + envAcceleration));
        SetForceY(val);         
    }
    public void SetForceX(float x)
    {
        velocity.x = x;
    }
    public void SetForceY(float y)
    {
        velocity.y = y;
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