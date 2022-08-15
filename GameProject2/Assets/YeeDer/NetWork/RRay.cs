using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RRay : MonoBehaviour {

    // Use this for initialization
    Camera camera;
    void Start ()
    {
        camera = transform.Find("Pivot").transform.Find("Main Camera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        RaycastHit hit = new RaycastHit();
        Vector3 pos = Input.mousePosition;
        Ray myray = new Ray(camera.transform.position, camera.transform.forward);
        if (Physics.Raycast(myray, out hit))
        {
            Debug.DrawLine(myray.origin, hit.point);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log(hit.point);
                Debug.Log(hit.collider.gameObject.transform.position);//不使用collider偵測會回傳母物件
                Debug.Log(hit.collider.gameObject.transform.name);
            }
        }
    }
}
