using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deadcheck : MonoBehaviour {
	public NetobjMainBodyStateControl body;
	// Use this for initialization
	void Start () {
		body = GetComponent<NetobjMainBodyStateControl> ();
	}

	public void Check()
	{
		if (body.currentHp < 0.1&&!body.isDead&&body.player.isLocalPlayer) {
			body.isDead = true;
			gameObject.GetComponent<Rigidbody> ().isKinematic = true;
			gameObject.GetComponent<Rigidbody> ().useGravity = false;
			body.repairList.Clear ();
			body.player.OnDeath ();
			StartCoroutine (body.waitForRespawn ());
		}
	}

	/*public IEnumerator waitForRespawn()
	{
		yield return new WaitForSeconds (10f);
		foreach (GameObject g in body.bodyList) {
			foreach(Renderer r in g.GetComponentsInChildren<Renderer>())r.enabled=true;
			foreach(Collider c in g.GetComponentsInChildren<Collider>())c.enabled = true;
			g.GetComponent<NetobjStateControl> ().CurrentHp = g.GetComponent<NetobjStateControl> ().MaxHp;
			g.GetComponent<NetobjStateControl> ().isBreak = false;
		}
		body.isDead = false;
		gameObject.GetComponent<Rigidbody> ().isKinematic = false;
		gameObject.GetComponent<Rigidbody> ().useGravity = true;
		body.currentHp = body.totalHp;
		if(body.player.isLocalPlayer)
			body.player.Respawn ();
		else if(body.player.isServer)
			body.player.RpcRespawn ();
	}*/
	// Update is called once per frame
	void Update () {
		
	}
}
