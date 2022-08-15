using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour {
	public NetobjMainBodyStateControl body;
	public List<NetFireBullet> regular=new List<NetFireBullet>();
	public List<NetFireRocket> rocket=new List<NetFireRocket>();
	public List<NetFireLaser> laser=new List<NetFireLaser>();
	public List<NetFireShell> shell=new List<NetFireShell>();
	// Use this for initialization
	void Start () {
		regular = body.regular;
		rocket = body.rocket;
		laser = body.laser;
		shell = body.shell;
		reset ();
	}
	public void reset()
	{
		foreach (NetFireBullet b in regular)
			b.enabled = false;
		foreach (NetFireRocket b in rocket)
			b.enabled = false;
		foreach (NetFireLaser b in laser)
			b.enabled = false;
		foreach (NetFireShell b in shell)
			b.enabled = false;
		do {
			if (regular.Count != 0) {
				foreach (NetFireBullet b in regular)
					b.enabled = true;
				break;
			}
			if (rocket.Count != 0) {
				foreach (NetFireRocket b in rocket)
					b.enabled = true;
				break;
			}
			if (laser.Count != 0) {
				foreach (NetFireLaser b in laser)
					b.enabled = true;
				break;
			}
			if (shell.Count != 0) {
				foreach (NetFireShell b in shell)
					b.enabled = true;
				break;
			}
			break;
		} while(true);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			foreach (NetFireBullet b in regular)
				b.enabled = true;
			foreach (NetFireRocket b in rocket)
				b.enabled = false;
			foreach (NetFireLaser b in laser)
				b.enabled = false;
			foreach (NetFireShell b in shell)
				b.enabled = false;
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			foreach (NetFireBullet b in regular)
				b.enabled = false;
			foreach (NetFireRocket b in rocket)
				b.enabled = true;
			foreach (NetFireLaser b in laser)
				b.enabled = false;
			foreach (NetFireShell b in shell)
				b.enabled = false;
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			foreach (NetFireBullet b in regular)
				b.enabled = false;
			foreach (NetFireRocket b in rocket)
				b.enabled = false;
			foreach (NetFireLaser b in laser)
				b.enabled = true;
			foreach (NetFireShell b in shell)
				b.enabled = false;
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			foreach (NetFireBullet b in regular)
				b.enabled = false;
			foreach (NetFireRocket b in rocket)
				b.enabled = false;
			foreach (NetFireLaser b in laser)
				b.enabled = false;
			foreach (NetFireShell b in shell)
				b.enabled = true;
		}
	}
}
