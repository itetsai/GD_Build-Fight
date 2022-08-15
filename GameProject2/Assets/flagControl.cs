using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class flagControl : NetworkBehaviour {
	[SyncVar(hook="OnHeld")]
	public bool held=false;
	[SyncVar(hook="OnPickableChanged")]
	public bool pickable=true;
	public NetobjMainBodyStateControl holder;
	public Vector3 startPoint;
	public GameObject ray;
	public FlagGoal goal;
	public int team = 0;
	// Use this for initialization
	void Start () {
		startPoint = transform.position;
		ray.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (!held)
			return;
		if (Input.GetKeyDown(KeyCode.F))
		{
			DropFlag ();
		}

	}

	void OnTriggerStay(Collider other)
	{
		NetobjMainBodyStateControl body=null;
		if(!held&& pickable)
		if ((body = other.attachedRigidbody.GetComponentInParent<NetobjMainBodyStateControl>())) {
			if ((body.player.Team != team)) {
				body.flagControlTime += Time.fixedDeltaTime;
			}
		}
		if(body!=null)
		if (body.flagControlTime >= 5) {
			Debug.Log(body.player.playerName+" got flag");
			body.flagControlTime = 0;
			holder = body;
			body.hasflag = true;
			body.flag = this;
			held = true;
			pickable = false;
			ray.SetActive (true);
			gameObject.transform.SetParent (body.transform);
			foreach(Collider c in GetComponentsInChildren<Collider> ())c.enabled = false;
			foreach(Renderer r in GetComponentsInChildren<Renderer> ())r.enabled = false;
			foreach(Renderer r in ray.GetComponentsInChildren<Renderer> ())r.enabled = true;
		}
	}
	public void DropFlag()
	{
		held = false;
		pickable = true;
		holder.hasflag = false;
		holder.flag = null;
		holder = null;
		gameObject.transform.SetParent (null);
		ray.SetActive (true);
		foreach(Collider c in GetComponentsInChildren<Collider> ())c.enabled = true;
		foreach(Renderer r in GetComponentsInChildren<Renderer> ())r.enabled = true;
		foreach(Renderer r in ray.GetComponentsInChildren<Renderer> ())r.enabled = true;
		StartCoroutine (countdownBack ());
		StartCoroutine (penaltyTime ());

	}
	public void deadDropFlag()
	{
		Debug.Log(holder.player.playerName+" dead drop flag");
		held = false;
		pickable = false;
		holder.hasflag = false;
		holder.flag = null;
		holder = null;
		gameObject.transform.SetParent (null);
		ray.SetActive (true);
		foreach(Collider c in GetComponentsInChildren<Collider> ())c.enabled = true;
		foreach(Renderer r in GetComponentsInChildren<Renderer> ())r.enabled = true;
		foreach(Renderer r in ray.GetComponentsInChildren<Renderer> ())r.enabled = true;
		StartCoroutine (countdownBack ());
		StartCoroutine (penaltyTime ());
	}
	public void GoalBack ()
	{
		transform.SetParent (null);
		back ();
		holder.hasflag = false;
		held = false;
		pickable = true;
		holder.flag = null;
		holder = null;
		foreach(Collider c in GetComponentsInChildren<Collider> ())c.enabled = true;
		foreach(Renderer r in GetComponentsInChildren<Renderer> ())r.enabled = true;
		foreach(Renderer r in ray.GetComponentsInChildren<Renderer> ())r.enabled = false;

		RpcGoalBack ();
	}

	[ClientRpc]
	public void RpcGoalBack()
	{
		Debug.Log ("Rpc Goal Back");

	}
	[ClientRpc]
	public void RpcdeadDropFlag()
	{
		Debug.Log("RPC "+holder.player.playerName+" dead drop flag");

	}

	void OnHeld(bool state)
	{
		Debug.Log("flag held state sync");
		held = state;
		if (held) {
			ray.SetActive (true);			
			foreach(Collider c in GetComponentsInChildren<Collider> ())c.enabled = false;
			foreach(Renderer r in GetComponentsInChildren<Renderer> ())r.enabled = false;
			foreach(Renderer r in ray.GetComponentsInChildren<Renderer> ())r.enabled = true;
		} else {
			holder = null;
			foreach(Collider c in GetComponentsInChildren<Collider> ())c.enabled = true;
			foreach(Renderer r in GetComponentsInChildren<Renderer> ())r.enabled = true;
		}
	}
	void OnPickableChanged(bool state)
	{
		Debug.Log("flag pickable state sync");
		pickable = state;
		if (!pickable&&!held)
			StartCoroutine (penaltyTime ());
	}
	IEnumerator penaltyTime()
	{
		yield return new WaitForSeconds (5f);
		pickable = true;
	}
	IEnumerator countdownBack()
	{
		Debug.Log("flag countdown back");
		for(int i=0;i<10;i++)
		{
			if (!held)
				yield return new WaitForSeconds (1f);
			else
				break;
			if (i == 9)
				back();
		}
	}
	public void back()
	{
		Debug.Log("flag back to start");
		transform.position = startPoint;
		ray.SetActive (false);
		foreach(Renderer r in ray.GetComponentsInChildren<Renderer> ())r.enabled = false;
	}
}
