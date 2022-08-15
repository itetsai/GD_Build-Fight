using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Vehicles.Car;

public class TimePsitionLock : MonoBehaviour {

    // Use this for initialization
    public GameObject body;
    public GameObject move;
	void Start ()
    {
		
	}

    float time = 0;
    bool k = true;
    void Update()
    {
        if (k)
        {
            if (time < 5)
            {
                move.GetComponent<Rigidbody>().isKinematic = true;
                body.GetComponent<Rigidbody>().isKinematic = true;
                time += Time.deltaTime;
                move.transform.position = body.transform.position;

                body.transform.eulerAngles = new Vector3(0, 0, 0);
                move.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
                k = false;

            if (k == false)
            {
                move.GetComponent<Rigidbody>().isKinematic = false;
                body.GetComponent<Rigidbody>().isKinematic = false;

                if (GetComponent<NetworkIdentity>().hasAuthority)
                {
                    move.AddComponent<FixedJoint>();
                    move.GetComponent<FixedJoint>().connectedBody = body.GetComponent<Rigidbody>();
                }

                if (move.GetComponent<NetCAR>())
                    if (GetComponent<NetworkIdentity>().hasAuthority)
                    {
                        move.GetComponent<NetCAR>().getWheel();
                        if (move.GetComponent<NetCAR>().m_WheelColliders.Count > 0)
                        {
                            move.GetComponent<NetCAR>().enabled = true;
                            move.GetComponent<NetCarControll>().enabled = true;
                        }
                    }

                if (move.GetComponent<NetHelicopterController>())
                    if (GetComponent<NetworkIdentity>().hasAuthority)
                        if (move.GetComponent<NetHelicopterController>().MainRotorController != null)
                        {
                            move.GetComponent<ControlPanel>().enabled = true;
                            move.GetComponent<NetHelicopterController>().enabled = true;
                        }

                if (move.GetComponentInChildren<NetTRF>())
                {
                    move.GetComponentInChildren<NetTRF>().enabled = true;
                }

                if (!GetComponent<NetworkIdentity>().hasAuthority)
                {
                    body.GetComponent<Rigidbody>().isKinematic = true;
                    move.GetComponent<Rigidbody>().isKinematic = true;
                }
                this.enabled = false;
            }
        }
    }
}
