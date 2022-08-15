using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.Cameras;
using UnityEngine.Networking;
public class Nettpsguncontrol : MonoBehaviour
{
    public GameObject gun, gunbase, turretbase, currentcamera;

    public RaycastHit hitInfo;
    public Transform firepos;
    // Use this for initialization
    void Start()
    {
        int connectID = GetComponent<FindBody>().connectID;

        GameObject[] playerlist = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < playerlist.Length; i++)
            if (playerlist[i].GetComponent<NetPlayerController>().connectID == connectID)
            {
                currentcamera = playerlist[i].transform.Find("Pivot").Find("Main Camera").gameObject;
            }

        if (!currentcamera)
            currentcamera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(currentcamera.transform.position, currentcamera.transform.forward, out hitInfo, Mathf.Infinity, LayerMask.NameToLayer("UI")))
        {
#if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view
            Debug.DrawLine(currentcamera.transform.position, hitInfo.point);
#endif
        }
		gunbase.transform.LookAt(hitInfo.point,turretbase.transform.parent.up);
		gunbase.transform.Rotate (0, -90, 0);
		gunbase.transform.Rotate (-gunbase.transform.localEulerAngles.x, 0, -gunbase.transform.localEulerAngles.z, Space.Self);
		gun.transform.LookAt(hitInfo.point,turretbase.transform.parent.up);
		gun.transform.Rotate (0, -90, 0);
    }
}
