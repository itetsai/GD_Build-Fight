using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trackControl : MonoBehaviour {
    public Renderer trackL;
    public Renderer trackR;
    public float trackSpeed = 0.02f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 trackLOffset = trackL.material.mainTextureOffset;
        trackLOffset.x -= trackSpeed;
        trackL.material.mainTextureOffset = trackLOffset;

        Vector2 trackROffset = trackR.material.mainTextureOffset;
        trackROffset.x -= trackSpeed;
        trackR.material.mainTextureOffset = trackROffset;
    }
}
