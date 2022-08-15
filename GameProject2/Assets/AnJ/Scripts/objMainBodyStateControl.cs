using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objMainBodyStateControl : MonoBehaviour {
    static public float maxX, maxY, maxZ, minX, minY, minZ;
	int maxBodyCount,currentBodyCount;
	List<GameObject> bodyList=new List<GameObject>();
	List<GameObject> weaponList=new List<GameObject>();
	[HideInInspector]
	public List<GameObject> repairList= new List<GameObject>();
	double autorepaircooltime=0,autorepairsuspend=0;
	public float totalHp=0,currentHp;
	// Use this for initialization
	void Start () {
        int count;
        count = this.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject temp = this.transform.GetChild(i).gameObject;
            Debug.Log(temp.name);
            if (temp.GetComponent<FireBullet>())
                temp.GetComponent<objStateControl>().bodyType = 1;
            if (temp.GetComponent<FireRocket>())
                temp.GetComponent<objStateControl>().bodyType = 2;
            if (temp.GetComponent<FireShell>())
                temp.GetComponent<objStateControl>().bodyType = 3;
            if (temp.GetComponent<FireLaser>())
                temp.GetComponent<objStateControl>().bodyType = 4;
            if (temp.transform.localPosition.x > maxX)
                maxX = temp.transform.localPosition.x;
            if (temp.transform.localPosition.y > maxY)
                maxY = temp.transform.localPosition.y;
            if (temp.transform.localPosition.z > maxZ)
                maxZ = temp.transform.localPosition.z;
            if (temp.transform.localPosition.x < minX)
                minX = temp.transform.localPosition.x;
            if (temp.transform.localPosition.y < minY)
                minY = temp.transform.localPosition.y;
            if (temp.transform.localPosition.z < minZ)
                maxZ = temp.transform.localPosition.z;
            temp.GetComponent<objStateControl>().getSurroundingState();
            totalHp += temp.GetComponent<objStateControl>().MaxHp;
            bodyList.Add(temp);
            if (temp.GetComponent<objStateControl>().bodyType != 0)
                weaponList.Add(temp);
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			for (int i = 0; i < weaponList.Count; i++) {
				switch (weaponList [i].GetComponent<objStateControl> ().bodyType) {
				case 1:
					weaponList [i].GetComponent<FireBullet> ().Fire ();
					break;
				case 2:
					weaponList [i].GetComponent<FireRocket> ().Fire ();
					break;
				case 3:
					weaponList [i].GetComponent<FireShell> ().Fire ();
					break;
				case 4:
					weaponList [i].GetComponent<FireLaser> ().Fire ();
					break;
				}	
			}
		}
		//Debug.Log ("nowtime-autorepaircooltime=" + (Time.time-autorepaircooltime) + " repairList count=" + repairList.Count);
		//AutoHpRegen
		/*if ((Time.time - autorepaircooltime) >= 10&&repairList.Count!=0) {
			if ((Time.time - autorepairsuspend) >= 0.25) {
				Transform p;
				p = repairList [0].transform.parent;
				repairList [0].transform.SetParent (this.transform);
				repairList[0].SetActive (true);
				repairList [0].GetComponent<objStateControl> ().returnStartPos ();
				repairList[0].GetComponent<objStateControl> ().isBreak = false;
				repairList.RemoveAt (0);
				if (p.childCount == 0)
					Destroy (p.gameObject);
				GetComponent<Rigidbody> ().ResetCenterOfMass ();
				autorepairsuspend = Time.time;
			}
		}*/
	}
	public void healing(float amount)
	{
		if(repairList.Count!=0){
			if ((Time.time - autorepairsuspend) >= 1) {
				autorepairsuspend = Time.time;
				do{
					
					objStateControl healingPart = repairList [0].GetComponent<objStateControl> ();
				//	Debug.Log("Healing "+repairList[0].name+" for "+amount);
					healingPart.CurrentHp+=amount;
					currentHp+=amount;
					if(healingPart.CurrentHp>healingPart.MaxHp){
						amount=healingPart.CurrentHp-healingPart.MaxHp;
						currentHp-=amount;
						healingPart.CurrentHp=healingPart.MaxHp;
						}
					else
						amount=0;
					Transform p;
					p = repairList [0].transform.parent;
					if(p!=this.transform)
					{
					repairList [0].transform.SetParent (this.transform);
					//repairList [0].SetActive (true);
					List<Renderer> rends=new List<Renderer>();
					List<Collider> collds=new List<Collider>();
					rends.AddRange(repairList [0].GetComponentsInChildren<Renderer> ());
					collds.AddRange(repairList [0].GetComponentsInChildren<Collider> ());
					foreach (Renderer r in rends)r.enabled = true;
					foreach (Collider r in collds)r.enabled = true;
					repairList [0].GetComponent<objStateControl> ().returnStartPos ();
					repairList [0].GetComponent<objStateControl> ().isBreak = false;
					GetComponent<Rigidbody> ().ResetCenterOfMass ();
					}
					if(healingPart.CurrentHp==healingPart.MaxHp)
					repairList.RemoveAt (0);
					if (p.childCount == 0&&p!=this.transform)
						Destroy (p.gameObject);
				}while (amount > 0&&repairList.Count!=0);
			}
		}
	}
	public void BreakCheck(GameObject breakingPoint)
	{
		autorepaircooltime = Time.time;
		currentBodyCount--;
		//make 6 list
		GameObject ph,ph1,ph2;
		int nowMax=0;
		List<List<GameObject>> alldir=new List<List<GameObject>>();
		Stack<GameObject> procstack=new Stack<GameObject>();
		for (int i = 0; i < 6; i++) {
			List<GameObject> temp=new List<GameObject>();
			alldir.Add(temp);
		}
		//loop through 6 direction
		for (int i = 5; i >=0; i--) {
			if ((ph = breakingPoint.GetComponent<objStateControl> ().getBodyDirection (i)) != null&&!ph.GetComponent<objStateControl>().isBreak) {
				ph.GetComponent<objStateControl> ().isInStack = true;
				procstack.Push (ph);
				while (procstack.Count != 0) {
					ph1 = procstack.Pop ();
					alldir [i].Add (ph1);
					for (int j = 5; j >=0; j--) {
						int c=0;
						//Debug.Log ("Checking dir " + j + " in dir " + i);
						if ((ph2 = ph1.GetComponent<objStateControl> ().getBodyDirection (j)) != null && !ph2.GetComponent<objStateControl> ().isInStack && !ph2.GetComponent<objStateControl> ().isChecked && !ph2.GetComponent<objStateControl> ().isBreak) {
							ph2.GetComponent<objStateControl> ().isInStack = true;
							procstack.Push (ph2);
							c++;
						}
						//Debug.Log ("Find " + c +" when checking dir "+j+ " in dir " + i);
					}

					ph1.GetComponent<objStateControl> ().isChecked = true;
				}
				if (alldir [i].Count > nowMax)
					nowMax = alldir [i].Count;
				if (alldir [i].Count == currentBodyCount)
					break;
			}
		}
		//break off every part except biggest one
	//	Debug.Log ("Start breaking off");
		//Debug.Log ("nowMax= " + nowMax);
		int equalcount=0;
		bool breakingPointListed=false;
		for (int i = 5; i >=0; i--) {
		//	Debug.Log ("dir "+i+" "+alldir [i].Count);
			if (alldir [i].Count != nowMax&&alldir [i].Count!=0||(alldir [i].Count == nowMax&&equalcount>0)) {
				//Debug.Log ("Breaking off dir "+i);
				GameObject p = new GameObject ();
				p.AddComponent<Rigidbody> ();
				for (int j = 0; j < alldir [i].Count; j++) {
					//ph = GameObject.Instantiate (alldir [i] [j], alldir [i] [j].transform.position, alldir [i] [j].transform.rotation);
					alldir [i] [j].transform.SetParent(p.transform);
					currentHp -= alldir [i] [j].GetComponent<objStateControl> ().CurrentHp;
					alldir [i] [j].GetComponent<objStateControl> ().CurrentHp=0;
					alldir [i] [j].GetComponent<objStateControl> ().isBreak = true;
				//	alldir [i] [j].SetActive (false);
					repairList.Add (alldir [i] [j]);

					currentBodyCount--;
				}
				if (!breakingPointListed) {
					breakingPoint.transform.SetParent (p.transform);
					breakingPointListed = true;
				}
				p.AddComponent<autodeactive> ();
				//Destroy (p, 3f);
			}
			if (alldir [i].Count == nowMax)equalcount++;		
		}
		//clear flags
		for (int i = 0; i < bodyList.Count; i++) {
			bodyList [i].GetComponent<objStateControl> ().isChecked = false;
			bodyList [i].GetComponent<objStateControl> ().isInStack = false;
		}
		if (!breakingPointListed) {
			GameObject p = new GameObject ();
			p.AddComponent<Rigidbody> ();
			breakingPoint.transform.SetParent (p.transform);
			breakingPointListed = true;
		}
		//breakingPoint.SetActive (false);
		foreach(Renderer r in breakingPoint.GetComponentsInChildren<Renderer>())r.enabled=false;
		foreach(Collider c in breakingPoint.GetComponentsInChildren<Collider>())c.enabled = false;
		repairList.Add (breakingPoint);
		GetComponent<Rigidbody> ().ResetCenterOfMass ();
	}
}
