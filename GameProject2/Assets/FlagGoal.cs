using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FlagGoal : NetworkBehaviour {
	public int team=0;
	public flagControl flag;
	NetobjMainBodyStateControl body=null;
	// Use this for initialization
	void Start () {
		
	}

	public void init()
	{
		if (transform.root.tag == "redSpawn")
			flag = GameObject.Find ("flag_red(Clone)").GetComponent<flagControl>();
		else if(transform.root.tag == "blueSpawn")
			flag = GameObject.Find ("flag_blue(Clone)").GetComponent<flagControl>();
		flag.goal = this;
	}
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider other)
	{
		body = null;
		if (body = other.attachedRigidbody.GetComponentInParent<NetobjMainBodyStateControl> ()) {
			if (body.hasflag&&body.player.Team==team&&body.player.isLocalPlayer) 
			{
				body.player.FlagGoal ();
				body.flag.transform.SetParent (null);
				body.flag.back ();
				body.hasflag = false;
				body.flag.held = false;
				body.flag.pickable = true;
				body.flag.holder = null;
				foreach(Collider c in body.flag.GetComponentsInChildren<Collider> ())c.enabled = true;
				foreach(Renderer r in body.flag.GetComponentsInChildren<Renderer> ())r.enabled = true;
				foreach(Renderer r in body.flag.ray.GetComponentsInChildren<Renderer> ())r.enabled = false;
				body.flag = null;
			}
		}
	}

	

}
