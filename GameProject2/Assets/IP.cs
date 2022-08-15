using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class IP : NetworkBehaviour {
	public Text ip;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		ip.text = LobbyManager.s_Singleton.networkAddress;
	}
}
