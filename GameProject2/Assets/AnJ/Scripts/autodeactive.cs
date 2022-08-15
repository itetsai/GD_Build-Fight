using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autodeactive : MonoBehaviour {
	List<GameObject> deactiveList=new List<GameObject>();
	// Use this for initialization
	void Start () {
		StartCoroutine (deactive ());
	}
	IEnumerator deactive()
	{
		yield return new WaitForSeconds (3f);
		for (int i = 0; i < transform.childCount; i++) {
			List<Renderer> rends=new List<Renderer>();
			List<Collider> collds=new List<Collider>();
			rends.AddRange(transform.GetChild(i).gameObject.GetComponentsInChildren<Renderer> ());
			collds.AddRange(transform.GetChild(i).gameObject.GetComponentsInChildren<Collider> ());
			foreach (Renderer r in rends)r.enabled = false;
			foreach (Collider r in collds)r.enabled = false;
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
