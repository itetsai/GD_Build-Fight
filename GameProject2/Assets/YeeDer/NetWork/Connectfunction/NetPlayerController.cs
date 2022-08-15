using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;
using UnityEngine.Networking.PlayerConnection;
using UnityEngine.UI;

public class NetPlayerController : NetworkBehaviour
{
    public bool local;
    [SyncVar]
    public int connectID;

    [SyncVar]
    public string playerName;

    [SyncVar(hook ="TeamCheck")]
    public int Team;//0是blue team //1是red team

	public NetobjMainBodyStateControl main;

	[SyncVar]
	public int respawnPos;

	[SyncVar]
	public int bodycount;
	void Awake()
	{
		
		if (FindObjectOfType<GameMode> ()) {
			Debug.Log ("mode found");
			GameMode.sPlayers.Add (this);
		}
		for (int i = 0; i < LobbyManager.s_Singleton.lobbySlots.Length; i++) {
			if (LobbyManager.s_Singleton.lobbySlots [i] != null) {
				if (((LobbyManager.s_Singleton.lobbySlots [i]) as LobbyPlayer).playerName==playerName) {
					respawnPos = ((LobbyManager.s_Singleton.lobbySlots [i]) as LobbyPlayer).respawnPos;
				}
			}
		}
	
	}

    void TeamCheck(int team)
    {
        Team = team;
        if (team == 0)
            transform.Find("Team").tag = "BlueTeam";
        else if (team == 1)
            transform.Find("Team").tag = "RedTeam";
    }


    void Start ()
    {
        if (Team == 0)
            transform.Find("Team").tag = "BlueTeam";
        else if (Team == 1)
            transform.Find("Team").tag = "RedTeam";
		int redCount=0, blueCount=0;
		for (int i = 0; i < GameMode.sPlayers.Count; i++) {
			if (GameMode.sPlayers [i].Team == 0)
				blueCount++;
			else
				redCount++;
		}
		if (Team == 0) {
			GameMode.sInstance.respawnTimesBlue = blueCount - 1;
		} else {
			GameMode.sInstance.respawnTimesRed = redCount - 1;
		}
        local = false;
        if (!isLocalPlayer)
        {
            GameObject g = transform.Find("Pivot").gameObject;
            g.SetActive(false);
            GetComponent<FreeLookCam>().enabled = false;
            GetComponent<ProtectCameraFromWallClip>().enabled = false;
        }
        else
        {
            local = true;
            RadarScript.MyPlayer = transform.Find("Team").gameObject;
        }

		/*if (isLocalPlayer) {
			Debug.Log (playerName + " player count = " + GameMode.sPlayers.Count + " lobbyplayer count = " + LobbyManager.s_Singleton._playerNumber);
			if ((GameMode.sPlayers.Count == LobbyManager.s_Singleton._playerNumber) && isLocalPlayer&&GameMode.sInstance.mode==1) {
				Debug.Log ("spawning flag");
				GameMode.sInstance.SpawnFlag ();
			}
		}*/

		StartCoroutine (firstSpawn ());
    }
	IEnumerator firstSpawn()
	{
		yield return new WaitForSeconds(4f);
		Respawn ();
		if (local) {
			foreach (GameObject g in main.bodyList) {
				if (g.GetComponent<NetobjStateControl> ().controllable) {
					foreach (Transform t in g.GetComponentsInChildren<Transform>()) {
						t.gameObject.layer = 11;
					}
				}
			}
			main.WC.enabled = true;
			main.hpbar.localplayer = this;
			main.hpbar.init ();
		}

	}

	public void Respawn()
	{
		Debug.Log ("Respawning "+playerName);
		if (isLocalPlayer) {
			GameMode.sInstance.respawnPlayer (this);
			main.WC.reset ();
		}
		else if(isServer)
			RpcRespawn ();
	}
	[ClientRpc]
	public void RpcRespawn()
	{
		Debug.Log ("RPCRespawning "+playerName);
		GameMode.sInstance.respawnPlayer (this);
	}

	public void OnDeath()
	{
		Debug.Log (playerName + " died");
		GameMode.sInstance.calcscore ((Team==1?0:1),1);
	}
	public void FlagGoal()
	{
		GameMode.sInstance.calcscore (Team,10);
		CmdFlagGoal();

	}
	[Command]
	void CmdFlagGoal()
	{
		main.flag.GoalBack ();
		GameMode.sInstance.calcscore (Team,10);
	}
	[ClientRpc]
	public void RpcUpdateScore()
	{
		GameMode.sInstance.updateScore ();

	}

	[ClientRpc]
	public void RpcTeamRedWin()
	{
		GameMode.sInstance.teamRedWin ();

	}

	[ClientRpc]
	public void RpcTeamBlueWin()
	{
		GameMode.sInstance.teamBlueWin ();

	}
}
