using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameMode : NetworkBehaviour {
	static public List<NetPlayerController> sPlayers = new List<NetPlayerController>();
	static public GameMode sInstance = null;
	public Text scoreTextRed, scoreTextBlue,resultText,winConditionText;
	public int mode = 1;
	public GameObject result;
	[SyncVar(hook ="OnRedGetScore")]
	public int scoreRed = 0;

	[SyncVar(hook ="OnBlueGetScore")]
	public int scoreBlue = 0;

	public int respawnTimesBlue = 0;

	public int respawnTimesRed = 0;

	public int	scoreWin=15;
	public NetPlayerController localplayer;
	public GameObject spawnPointRed, spawnPointBlue,flagSpawnPointRed,flagSpawnPointBlue;
	public GameObject flagRed, flagBlue,win,lose;
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
		winConditionText.text = scoreWin.ToString();
		foreach (NetPlayerController p in sPlayers)
			if (p.isLocalPlayer) {
				localplayer = p;
				break;
			}
		if (!localplayer&&mode==1) {
			GameObject flag_r = Instantiate (flagRed, flagSpawnPointRed.transform.position+Vector3.up, Quaternion.identity);
			GameObject flag_b =Instantiate (flagBlue, flagSpawnPointBlue.transform.position+Vector3.up, Quaternion.identity);
			NetworkServer.Spawn (flag_r);
			NetworkServer.Spawn (flag_b);
			flagSpawnPointRed.GetComponent<FlagGoal> ().init ();
			flagSpawnPointBlue.GetComponent<FlagGoal> ().init ();
		}
	}
		
	/*public void SpawnFlag()
	{
		Debug.Log ("spawning flag");
		CmdSpawnFlag ();
		flagSpawnPointRed.GetComponent<FlagGoal> ().init ();
		flagSpawnPointBlue.GetComponent<FlagGoal> ().init ();
	}
	[Command]
	public void CmdSpawnFlag()
	{
		Debug.Log ("CMDspawning flag");
		GameObject flag_r = Instantiate (flagRed, flagSpawnPointRed.transform.position, Quaternion.identity);
		GameObject flag_b =Instantiate (flagBlue, flagSpawnPointBlue.transform.position, Quaternion.identity);
		NetworkServer.Spawn (flag_r);
		NetworkServer.Spawn (flag_b);
		flagSpawnPointRed.GetComponent<FlagGoal> ().init ();
		flagSpawnPointBlue.GetComponent<FlagGoal> ().init ();
	}*/

	public void respawnPlayer(NetPlayerController player)
	{
		if (player.isLocalPlayer)
			Debug.Log ("local respawn");
		if(player.isClient)
			Debug.Log ("client respawn");
		if(player.isServer)
			Debug.Log ("server respawn");
		player.main.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		player.main.isDead = false;
		if (player.Team == 0) {
			player.main.gameObject.transform.position = spawnPointBlue.transform.GetChild (player.respawnPos).position + Vector3.up * 5;
		} else {
			player.main.gameObject.transform.position = spawnPointRed.transform.GetChild (player.respawnPos).position + Vector3.up * 5;
		}
		Debug.Log (player.playerName+" Respawned");
	}
	// Update is called once per frame
	void Update () {
		
	}
	void OnRedGetScore(int newScore)
	{
		scoreRed = newScore;
		if (scoreRed > scoreWin)
			scoreRed = scoreWin;
		updateScore ();
	}
	void OnBlueGetScore(int newScore)
	{
		scoreBlue = newScore;
		if (scoreBlue > scoreWin)
			scoreBlue = scoreWin;
		updateScore ();
	}
	public void calcscore(int team,int score)
	{
			Debug.Log ("calc score "+ team);
			switch (team) {
			case 0:
			scoreBlue+=score;
				break;
			case 1:
			scoreRed+=score;
				break;
		}
		updateScore ();
		if (!isServer)
			CmdScoreChanged (scoreBlue, scoreRed);
		if (isServer) {
			foreach (NetPlayerController p in sPlayers)
				p.RpcUpdateScore ();
			if (scoreRed >= scoreWin) {
				foreach (NetPlayerController p in sPlayers)
					p.RpcTeamRedWin ();
				StartCoroutine (backToLobby ());
			}
			if (scoreBlue >= scoreWin) {
				foreach (NetPlayerController p in sPlayers)
					p.RpcTeamBlueWin ();
				StartCoroutine (backToLobby ());
			}
		}
	}

	[Command]
	void CmdScoreChanged(int blue,int red)
	{
		scoreBlue = blue;
		scoreRed = red;
		updateScore ();
		RpcScoreChanged (blue, red);
	}

	[ClientRpc]
	void RpcScoreChanged(int blue,int red)
	{
		scoreBlue = blue;
		scoreRed = red;
		updateScore ();
	}

	public void updateScore()
	{
		Debug.Log ("update score");
		scoreTextRed.text = scoreRed.ToString();
		scoreTextBlue.text = scoreBlue.ToString();
	}

	public void teamRedWin()
	{
		if (localplayer.Team == 1) {
			win.transform.SetParent (null);
			win.GetComponent<Canvas> ().worldCamera = Camera.current;
			win.SetActive (true);
		} else {
			lose.transform.SetParent (null);
			lose.GetComponent<Canvas> ().worldCamera = Camera.current;
			lose.SetActive (true);
		}
	}

	public void teamBlueWin()
	{
		if (localplayer.Team == 1) {
			lose.transform.SetParent (null);
			lose.GetComponent<Canvas> ().worldCamera = Camera.main;
			lose.SetActive (true);
		} else {
			win.transform.SetParent (null);
			win.GetComponent<Canvas> ().worldCamera = Camera.main;
			win.SetActive (true);
		}
	}

	public IEnumerator backToLobby()
	{
		yield return new WaitForSeconds (10f);
		LobbyManager.s_Singleton.GoBackButton();

	}
}
