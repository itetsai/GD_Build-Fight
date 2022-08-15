using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RFAnimation : MonoBehaviour {
    public float xInput;
    public float zInput;
    public float speed;
    private Animator animator;
    // Use this for initialization
    void Start () {
        speed = 0;
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
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

        if (Input.GetKey(KeyCode.W))
        {
            zInput = 1.0f;
            speed = 1;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            zInput = 0.0f;
            speed = 0;
        }
        if (Input.GetKey(KeyCode.S))
        {
            zInput = -1.0f;
            speed = 1;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            zInput = 0.0f;
            speed = 0;
        }



        if (Input.GetKey(KeyCode.A))
        {
            zInput = -1.0f;
            speed = 1;
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            zInput = 0.0f;
            speed = 0;
        }

        if (Input.GetKey(KeyCode.D))
        {
            zInput = -1.0f;
            speed = 1;
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            zInput = 0.0f;
            speed = 0;
        }
        animator.SetFloat("speed", speed);
    }
}
