using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetHpDisplay : NetworkBehaviour
{
    List<GameObject> players = new List<GameObject>();
	public NetPlayerController player;
    public GameObject enemyhpbar, allyhpbar;
    // Use this for initialization
    void Start()
    {
		player = transform.root.GetComponent<NetPlayerController> ();
        if (transform.parent.GetComponent<NetworkIdentity>().isLocalPlayer)
        {//自己頭上不用加血條
            print("MEEEEEEEEEEEEEEEEEEEE");
        }
        else
        {
			AddHpBar(player);
            print("OOOOOOOOOOOther");
        }


    }

	public void AddHpBar(NetPlayerController player)//傳入的物件是player下Team的gameobject
    {
        GameObject hpbar;
		if(!player.isServer&&player.Team==GameMode.sInstance.localplayer.Team)
        hpbar = GameObject.Instantiate(allyhpbar, player.transform.position, player.transform.rotation);
		else
			hpbar = GameObject.Instantiate(enemyhpbar, player.transform.position, player.transform.rotation);
        hpbar.transform.parent = player.transform;
        hpbar.GetComponent<RectTransform>().localPosition = new Vector3(0, 2, 0);
    }
}
