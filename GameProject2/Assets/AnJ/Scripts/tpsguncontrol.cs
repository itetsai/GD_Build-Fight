using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;
using UnityEngine.Networking;
public class tpsguncontrol : MonoBehaviour {
	public GameObject gun, gunbase,turretbase,currentcamera;

	public RaycastHit hitInfo;
	public Transform firepos;
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
		gunbase.transform.LookAt(hitInfo.point,turretbase.transform.parent.up);
		gunbase.transform.Rotate (0, -90, 0);
		//gunbase.transform.localRotation.eulerAngles.Set ( 0, gunbase.transform.localRotation.eulerAngles.y, 0);
		gunbase.transform.Rotate (-gunbase.transform.localEulerAngles.x, 0, -gunbase.transform.localEulerAngles.z, Space.Self);
		gun.transform.LookAt(hitInfo.point,turretbase.transform.parent.up);
		gun.transform.Rotate (0, -90, 0);
		//gunbase.transform.localRotation.eulerAngles.Set(0, gunbase.transform.localRotation.eulerAngles.y, 0);
	//	gun.transform.LookAt(hitInfo.point,turretbase.transform.up);
		/*gunbase.transform.Rotate (Mathf.Rad2Deg * (Mathf.Acos (Vector3.Dot (gunbase.transform.up, turretbase.transform.up))), 0.0f,0.0f);
		if(gunbase.transform.localEulerAngles.x>0.0f)
			gunbase.transform.Rotate (-Mathf.Rad2Deg * (Mathf.Acos (Vector3.Dot (gunbase.transform.up, turretbase.transform.up))), 0.0f,0.0f);
		//gunbase.transform.localEulerAngles.Set(0,gunbase.transform.localEulerAngles.y,0);
	
		//Debug.Log ("x=" + gunbase.transform.localEulerAngles.x + ",y=" + gunbase.transform.localEulerAngles.y + ",z=" + gunbase.transform.localEulerAngles.z);
		gunbase.transform.Rotate (0, -90, 0);

		gun.transform.Rotate (0, -90, 0);*/
	}
}
