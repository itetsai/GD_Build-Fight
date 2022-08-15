using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamControl : MonoBehaviour {

    public float speedFactor = 0.1f;
    public Transform currentMount;


    //public void Start()
    //{
    //    transform.position = Vector3.Lerp(transform.position, currentMount.position, speedFactor);
    //    transform.rotation = Quaternion.Slerp(transform.rotation, currentMount.rotation, speedFactor);
    //}

    public void SetMount(Transform newMount)
    {
        currentMount = newMount;
    }
    public void Update () {
        transform.position = Vector3.Lerp(transform.position, currentMount.position, speedFactor);
        transform.rotation=Quaternion.Slerp(transform.rotation,currentMount.rotation,speedFactor);
	}


}
