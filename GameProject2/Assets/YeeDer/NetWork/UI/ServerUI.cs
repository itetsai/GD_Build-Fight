using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class ServerUI : NetworkBehaviour {

    [SerializeField]
    public List<GameObject> NotUseObject;
    // Use this for initialization
    void Start ()
    {
        if (isServer)
            for (int i = 0; i < NotUseObject.Count; i++)
                NotUseObject[i].SetActive(false);
    }
}
