using UnityEngine;
using System.Collections;

public class GetColor_Particle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ParticleSystem ps = this.gameObject.GetComponent<ParticleSystem>();
		ps.startColor = GetComponentInParent<BeamParam>().BeamColor;
		ps.startSize *= GetComponentInParent<BeamParam>().Scale;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
