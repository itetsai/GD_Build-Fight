using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class faceToCamera: MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    Camera camera=null;
	// Update is called once per frame
	void Update ()
    {
        if (!camera)
        {
            GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < player.Length; i++)
                if (player[i].GetComponent<NetPlayerController>().local)
                {
                    camera = player[i].GetComponentInChildren<Camera>();
                    print("GGGetCamera");
                }
        }

        if (camera)
        {
            transform.rotation = camera.transform.rotation;
        }
    }
}
