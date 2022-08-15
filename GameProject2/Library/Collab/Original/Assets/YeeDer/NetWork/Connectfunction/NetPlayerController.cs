using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
	void Awake()
	{
		
		if (FindObjectOfType<GameMode> ()) {
			Debug.Log ("mode found");
			GameMode.sPlayers.Add (this);
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
		StartCoroutine (firstSpawn ());
    }
	IEnumerator firstSpawn()
	{
		yield return new WaitForSeconds(1f);
		Respawn ();
	}

	public void Respawn()
	{
		Debug.Log ("Respawning "+playerName);
		GameMode.sInstance.respawnPlayer (this);
	}
	[ClientRpc]
	public void RpcRespawn()
	{
		Debug.Log (playerName + " respawned");
		GameMode.sInstance.respawnPlayer (this);
	}

	public void OnDeath()
	{
		Debug.Log (playerName + " died");
		GameMode.sInstance.calcscore (this);
		CmdOnDeath ();
	}
	[Command]
	public void CmdOnDeath()
	{
		Debug.Log (playerName + " died");
		GameMode.sInstance.calcscore (this);

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
