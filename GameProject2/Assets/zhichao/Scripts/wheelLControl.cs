using UnityEngine;
using System.Collections;
public class wheelLControl : MonoBehaviour
{
    public int moving = 1;
    public Vector3 velocity_ = Vector3.zero;
    public Vector3 deltaMove_ = Vector3.zero;
    public float speed_ = 5;
    public float acceleration_ = 1.0f;
    float xInput_ = 0;
    float zInput_ = 0;
    public float envSpeed_ = 0;
    public float envAcceleration_ = 0;
    float mouseW_;
    public Renderer wheelL;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mouseW_ = Input.mousePosition.x;
        float deltatime = Time.deltaTime;
        HandleKeyInput_(deltatime);
    }
    void LateUpdate()
    {
        float deltatime = Time.deltaTime;
        UpdateHorForce_(deltatime);

        UpdateMovement_(deltatime);
    }

    void UpdateMovement_(float deltatime)
    {
        deltaMove_ = velocity_ * deltatime;
        //gameObject.transform.Translate(0, 0, deltaMove_.z * 2);
        wheelL.transform.Rotate(-1*deltaMove_.z * 100, 0, 0);
        if (xInput_ != 0)
        {
         //   gameObject.transform.Rotate(Vector3.up, deltaMove_.x * 3);
        }
      /*  else
        {
            if (mouseW_ - Screen.width / 2 > 50)
                gameObject.transform.Rotate(Vector3.up, 3 * speed_ * deltatime);
            else if (mouseW_ - Screen.width / 2 < -50)
                gameObject.transform.Rotate(Vector3.up, -3 * speed_ * deltatime);
        }*/
        /*       //recalute velocity_ by final deltaMove_.
               if (deltatime & gt; 0)
               {
                   velocity_ = deltaMove_ / deltatime;
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
        float val = speed_ * xInput_;
        SetForceX_(val);
        val = Mathf.Lerp(velocity_.z, (speed_ + envSpeed_) * zInput_, deltatime * (acceleration_ + envAcceleration_));
        SetForceZ_(val);

    }
    public void SetForceX_(float x)
    {
        velocity_.x = x;
    }
    public void SetForceZ_(float z)
    {
        velocity_.z = z;
    }

    public void AddEnvSpeed(float val)
    {
        envSpeed_ += val;
    }
    public void AddEnvAcc(float val)
    {
        envAcceleration_ += val;
    }
}