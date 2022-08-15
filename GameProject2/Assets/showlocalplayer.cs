using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class showlocalplayer : NetworkBehaviour {
	Text t;
	// Use this for initialization
	void Start () {
		t = GetComponent<Text> ();
		StartCoroutine (getlocalplayer());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	IEnumerator getlocalplayer()
	{
		for(int i=0;i<10;i++) {
			yield return new WaitForSeconds (0.5f);
			t.text=GameMode.sInstance.localplayer.playerName;
			if (GameMode.sInstance.localplayer != null)
				break;
		}
	}
}
