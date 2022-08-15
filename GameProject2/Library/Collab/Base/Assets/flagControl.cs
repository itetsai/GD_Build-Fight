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
	public int team = 0;
	// Use this for initialization
	void Start () {
		startPoint = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider other)
	{
		NetobjMainBodyStateControl body=null;
		if(!held&& pickable)
		if ((body = other.attachedRigidbody.GetComponentInParent<NetobjMainBodyStateControl>())) {
			Debug.Log("flag found body "+body.player.playerName);
			if ((body.player.Team != team)) {
				Debug.Log(body.player.playerName+" pick flag counting");
				body.flagControlTime += Time.fixedDeltaTime;
			}
		}
		if(body!=null)
		if (body.flagControlTime >= 3) {
			Debug.Log(body.player.playerName+" got flag");
			body.flagControlTime = 0;
			holder = body;
			body.hasflag = true;
			body.flag = this;
			held = true;
			pickable = false;
			gameObject.transform.SetParent (body.transform);
			GetComponent<Renderer> ().enabled = false;
		}
		if(holder)
		if (holder.isDead) {
			Debug.Log(body.player.playerName+" dead drop flag");
			held = false;
			pickable = false;
			body.hasflag = false;
			body.flag = null;
			gameObject.transform.SetParent (null);
			GetComponent<Collider> ().enabled = true;
			GetComponent<Renderer> ().enabled = true;
			StartCoroutine (countdownBack ());
		}
	}
	void OnHeld(bool state)
	{
		Debug.Log("flag held state sync");
		held = state;
		if (held) {
			GetComponent<Collider> ().enabled = false;
			GetComponent<Renderer> ().enabled = false;
		} else {
			GetComponent<Collider> ().enabled = true;
			GetComponent<Renderer> ().enabled = true;
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
		transform.position = startPoint;;
	}
}
