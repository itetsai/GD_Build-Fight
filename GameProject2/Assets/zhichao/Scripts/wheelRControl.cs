using UnityEngine;
using System.Collections;
public class wheelRControl : MonoBehaviour
{
    public int moving = 1;
    public Vector3 velocity = Vector3.zero;
    public Vector3 deltaMove = Vector3.zero;
    public float speed = 5;
    public float acceleration = 1.0f;
    float xInput = 0;
    float zInput = 0;
    public float envSpeed = 0;
    public float envAcceleration = 0;
    float mouseW;
    public Renderer wheelR;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
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

    void UpdateMovement(float deltatime)
    {
        deltaMove = velocity * deltatime;
       // gameObject.transform.Translate(0, 0, deltaMove.z * 2);
        wheelR.transform.Rotate(deltaMove.z * 100, 0, 0);
        if (xInput != 0)
        {
           // gameObject.transform.Rotate(Vector3.up, deltaMove.x * 3);
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
    }
    void UpdateHorForce(float deltatime)
    {

        //float val = Mathf.Lerp(velocity.x, speed * xInput, deltatime * acceleration);
        float val = speed * xInput;
        SetForceX(val);
        val = Mathf.Lerp(velocity.z, (speed + envSpeed) * zInput, deltatime * (acceleration + envAcceleration));
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