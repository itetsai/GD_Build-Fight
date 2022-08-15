using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Netbullet : NetworkBehaviour {
    public float autodestroytime = 5;
	public NetPlayerController fromPlayer;
    public int damage = 10;
	public bool burn = false;
    //RaycastHit hitinfo;
    void Start()
    {
        Destroy(gameObject, autodestroytime);
    }
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
		print (other.gameObject.name + " layer= " + other.gameObject.layer);
		NetobjStateControl findobjStateControl;
		if (findobjStateControl = other.gameObject.GetComponentInParent<NetobjStateControl> ())
		if ((findobjStateControl.main.player == fromPlayer)&&(findobjStateControl.gameObject.GetComponent<ObjectName>().kind=="gun"))
			return;
		if (other.gameObject.layer != 11) {
			if (findobjStateControl = other.gameObject.GetComponentInParent<NetobjStateControl> ()) {
				if (findobjStateControl.team != fromPlayer.Team) {
					findobjStateControl.OnDamage (damage);
					if (burn)
						findobjStateControl.startBurn ();
				}
			}
			Destroy (gameObject);
		}
    }

}
