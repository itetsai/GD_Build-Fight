using UnityEngine;
using System.Collections;

public class FireLaser : MonoBehaviour {
	double cooltime=0.8,firetime=0;
	public GameObject Shot1;
    public GameObject Wave;
	public GameObject firepos;
	public AudioSource firesound;
	public float Disturbance = 0;

	public int ShotType = 0;

	private GameObject NowShot;

	void Start () {
		NowShot = null;
	}

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
        GameObject Bullet;
        Bullet = Shot1;
        //Fire
        RaycastHit hitinfo;
        hitinfo = GetComponentInChildren<tpsguncontrol_lasercanon>().hitInfo;
        firepos.transform.LookAt(hitinfo.point);
        GameObject s1 = (GameObject)Instantiate(Bullet, firepos.transform.position, firepos.transform.rotation);
        s1.GetComponent<BeamParam>().SetBeamParam(this.GetComponent<BeamParam>());

        GameObject wav = (GameObject)Instantiate(Wave, firepos.transform.position, firepos.transform.rotation);
        wav.transform.localScale *= 0.25f;
        wav.transform.Rotate(Vector3.left, 90.0f);
        wav.GetComponent<BeamWave>().col = this.GetComponent<BeamParam>().BeamColor;
        firesound.Play();

    }
}
