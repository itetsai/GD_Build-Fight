using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagGoal : MonoBehaviour {
	public int team=0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider other)
	{
		NetobjMainBodyStateControl body;
		if (body = GetComponentInParent<NetobjMainBodyStateControl> ()) {
			if (body.hasflag&&body.player.Team==team) {
				GameMode.sInstance.calcscore (body.player.Team, 10);
				body.hasflag = false;
				body.flag.held = false;
				body.flag.pickable = true;
				body.flag.transform.SetParent (null);
				body.flag.back ();
				body.flag = null;
			}
		}
	}
}
