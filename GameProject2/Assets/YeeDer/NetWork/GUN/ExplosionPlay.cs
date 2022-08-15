using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPlay : MonoBehaviour
{   
	void Start () {
        GetComponent<ParticleSystem>().Play();	
	}
}
