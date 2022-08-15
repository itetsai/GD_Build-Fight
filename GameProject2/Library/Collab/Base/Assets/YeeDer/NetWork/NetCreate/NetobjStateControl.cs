using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetobjStateControl : NetworkBehaviour
{

    public float MaxHp = 100;
    [SyncVar(hook ="HpDetermination")]
    public float CurrentHp;
    public GameObject right = null, left = null, up = null, down = null, forward = null, back = null;
    public GameObject breakBurst, breakWave;
	public bool isChecked = false, isInStack = false;
	[SyncVar(hook ="OnBreakStateChanged")]
	public bool isBreak = false;
	public bool onFire=false;
	public NetobjMainBodyStateControl main;
	public int team;
	Vector3 startPos;
	Quaternion startRot;
    CubePropertyData Cp;
	deadcheck dead;

    void HpDetermination(float hp)
    {
		main.currentHp -= (CurrentHp - hp);
        CurrentHp = hp;
		if(CurrentHp<=0){
			main.currentHp -= CurrentHp;
			CurrentHp = 0;
			isBreak = true;
		}
		dead.Check ();
    }

	void OnBreakStateChanged(bool state)
	{
		isBreak = state;
		if (isBreak) {
			foreach (Renderer r in GetComponentsInChildren<Renderer>())
				r.enabled = false;
			foreach (Collider c in GetComponentsInChildren<Collider>())
				c.enabled = false;
		} else {
			foreach (Renderer r in GetComponentsInChildren<Renderer>())
				r.enabled = true;
			foreach (Collider c in GetComponentsInChildren<Collider>())
				c.enabled = true;
		}
	}
    void Start()
    {
		main = transform.root.GetComponentInChildren<NetobjMainBodyStateControl>();
		team = main.player.Team;
		dead = main.dead;
		startPos = this.transform.localPosition;
		startRot = this.transform.localRotation;
        CurrentHp = MaxHp;
        breakBurst = Resources.Load("Effects/Lights_Burst") as GameObject;
        breakWave = Resources.Load("Effects/Wave") as GameObject;

		if (!(isServer||isLocalPlayer))
            enabled = false;
    }

    public void getSurroundingState()
    {
		Transform hitgameobject=transform;
		RaycastHit hitinfo;
		while (Physics.Raycast(hitgameobject.position, Vector3.up, out hitinfo, 1f))
		{
			hitgameobject = hitinfo.collider.transform;
			if (hitgameobject.root == transform.root && !(hitgameobject.IsChildOf (transform) ||(hitgameobject == transform))) {
				up = hitgameobject.gameObject.GetComponentInParent<NetobjStateControl> ().gameObject;
				hitgameobject = transform;
				break;
			} else if (hitgameobject.root != transform.root)
				break;
		}
		while (Physics.Raycast(hitgameobject.position, Vector3.down, out hitinfo, 1f))
		{
			hitgameobject = hitinfo.collider.transform;
			if (hitgameobject.root == transform.root && !(hitgameobject.IsChildOf (transform)||(hitgameobject==transform))) {
				down = hitgameobject.gameObject.GetComponentInParent<NetobjStateControl> ().gameObject;
				hitgameobject = transform;
				break;
			}else if (hitgameobject.root != transform.root)
				break;
		}
		while (Physics.Raycast(hitgameobject.position, Vector3.right, out hitinfo, 1f))
		{
			hitgameobject = hitinfo.collider.transform;
			if (hitgameobject.root == transform.root && !(hitgameobject.IsChildOf (transform)||(hitgameobject==transform))) {
				right = hitgameobject.gameObject.GetComponentInParent<NetobjStateControl> ().gameObject;
				hitgameobject = transform;
				break;
			}else if (hitgameobject.root != transform.root)
				break;
		}
		while (Physics.Raycast(hitgameobject.position, Vector3.left, out hitinfo, 1f))
		{
			hitgameobject = hitinfo.collider.transform;
			if (hitgameobject.root == transform.root && !(hitgameobject.IsChildOf (transform)||(hitgameobject==transform))) {
				left = hitgameobject.gameObject.GetComponentInParent<NetobjStateControl> ().gameObject;
				hitgameobject = transform;
				break;
			}else if (hitgameobject.root != transform.root)
				break;
		}
		while (Physics.Raycast(hitgameobject.position, Vector3.forward, out hitinfo, 1f))
		{
			hitgameobject = hitinfo.collider.transform;
			if (hitgameobject.root == transform.root && !(hitgameobject.IsChildOf (transform)||(hitgameobject==transform))) {
				forward = hitgameobject.gameObject.GetComponentInParent<NetobjStateControl> ().gameObject;
				hitgameobject = transform;
				break;
			}else if (hitgameobject.root != transform.root)
				break;
		}
		while (Physics.Raycast(hitgameobject.position, Vector3.back, out hitinfo, 1f))
		{
			hitgameobject = hitinfo.collider.transform;
			if (hitgameobject.root == transform.root && !(hitgameobject.IsChildOf (transform)||(hitgameobject==transform))) {
				back = hitgameobject.gameObject.GetComponentInParent<NetobjStateControl> ().gameObject;
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
			if (main.repairList.Contains (this.gameObject))
			main.repairList.Add (this.gameObject);
		}
		Debug.Log (name + " HP: " + CurrentHp);
		if (CurrentHp <= 0) {
			main.currentHp -= CurrentHp;
			CurrentHp = 0;
			isBreak = true;
			OnBreak ();
		}
    }
    public void OnBreak()
    {
        GameObject breakburst = GameObject.Instantiate(breakBurst, this.transform.position, this.transform.rotation);
        GameObject breakwave = GameObject.Instantiate(breakWave, this.transform.position, this.transform.rotation);
        breakburst.GetComponent<ParticleSystem>().Play();
        breakwave.GetComponent<ParticleSystem>().Play();
        ParticleSystem.MainModule breakburst_mainModule = breakburst.GetComponent<ParticleSystem>().main;
        ParticleSystem.MainModule breakwave_mainModule = breakwave.GetComponent<ParticleSystem>().main;
        Destroy(breakwave.gameObject, breakwave_mainModule.duration / 3);
        Destroy(breakburst.gameObject, breakburst_mainModule.duration);
       main.BreakCheck(this.gameObject);
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
        switch (dir)
        {
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
