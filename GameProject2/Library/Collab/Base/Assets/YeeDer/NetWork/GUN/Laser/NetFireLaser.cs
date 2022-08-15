using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class NetFireLaser : NetworkBehaviour {
    double cooltime = 0.8, firetime = 0;
    public GameObject Shot1;
	public GameObject Wave;
	public GameObject firepos;
    public AudioSource firesound;
	public NetPlayerController player;

    public float Disturbance = 0;

	public int ShotType = 0;

	private GameObject NowShot;

	void Start () {
		NowShot = null;
		player = GetComponentInParent<NetobjMainBodyStateControl> ().player;
	}

	void Update () {
		if (hasAuthority)
		{
            if (Input.GetMouseButtonDown(0))
            {
                if ((Time.time - firetime) >= cooltime)
                {
                    CmdFire();
                    firetime = Time.time;
                    firesound.Play();
                }
            }
        }
	}
	[Command]
	public void CmdFire()
	{
        print("in");
		GameObject Bullet;
		Bullet = Shot1;
		//Fire
		RaycastHit hitinfo;
		hitinfo = GetComponentInChildren<Nettpsguncontrol_lasercanon> ().hitInfo;
		firepos.transform.LookAt (hitinfo.point);
		GameObject s1 = (GameObject)Instantiate(Bullet, firepos.transform.position, firepos.transform.rotation);
        //		s1.GetComponent<BeamParam>().SetBeamParam(this.GetComponent<BeamParam>());
		s1.GetComponent<NetBeamCollision>().fromPlayer=player;
        print("s1");
		NetworkServer.Spawn(s1);
		GameObject wav = (GameObject)Instantiate(Wave, firepos.transform.position, firepos.transform.rotation);
        print("wav");
//		wav.GetComponent<BeamWave>().col = this.GetComponent<BeamParam>().BeamColor;
		NetworkServer.Spawn(wav);
        print("fire");
	}
}

