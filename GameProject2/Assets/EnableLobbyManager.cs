using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableLobbyManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject g=GameObject.Find("LobbyManager");
		g.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
