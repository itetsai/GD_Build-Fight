using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetSpeedTest : NetworkBehaviour {

    [SyncVar]
    public float time;
    float lasttime;
    float t;
    void Start ()
    {
        t = 0;
        time = 0;
        lasttime = 0;
	}

    private void Update()
    {
        if (isServer)
            time += Time.deltaTime;

        if (isClient&&lasttime!=time&&time>4.0&&time<5.0)
        {
            print(time);
        }
        lasttime = time;
    }
}
