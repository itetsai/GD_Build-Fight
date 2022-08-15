using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour {
	double cooltime=0.1,firetime=0;
	public GameObject firepos;
	public GameObject bullet;
	public AudioSource firesound;
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) {
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
		obj.GetComponent<Rigidbody>().AddForce(firepos.transform.forward*8000);

		/*if (hitinfo.collider.gameObject.GetComponent<objStateControl> ())
			hitinfo.collider.gameObject.GetComponent<objStateControl> ().OnDamage (damage);*/
	}
}
