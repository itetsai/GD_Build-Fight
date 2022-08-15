using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpCam : MonoBehaviour {
    public float speedFactor = 0.1f;
    public Transform currentMount;



    // Update is called once per frame
    public void  Transform ()
    {
        transform.position = Vector3.Lerp(transform.position, currentMount.position, speedFactor);
        transform.rotation = Quaternion.Slerp(transform.rotation, currentMount.rotation, speedFactor);
    }



}
