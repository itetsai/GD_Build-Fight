using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objStateControl : MonoBehaviour {

	public float MaxHp=100,CurrentHp;
	public GameObject right=null,left=null,up=null,down=null,forward=null,back=null;
	public int bodyType=0;
	public GameObject breakBurst,breakWave;
	public bool isChecked=false,isInStack=false,isBreak=false;
	public bool onFire=false;
	objMainBodyStateControl main;
	Vector3 startPos;
	Quaternion startRot;
    CubePropertyData Cp;

	// Use this for initialization
	void Start () {
		main = gameObject.GetComponentInParent<objMainBodyStateControl> ();
		CurrentHp = MaxHp;
		//Debug.Log (transform.root.gameObject.name);
		//this.transform.localPosition = new Vector3 (this.transform.localPosition.x - 12.5f, this.transform.localPosition.y, this.transform.localPosition.z - 12.5f);
		startPos = this.transform.localPosition;
		startRot = this.transform.localRotation;
		//Debug.Log ("x="+startPos.x+" y="+ startPos.y+" z="+startPos.z);
		breakBurst = Resources.Load ("Effects/Lights_Burst") as GameObject;
		breakWave = Resources.Load ("Effects/Wave") as GameObject;
	}

	// Update is called once per frame
	void Update () {

	}
	public void getSurroundingState()
	{
		Transform hitgameobject=transform;
		RaycastHit hitinfo;
		while (Physics.Raycast(hitgameobject.position, Vector3.up, out hitinfo, 1f))
		{
			hitgameobject = hitinfo.collider.transform;
			if (hitgameobject.root == transform.root && !(hitgameobject.IsChildOf (transform) ||(hitgameobject == transform))) {
				up = hitgameobject.gameObject.GetComponentInParent<objStateControl> ().gameObject;
				hitgameobject = transform;
				break;
			} else if (hitgameobject.root != transform.root)
				break;
		}
		while (Physics.Raycast(hitgameobject.position, Vector3.down, out hitinfo, 1f))
		{
			hitgameobject = hitinfo.collider.transform;
			if (hitgameobject.root == transform.root && !(hitgameobject.IsChildOf (transform)||(hitgameobject==transform))) {
				down = hitgameobject.gameObject.GetComponentInParent<objStateControl> ().gameObject;
				hitgameobject = transform;
				break;
			}else if (hitgameobject.root != transform.root)
				break;
		}
		while (Physics.Raycast(hitgameobject.position, Vector3.right, out hitinfo, 1f))
		{
			hitgameobject = hitinfo.collider.transform;
			if (hitgameobject.root == transform.root && !(hitgameobject.IsChildOf (transform)||(hitgameobject==transform))) {
				right = hitgameobject.gameObject.GetComponentInParent<objStateControl> ().gameObject;
				hitgameobject = transform;
				break;
			}else if (hitgameobject.root != transform.root)
				break;
		}
		while (Physics.Raycast(hitgameobject.position, Vector3.left, out hitinfo, 1f))
		{
			hitgameobject = hitinfo.collider.transform;
			if (hitgameobject.root == transform.root && !(hitgameobject.IsChildOf (transform)||(hitgameobject==transform))) {
				left = hitgameobject.gameObject.GetComponentInParent<objStateControl> ().gameObject;
				hitgameobject = transform;
				break;
			}else if (hitgameobject.root != transform.root)
				break;
		}
		while (Physics.Raycast(hitgameobject.position, Vector3.forward, out hitinfo, 1f))
		{
			hitgameobject = hitinfo.collider.transform;
			if (hitgameobject.root == transform.root && !(hitgameobject.IsChildOf (transform)||(hitgameobject==transform))) {
				forward = hitgameobject.gameObject.GetComponentInParent<objStateControl> ().gameObject;
				hitgameobject = transform;
				break;
			}else if (hitgameobject.root != transform.root)
				break;
		}
		while (Physics.Raycast(hitgameobject.position, Vector3.back, out hitinfo, 1f))
		{
			hitgameobject = hitinfo.collider.transform;
			if (hitgameobject.root == transform.root && !(hitgameobject.IsChildOf (transform)||(hitgameobject==transform))) {
				back = hitgameobject.gameObject.GetComponentInParent<objStateControl> ().gameObject;
				hitgameobject = transform;
				break;
			}else if (hitgameobject.root != transform.root)
				break;
		}
	}
	public void OnDamage(float damage)
	{
		if (CurrentHp > 0) {
			CurrentHp -= damage;
			main.currentHp -= damage;
			main.repairList.Add (this.gameObject);
		}
			Debug.Log (name + " HP: " + CurrentHp);
		if (CurrentHp <= 0&&!isBreak) {
				main.currentHp -= CurrentHp;
				CurrentHp = 0;
				isBreak = true;
				OnBreak ();
			}
	}
	public void OnBreak()
	{
		if (isBreak) {
			GameObject breakburst = GameObject.Instantiate (breakBurst, this.transform.position, this.transform.rotation);
			GameObject breakwave = GameObject.Instantiate (breakWave, this.transform.position, this.transform.rotation);
			breakburst.GetComponent<ParticleSystem> ().Play ();
			breakwave.GetComponent<ParticleSystem> ().Play ();
			ParticleSystem.MainModule breakburst_mainModule = breakburst.GetComponent<ParticleSystem> ().main;
			ParticleSystem.MainModule breakwave_mainModule = breakwave.GetComponent<ParticleSystem> ().main;
			Destroy (breakwave.gameObject, breakwave_mainModule.duration / 3);
			Destroy (breakburst.gameObject, breakburst_mainModule.duration);
			main.BreakCheck (this.gameObject);
		} else {
			returnStartPos ();
		}
	}
	public void startBurn()
	{
		onFire = true;
		StartCoroutine (burn());
	}
	IEnumerator burn()
	{
		float burnDamage = MaxHp / 20;
		for (int i = 0; i < 6; i++) {
			if (onFire) {
				yield return new WaitForSeconds (1f);
				OnDamage(burnDamage);
			}
		}
		onFire = false;
	}
	//up=0,down=1,right=2,left=3,forward=4,back=5
	public GameObject getBodyDirection(int dir)
	{
		switch (dir) {
		case 0:
			return up;
		case 1:
			return down;
		case 2:
			return right;
		case 3:
			return left;
		case 4:
			return forward;
		case 5:
			return back;	
		}
		return null;
	}
	public GameObject getUp()
	{
		return up;
	}
	public GameObject getDown()
	{
		return down;
	}
	public GameObject getRight()
	{
		return right;
	}
	public GameObject getLeft()
	{
		return left;
	}
	public GameObject getForward()
	{
		return forward;
	}
	public GameObject getBack()
	{
		return back;
	}
	public void returnStartPos()
	{
		this.transform.localPosition = startPos;
		this.transform.localRotation = startRot;
	}
}
