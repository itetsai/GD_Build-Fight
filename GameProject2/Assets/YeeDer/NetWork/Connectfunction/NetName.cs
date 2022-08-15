using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;
using UnityEngine.UI;

public class NetName : NetworkBehaviour
{
    [SyncVar(hook ="ChangeName")]
    public string ThisName;

    void ChangeName(string name)
    {
        gameObject.name = name;
        GameObject.Find("TestText").GetComponent<Text>().text = name;
        ThisName = name;
        //GameObject.Find("TestText").GetComponent<Text>().text = ThisName;
        print("change");
    }
    private void Update()
    {
        if (isServer)
        {
            ThisName = GameObject.Find("InputField").GetComponent<InputField>().text;
        }
    }
}
