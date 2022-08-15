using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class NetFireRocket : NetworkBehaviour {
	double cooltime=0.8,firetime=0;
	public GameObject firepos;
	public GameObject bullet;
	public NetPlayerController player=null;
	public AudioSource firesound;
	// Use this for initialization
	void Start () {
		if(GetComponentInParent<NetobjMainBodyStateControl> ()!=null)
		player = GetComponentInParent<NetobjMainBodyStateControl> ().player;
		if (player == null)
			StartCoroutine (getTeam ());
	}

	// Update is called once per frame
	void Update () {
		if (hasAuthority)
		{
			if (Input.GetMouseButtonDown (0)) {
				if ((Time.time - firetime) >= cooltime) {
					CmdFire ();
					firetime = Time.time;
				}
			}
		}
	}
	[Command]
	public void CmdFire()
	{
		RaycastHit hitinfo;
		hitinfo = GetComponentInChildren<Nettpsguncontrol_lasercanon> ().hitInfo;
		firepos.transform.LookAt (hitinfo.point);
		GameObject obj = Instantiate(bullet,firepos.transform.position,firepos.transform.rotation);
		obj.GetComponent<NetRocketExplosion> ().fromPlayer= player;
		obj.GetComponent<Rigidbody>().AddForce(firepos.transform.forward*1500);
		NetworkServer.Spawn(obj);
		RpcFireSound ();

	}
	[ClientRpc]
	void RpcFireSound()
	{
		firesound.Play();
	}
	IEnumerator getTeam()
	{
		while (true) {
			yield return new WaitForSeconds (0.5f);
			player = GetComponentInParent<NetobjMainBodyStateControl> ().player;
			if (player != null)
				break;
		}
	}
}
