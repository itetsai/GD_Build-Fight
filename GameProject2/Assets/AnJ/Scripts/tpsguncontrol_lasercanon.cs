using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class tpsguncontrol_lasercanon : MonoBehaviour {
	public GameObject gun,currentcamera,gunbase;
	public RaycastHit hitInfo;
	// Use this for initialization
	void Start ()
    {
        if (!currentcamera)
			currentcamera = Camera.main.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (Physics.Raycast (currentcamera.transform.position, currentcamera.transform.forward, out hitInfo,Mathf.Infinity,LayerMask.NameToLayer("UI"))) {
			#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
			Debug.DrawLine (currentcamera.transform.position, hitInfo.point);
			#endif
		}
		gun.transform.LookAt(hitInfo.point, gunbase.transform.parent.up);
		//gun.transform.Rotate (-90, 0, 0);
		gun.transform.Rotate(gunbase.transform.rotation.eulerAngles);
		gun.transform.Rotate(-gunbase.transform.parent.rotation.eulerAngles.x, gunbase.transform.parent.rotation.eulerAngles.z, -gunbase.transform.parent.rotation.eulerAngles.y);
		//gun.transform.Rotate(-gunbase.transform.parent.parent.rotation.eulerAngles.x, gunbase.transform.parent.parent.rotation.eulerAngles.z, -gunbase.transform.parent.parent.rotation.eulerAngles.y);

	}
}
