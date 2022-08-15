using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameMode : NetworkBehaviour {
	static public List<NetPlayerController> sPlayers = new List<NetPlayerController>();
	static public GameMode sInstance = null;
	public Text scoreTextRed, scoreTextBlue,resultText;
	public int mode = 0;
	public GameObject result;
	[SyncVar(hook ="OnRedGetScore")]
	public int scoreRed = 0;
	[SyncVar(hook ="OnBlueGetScore")]
	public int scoreBlue = 0;
	[SyncVar(hook ="OnBlueRespawnTime")]
	public int respawnTimesBlue = 0;
	[SyncVar(hook ="OnRedRespawnTime")]
	public int respawnTimesRed = 0;
	public int	scoreWin=15;
	public NetPlayerController localplayer;
	public GameObject spawnPointRed, spawnPointBlue,flagSpawnPointRed,flagSpawnPointBlue;
	public GameObject flagRed, flagBlue;
	void Awake()
	{
		sInstance = this;
		spawnPointRed = GameObject.FindGameObjectWithTag ("redSpawn");
		spawnPointBlue = GameObject.FindGameObjectWithTag ("blueSpawn");
		if (mode == 1) {
			flagSpawnPointRed = spawnPointRed.transform.GetChild (5).gameObject;
			flagSpawnPointBlue = spawnPointBlue.transform.GetChild (5).gameObject;
		}
	}
	// Use this for initialization
	void Start () {
		foreach (NetPlayerController p in sPlayers)
			if (p.isLocalPlayer) {
				localplayer = p;
				break;
			}
	}
	IEnumerator spawn()
	{
		int teamRedNumber=0, teamBlueNumber=0;
		yield return new WaitForSeconds (0.5f);
		for (int i = 0; i < sPlayers.Count; i++) {
			if (sPlayers [i].Team == 1) {
				sPlayers [i].main.gameObject.transform.position = spawnPointRed.transform.GetChild (teamRedNumber).position+transform.up*5;
				teamRedNumber++;
			} else {
				sPlayers [i].main.gameObject.transform.position = spawnPointBlue.transform.GetChild (teamRedNumber).position+transform.up*5;
				teamBlueNumber++;
			}
		}
	}

	public void respawnPlayer(NetPlayerController player)
	{
		if (player.Team == 0) {
			player.main.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			player.main.gameObject.transform.position = spawnPointBlue.transform.GetChild (respawnTimesBlue).position + Vector3.up * 5;
			respawnTimesBlue++;
			respawnTimesBlue %= 5;
			if(!player.isServer)
			CmdRespawnTimeBlueChanged ();
		} else {
			player.main.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			player.main.gameObject.transform.position = spawnPointRed.transform.GetChild (respawnTimesRed).position + Vector3.up * 5;
			respawnTimesRed++;
			respawnTimesRed %= 5;
			if(!player.isServer)
			CmdRespawnTimeRedChanged ();
		}
		Debug.Log (player.playerName+" Respawned");
	}
	[Command]
	void CmdRespawnTimeBlueChanged()
	{
		respawnTimesBlue=respawnTimesBlue+1;

	}

	[Command]
	void CmdRespawnTimeRedChanged()
	{
		respawnTimesRed=respawnTimesRed+1;

	}
	void OnRedRespawnTime(int times)
	{
		respawnTimesRed = times%5;

	}

	void OnBlueRespawnTime(int times)
	{
		respawnTimesBlue = times%5;

	}
	// Update is called once per frame
	void Update () {
		
	}
	void OnRedGetScore(int newScore)
	{
		scoreRed = newScore;
	}
	void OnBlueGetScore(int newScore)
	{
		scoreBlue = newScore;
	}
	public void calcscore(NetPlayerController player)
	{
		Debug.Log ("calc score");
		switch (player.Team) {
		case 0:
			scoreRed++;
			break;
		case 1:
			scoreBlue++;
			break;
		}
		foreach(NetPlayerController p in sPlayers)
			p.RpcUpdateScore();
		if (scoreRed >= scoreWin)
			foreach(NetPlayerController p in sPlayers)
				p.RpcTeamRedWin();
		if (scoreBlue >= scoreWin)
			foreach(NetPlayerController p in sPlayers)
				p.RpcTeamBlueWin();
	}

	public void updateScore()
	{
		Debug.Log ("update score");
		scoreTextRed.text = scoreRed.ToString();
		scoreTextBlue.text = scoreBlue.ToString();
	}

	public void teamRedWin()
	{
		resultText.text="Team Red Won!";
		result.SetActive (true);
	}

	public void teamBlueWin()
	{
		resultText.text="Team Blue Won!";
		result.SetActive (true);
	}
}
