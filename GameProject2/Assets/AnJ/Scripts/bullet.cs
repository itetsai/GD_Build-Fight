using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {
	public float autodestroytime=5;
	public int damage=10;
	public bool burn=true;
	//RaycastHit hitinfo;
	void Start(){
		Destroy (gameObject, autodestroytime);
	}
	void Update()
	{
		/*if (Physics.Raycast (transform.position, transform.forward, out hitinfo, 20f)) {
			if(hitinfo.collider.gameObject.GetComponent<objStateControl> ())
				hitinfo.collider.gameObject.GetComponent<objStateControl> ().OnDamage (damage);
			Destroy(gameObject);
		}*/
	}
	void OnTriggerEnter(Collider other) 
	{
		objStateControl hit;
		if (hit=other.gameObject.GetComponentInParent<objStateControl> ()) {
			hit.OnDamage (damage);
			if(burn)
			hit.startBurn ();
		}
        	Destroy(gameObject);

    }

}

