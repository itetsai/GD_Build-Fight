using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackAnimation : MonoBehaviour {

    public Renderer track;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 trackLOffset = track.material.mainTextureOffset;

        trackLOffset.x += 0.01f;
        track.material.mainTextureOffset = trackLOffset;
    }
}
