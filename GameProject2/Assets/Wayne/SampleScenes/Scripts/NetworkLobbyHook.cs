using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook 
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
		NetPlayerController game = gamePlayer.GetComponent<NetPlayerController> ();
        //NetworkSpaceship spaceship = gamePlayer.GetComponent<NetworkSpaceship>();
        /*spaceship.name = lobby.name;
        spaceship.color = lobby.playerColor;
        spaceship.score = 0;
        spaceship.lifeCount = 3;*/
		game.bodycount = lobby.bodycount;
		game.Team = lobby.team;
		game.playerName = lobby.playerName;
		game.respawnPos = lobby.respawnPos;
		Debug.Log (game.playerName+" is "+(game.Team==1?"Red Team":"Blue Team"));
        /*string playerName = lobbyPlayer.GetComponent<LobbyPlayer>().playerName;
        gamePlayer.GetComponent<NetPlayerController>().playerName = playerName;


        if (playerName.Length > 10)//目前暫時使用名稱長度分隊
            gamePlayer.GetComponent<NetPlayerController>().Team = 1;
        else
            gamePlayer.GetComponent<NetPlayerController>().Team = 0;*/



        CreateMessage mb = new CreateMessage();
		mb.connectID = game.connectID;
		mb.networkID = (uint)game.connectID;//gamePlayer.GetComponent<NetworkIdentity>().netId.Value;
        print(mb.networkID);
		NetworkServer.SendToClient(game.connectID, RegisteFunction.SENDROBOT, mb);
    }
}
