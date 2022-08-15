using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGround : MonoBehaviour {

    public bool onGround;
    private void OnTriggerStay(Collider other)
    {
        onGround = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        onGround = true;
    }

    private void OnTriggerExit(Collider other)
    {
        onGround = false;
    }
}
