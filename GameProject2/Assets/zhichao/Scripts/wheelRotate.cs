using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheelRotate : MonoBehaviour {
    // Use this for initialization
    float deltaTime = 0;
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKey(KeyCode.W))
        {
            deltaTime += Time.deltaTime*1;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            deltaTime += Time.deltaTime*1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            deltaTime -= Time.deltaTime*1;

        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            deltaTime -= Time.deltaTime*1;
        }
        this.transform.Rotate(deltaTime, 0, 0);
        deltaTime -= Time.deltaTime * 0.2f;
        if (deltaTime < -1)
            deltaTime = 0;
    }
}
