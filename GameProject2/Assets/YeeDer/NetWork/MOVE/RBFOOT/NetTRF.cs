using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetTRF : NetworkBehaviour {

    public bool IsOnGround;
    public GameObject triggerGround;
    // Use this for initialization
    public Camera Mycamera;
    public float ForwardForce;

    // Update is called once per frame
    void Update()
    {
        if (hasAuthority)
        {
            IsOnGround = triggerGround.GetComponent<OnGround>().onGround;

            if (Input.GetKey(KeyCode.W))
            {
                if (Input.GetKey(KeyCode.D))
                    Walk(45, 1);
                else if (Input.GetKey(KeyCode.A))
                    Walk(-45, 1);
                else
                    Walk(0, 1);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.D))
                    Walk(-45, -1);
                else if (Input.GetKey(KeyCode.A))
                    Walk(45, -1);
                else
                    Walk(0, -1);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                Walk(-90, 1);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Walk(90, 1);
            }


            if (Input.GetKeyUp(KeyCode.W))
                transform.parent.GetComponent<Rigidbody>().isKinematic = true;
            if (Input.GetKeyUp(KeyCode.S))
                transform.parent.GetComponent<Rigidbody>().isKinematic = true;
            if (Input.GetKeyUp(KeyCode.A))
                transform.parent.GetComponent<Rigidbody>().isKinematic = true;
            if (Input.GetKeyUp(KeyCode.D))
                transform.parent.GetComponent<Rigidbody>().isKinematic = true;

            transform.parent.GetComponent<Rigidbody>().isKinematic = false;
        }
    }


    void Walk(float r, float forward)
    {
        float a = transform.eulerAngles.y;
        float b = Mycamera.transform.root.eulerAngles.y + r;


        float rr = a - b;

        if (rr < 0)
            rr += 360;

        if (rr > 360)
            rr -= 360;

        if (rr < 0)
            rr += 360;

        if (rr > 360)
            rr -= 360;

        if (rr > 180)
            transform.Rotate(new Vector3(0, 1, 0));
        else
            transform.Rotate(new Vector3(0, -1, 0));

        if (IsOnGround)
            transform.parent.GetComponent<Rigidbody>().AddForce(transform.forward * ForwardForce * 1 * forward);

    }


    private void OnCollisionEnter()
    {
        IsOnGround = true;
    }

    private void OnCollisionExit()
    {
        IsOnGround = false;
    }
}
