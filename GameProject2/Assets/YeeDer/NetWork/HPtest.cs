using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class HPtest : NetworkBehaviour {

    [SyncVar]
    public int hp;

    void Start ()
    {
        hp = 100;
	}
	
	// Update is called once per frame
	void Update ()
    {
    
	}

    public void islife()
    {
        Debug.LogAssertion("scsc");
        print("scsc");
        hp -= 30;
        if (hp <= 0)
            GetComponent<Renderer>().enabled = false;
    }
}
