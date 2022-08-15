using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamWaveS : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        transform.localScale *= 0.25f;
        transform.Rotate(Vector3.left, 90.0f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
