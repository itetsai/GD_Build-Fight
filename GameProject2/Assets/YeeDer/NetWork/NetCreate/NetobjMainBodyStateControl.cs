using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetobjMainBodyStateControl :NetworkBehaviour
{
	double autorepaircooltime=0,autorepairsuspend=0;

	[SyncVar(hook="OnDeadStateChanged")]
	public bool isDead=false;

	public float totalHp = 0;

	[SyncVar(hook="OnHpChanged")]
	public float currentHp=0;
	bool bodyChecked=false;
    int maxBodyCount, currentBodyCount;
    public List<GameObject> bodyList = new List<GameObject>();
	public List<GameObject> repairList= new List<GameObject>();
	public List<NetFireBullet> regular=new List<NetFireBullet>();
	public List<NetFireRocket> rocket=new List<NetFireRocket>();
	public List<NetFireLaser> laser=new List<NetFireLaser>();
	public List<NetFireShell> shell=new List<NetFireShell>();
	public WeaponControl WC;
	public RobotProperty rp;
	public NetPlayerController player=null;
	public deadcheck dead;
	public float flagControlTime=0;
	public bool hasflag = false;
	public flagControl flag;
	public HealthBarScript hpbar;
    private void Start()
    {
		dead = GetComponent<deadcheck> ();
		rp = gameObject.GetComponentInParent<RobotProperty> ();
		GameObject[] players;
		players=GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject g in players) {
			if (g.GetComponent<NetPlayerController> ().connectID == rp.connectID) {
				player = g.GetComponent<NetPlayerController> ();
				break;
			}
		}
		/*if (player.isServer) {
			GetComponent<NetworkIdentity> ().AssignClientAuthority (NetworkServer.connections [player.connectID]);
			Debug.Log ("Grant " + player.playerName + " mainbodycontrol " + player.connectID);
		}*/
		if (!player.isServer||!player.isLocalPlayer)//方塊破壞交給server處理
			enabled = false;
		player.main = this;
		hpbar = HealthBarScript.FindObjectOfType<HealthBarScript> ();

    }
    public void StartConnectSurroundingState()
    {
		int count;
		count = transform.parent.Find ("NetMove").childCount;
		print(player.bodycount+" connect to start connect "+(transform.childCount+count)+" now" );
		if (player.bodycount - (transform.childCount+count) <=3) {

			print (player.playerName + " StartConnect");

			count = transform.root.childCount;
			NetobjStateControl body;
			for (int i = 0; i < count; i++) {//抓body & gun
				GameObject temp = transform.root.GetChild (i).gameObject;
				if (body = temp.GetComponent<NetobjStateControl> ()) {
					body.getSurroundingState ();
					if (!bodyList.Contains (body.gameObject)) {
						bodyList.Add (body.gameObject);
						totalHp += body.MaxHp;
					}
				} else {
					for (int j = 0; j < temp.transform.childCount; j++) {
						if (body = temp.transform.GetChild (j).GetComponent<NetobjStateControl> ()) {
							body.getSurroundingState ();
							if (!bodyList.Contains (body.gameObject)) {
								bodyList.Add (body.gameObject);
								totalHp += body.MaxHp;
							}
							if (body.GetComponent<NetFireBullet> () && !regular.Contains (body.GetComponent<NetFireBullet> ())) {
								regular.Add (body.GetComponent<NetFireBullet> ());
							}
							if (body.GetComponent<NetFireRocket> () && !rocket.Contains (body.GetComponent<NetFireRocket> ())) {
								rocket.Add (body.GetComponent<NetFireRocket> ());
							}
							if (body.GetComponent<NetFireLaser> () && !laser.Contains (body.GetComponent<NetFireLaser> ())) {
								laser.Add (body.GetComponent<NetFireLaser> ());
							}
							if (body.GetComponent<NetFireShell> () && !shell.Contains (body.GetComponent<NetFireShell> ())) {
								shell.Add (body.GetComponent<NetFireShell> ());

							}

						}
					}
				}
			}

			count = transform.parent.Find ("NetMove").childCount;
			for (int i = 0; i < count; i++) {//抓move
				GameObject temp = transform.parent.Find ("NetMove").GetChild (i).gameObject;
				if (body = temp.GetComponent<NetobjStateControl> ()) {
					body.getSurroundingState ();
					if (!bodyList.Contains (body.gameObject)) {
						bodyList.Add (body.gameObject);
						totalHp += body.MaxHp;
					}
				}
			}
			currentHp = totalHp;

			maxBodyCount = bodyList.Count;
			currentBodyCount = maxBodyCount;
			bodyChecked = true;
		}
    }
	public void healing(float amount)
	{
		if (!isDead) {
			if (repairList.Count != 0) {
				if ((Time.time - autorepairsuspend) >= 1) {
					autorepairsuspend = Time.time;
					do {

						NetobjStateControl healingPart = repairList [0].GetComponent<NetobjStateControl> ();
						//Debug.Log("Healing "+repairList[0].name+" for "+amount);
						healingPart.CurrentHp += amount;
						currentHp += amount;
						if (healingPart.CurrentHp > healingPart.MaxHp) {
							amount = healingPart.CurrentHp - healingPart.MaxHp;
							currentHp -= amount;
							healingPart.CurrentHp = healingPart.MaxHp;
						} else
							amount = 0;
						Transform p;
						p = repairList [0].transform.parent;
						if (p != this.transform) {
							repairList [0].transform.SetParent (this.transform);
							//	repairList [0].SetActive (true);
							foreach (Renderer r in repairList [0].GetComponentsInChildren<Renderer> ())r.enabled = true;
							foreach (Collider c in repairList [0].GetComponentsInChildren<Collider> ())c.enabled = true;
							repairList [0].GetComponent<NetobjStateControl> ().returnStartPos ();
							repairList [0].GetComponent<NetobjStateControl> ().isBreak = false;
							GetComponent<Rigidbody> ().ResetCenterOfMass ();
						}
						if (healingPart.CurrentHp == healingPart.MaxHp)
							repairList.RemoveAt (0);
						if (p.childCount == 0 && p != this.transform)
							Destroy (p.gameObject);
					} while (amount > 0 && repairList.Count != 0);
				}
			}
		}
	}


    public void BreakCheck(GameObject breakingPoint)
    {
		autorepaircooltime = Time.time;
        currentBodyCount--;
        //make 6 list
        GameObject ph, ph1, ph2;
        int nowMax = 0;
        List<List<GameObject>> alldir = new List<List<GameObject>>();
        Stack<GameObject> procstack = new Stack<GameObject>();
        for (int i = 0; i < 6; i++)
        {
            List<GameObject> temp = new List<GameObject>();
            alldir.Add(temp);
        }
        //loop through 6 direction
        for (int i = 0; i < 6; i++)
        {
            if ((ph = breakingPoint.GetComponent<NetobjStateControl>().getBodyDirection(i)) != null)
            {
				if (!ph.GetComponent<NetobjStateControl> ().isBreak) {
					ph.GetComponent<NetobjStateControl> ().isInStack = true;
					procstack.Push (ph);
					while (procstack.Count != 0) {
						ph1 = procstack.Pop ();
						alldir [i].Add (ph1);
						for (int j = 0; j < 6; j++) {
							int c = 0;
							if ((ph2 = ph1.GetComponent<NetobjStateControl> ().getBodyDirection (j)) != null ) {
								if (!ph2.GetComponent<NetobjStateControl> ().isInStack && !ph2.GetComponent<NetobjStateControl> ().isChecked && !ph2.GetComponent<NetobjStateControl> ().isBreak) {
									ph2.GetComponent<NetobjStateControl> ().isInStack = true;
									procstack.Push (ph2);
									c++;
								}
							}
						}

						ph1.GetComponent<NetobjStateControl> ().isChecked = true;
					}
					if (alldir [i].Count > nowMax)
						nowMax = alldir [i].Count;
					if (alldir [i].Count == currentBodyCount)
						break;
				}
            }
        }


        int equalcount = 0;
		bool breakingPointListed=false;
		for (int i = 0; i < 6; i++) {
			//	Debug.Log ("dir "+i+" "+alldir [i].Count);
			if (alldir [i].Count != nowMax && alldir [i].Count != 0 || (alldir [i].Count == nowMax && equalcount > 0)) {
				//Debug.Log ("Breaking off dir "+i);
				//GameObject p = new GameObject ();
				//p.AddComponent<Rigidbody> ();
				for (int j = 0; j < alldir [i].Count; j++) {
					//  ph = GameObject.Instantiate(alldir[i][j], alldir[i][j].transform.position, alldir[i][j].transform.rotation);
					//alldir [i] [j].transform.SetParent (p.transform);
					currentHp -= alldir [i] [j].GetComponent<NetobjStateControl> ().CurrentHp;
					alldir [i] [j].GetComponent<NetobjStateControl> ().isBreak = true;
					alldir [i] [j].GetComponent<NetobjStateControl> ().CurrentHp = 0;//網路需求
					//alldir[i][j].SetActive(false);
					currentBodyCount--;
				}
				/*if (!breakingPointListed) {
					breakingPoint.transform.SetParent (p.transform);
					breakingPointListed = true;
					// Destroy(p, 3f);
					//p.AddComponent<autodeactive> ();
				}*/
				if (alldir [i].Count == nowMax)
					equalcount++;
			}
		}

        for (int i = 0; i < bodyList.Count; i++)
        {
            bodyList[i].GetComponent<NetobjStateControl>().isChecked = false;
            bodyList[i].GetComponent<NetobjStateControl>().isInStack = false;
        }
			/*if (!breakingPointListed) {
				GameObject p = new GameObject ();
				p.AddComponent<Rigidbody> ();
				breakingPoint.transform.SetParent (p.transform);
				breakingPointListed = true;
			}*/
			repairList.Add (breakingPoint);
			GetComponent<Rigidbody> ().ResetCenterOfMass ();
        //breakingPoint.SetActive(false);
		foreach(Renderer r in breakingPoint.GetComponentsInChildren<Renderer>())r.enabled=false;
		foreach(Collider c in breakingPoint.GetComponentsInChildren<Collider>())c.enabled = false;
		if (currentHp <= 0.1&&!isDead) {
			Debug.Log ("M dead check");
			isDead = true;
			gameObject.GetComponent<Rigidbody> ().isKinematic = true;
			gameObject.GetComponent<Rigidbody> ().useGravity = false;
			repairList.Clear ();
			player.OnDeath ();
			StartCoroutine (waitForRespawn ());
		}
    }
	void OnDeadStateChanged(bool state)
	{
		Debug.Log ("syncing dead state");
		isDead = state;
	}

	void OnHpChanged(float hp)
	{
		Debug.Log ("syncing hp");
		currentHp = hp;
		if (currentHp <= 0.1 && !isDead) {
			isDead = true;
		}
	}
	public IEnumerator waitForRespawn()
	{
		yield return new WaitForSeconds (10f);
		foreach (GameObject g in bodyList) {
			foreach(Renderer r in g.GetComponentsInChildren<Renderer>())r.enabled=true;
			foreach(Collider c in g.GetComponentsInChildren<Collider>())c.enabled = true;
			g.GetComponent<NetobjStateControl> ().CurrentHp = g.GetComponent<NetobjStateControl> ().MaxHp;
			g.GetComponent<NetobjStateControl> ().isBreak = false;
		}
		isDead = false;
		gameObject.GetComponent<Rigidbody> ().isKinematic = false;
		gameObject.GetComponent<Rigidbody> ().useGravity = true;
		currentHp = totalHp;
		if(player.isLocalPlayer)
		player.Respawn ();
		else if(player.isServer)
		player.RpcRespawn ();
	}

}
