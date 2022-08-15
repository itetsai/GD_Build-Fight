using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShell : MonoBehaviour {
	double cooltime=0.8,firetime=0;
	public GameObject firepos;
	public GameObject bullet;
	public AudioSource firesound;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			if((Time.time-firetime)>=cooltime)
			{
				Fire ();
				firetime = Time.time;
			}
		}
	}
	public void Fire()
	{
		RaycastHit hitinfo;
		hitinfo = GetComponent<tpsguncontrol>().hitInfo;
		firepos.transform.LookAt (hitinfo.point);
		GameObject obj = Instantiate(bullet,firepos.transform.position,firepos.transform.rotation) as GameObject;
		firesound.Play ();
		obj.GetComponent<Rigidbody>().AddForce(firepos.transform.forward*2000);
	}
}
