using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//將地板的tag設為ground


public class flash : MonoBehaviour {
    public TrailRenderer flashEffect;
    public Collider test;
    public Rigidbody body;
    public float flashDistance = 3;
    float time;
    Vector3 prePosition;
    Vector3 posPostion;

    // Use this for initialization
    void Start () {
        flashEffect.enabled = false;
        test.isTrigger = false;
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.F))
        {
            prePosition = gameObject.transform.position;
            time = 0;
            flashEffect.enabled = true;
            gameObject.transform.position += new Vector3(flashDistance, 0, 0);
            posPostion = gameObject.transform.position;
            test.isTrigger = true;
            body.useGravity = false;
        }
        time += Time.deltaTime;
        if (time > flashEffect.time + 1)
        {
            flashEffect.enabled = false;
            body.useGravity = true;
            test.isTrigger = false;
        }
	}
    void OnTriggerStay(Collider collider)
    {
        //if (collider.tag != "Ground")
            gameObject.transform.position -= (posPostion - prePosition) / 10;
    }
    
}
