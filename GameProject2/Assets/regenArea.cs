using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*!!!this script needs a trigger!!!*/
[RequireComponent(typeof(Collider))]
public class regenArea : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.GetComponentInParent<objMainBodyStateControl> ()) {
			objMainBodyStateControl healingPlayer= other.gameObject.GetComponentInParent<objMainBodyStateControl> ();
			healingPlayer.healing (healingPlayer.totalHp / 20);
		}
	}
}
