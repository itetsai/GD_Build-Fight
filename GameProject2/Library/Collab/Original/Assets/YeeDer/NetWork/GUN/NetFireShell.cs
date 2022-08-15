using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class NetFireShell : NetworkBehaviour {
	double cooltime=0.8,firetime=0;
	public GameObject firepos;
	public GameObject bullet;
	// Use this for initialization
	void Start ()
    {

	}

	// Update is called once per frame
	void Update ()
    {
        if(hasAuthority)
		if (Input.GetMouseButtonDown (0)) {
			if ((Time.time - firetime) >= cooltime) {
				CmdFire ();
				firetime = Time.time;
			}
		}
	}
    [Command]
	public void CmdFire()
	{
        RaycastHit hitinfo;
        hitinfo = GetComponent<Nettpsguncontrol>().hitInfo;
		firepos.transform.LookAt (hitinfo.point);
		GameObject obj = Instantiate(bullet,firepos.transform.position,firepos.transform.rotation);
		obj.GetComponent<Rigidbody>().AddForce(firepos.transform.forward*2000);
		NetworkServer.Spawn(obj);
	}
}
