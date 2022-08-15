using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class NetFireLaser : NetworkBehaviour {
    double cooltime = 0.8, firetime = 0;
    public GameObject ShotBlueTeam;
	public GameObject ShotRedTeam;
	public GameObject Wave;
	public GameObject firepos;
    public AudioSource firesound;
	public NetPlayerController player=null;

    public float Disturbance = 0;

	public int ShotType = 0;

	private GameObject NowShot;

	void Start () {
		NowShot = null;
		if(GetComponentInParent<NetobjMainBodyStateControl> ()!=null)
		player = GetComponentInParent<NetobjMainBodyStateControl> ().player;
		if (player == null)
			StartCoroutine (getTeam ());
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

	void Update () {
		if (hasAuthority)
		{
            if (Input.GetMouseButtonDown(0))
            {
                if ((Time.time - firetime) >= cooltime)
                {
                    CmdFire();
                    firetime = Time.time;
                    
                }
            }
        }
	}
	[Command]
	public void CmdFire()
	{
        print("in");
		GameObject Bullet;
		if(player.Team==0)
		Bullet = ShotBlueTeam;
		else
			Bullet = ShotRedTeam;
		//Fire
		RaycastHit hitinfo;
		hitinfo = GetComponentInChildren<Nettpsguncontrol_lasercanon> ().hitInfo;
		firepos.transform.LookAt (hitinfo.point);
		Bullet.GetComponent<NetBeamCollision>().fromPlayer=player;
		GameObject s1 = (GameObject)Instantiate(Bullet, firepos.transform.position, firepos.transform.rotation);
        //		s1.GetComponent<BeamParam>().SetBeamParam(this.GetComponent<BeamParam>());
        print("s1");
		NetworkServer.Spawn(s1);
		GameObject wav = (GameObject)Instantiate(Wave, firepos.transform.position, firepos.transform.rotation);
        print("wav");
//		wav.GetComponent<BeamWave>().col = this.GetComponent<BeamParam>().BeamColor;
		NetworkServer.Spawn(wav);
        print("fire");
		RpcFireSound ();

	}
	[ClientRpc]
	void RpcFireSound()
	{
		firesound.Play();
	}
		
}

