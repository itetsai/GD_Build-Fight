using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asdasd : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.D))
        {
            print("key");
            this.transform.localEulerAngles = new Vector3(0, 25, 0);
        }
        else
        {
            this.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
	}
}
