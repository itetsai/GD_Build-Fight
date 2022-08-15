using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {
	public float Vertical;
	public float Horizontal;
	public bool runOrWalk;   //true:Run  false:Walk
	public bool GameStart;
	GameObject gun;
	GameObject body;
	GameObject tanks;
	GameObject gun_M;
	Transform firePos;
	public GameObject m_fpscam,m_tpscam;
	GameObject m_currentcam;
	RaycastHit hitInfo;
	private AudioSource fire;
	bool tpsorfps=true,lightsonoff=true;
	public GameObject light1, light2;
	// Use this for initialization
	void Start () {		
		tanks =GameObject.FindGameObjectWithTag("Player");
		body = GameObject.Find("body");
		gun =GameObject.Find("Gun");
		gun_M=GameObject.Find("GunM");
		firePos=GameObject.Find("fireHole").transform;
		//m_tpscam=GameObject.Find("tpscam");
		//m_fpscam=GameObject.Find("fpscam");
		fire = GameObject.Find ("fire").GetComponent<AudioSource> ();
		runOrWalk = true;	
		GameStart=false;
		Vertical=0.0f;
		
	}	
	void Update()
	{

		if (Input.GetKey (KeyCode.W)) {
			Vector3 forward = body.transform.TransformDirection (Vector3.forward);
			tanks.GetComponent<CharacterController> ().SimpleMove (forward * 10);

		}
		if (Input.GetKey (KeyCode.S)) {
			Vector3 forward = body.transform.TransformDirection (Vector3.forward);
			tanks.GetComponent<CharacterController> ().SimpleMove (-forward * 10);

		}

		if (Input.GetKey(KeyCode.A)) {
			body.transform.Rotate (new Vector3 (0.0f, -0.5f, 0.0f));

		}
		if (Input.GetKey (KeyCode.D)) {
			body.transform.Rotate (new Vector3 (0.0f, 0.5f, 0.0f));		
		}
		if (Input.GetKeyDown(KeyCode.V)) {
			if (tpsorfps) {
				m_tpscam.SetActive (tpsorfps);
				m_tpscam.GetComponent<AudioListener> ().enabled = tpsorfps;
				tpsorfps = !tpsorfps;
				m_fpscam.SetActive (tpsorfps);
				m_fpscam.GetComponent<AudioListener> ().enabled = tpsorfps;
			} else {
				m_fpscam.SetActive (!tpsorfps);
				m_fpscam.GetComponent<AudioListener> ().enabled = !tpsorfps;
				tpsorfps = !tpsorfps;
				m_tpscam.SetActive (!tpsorfps);
				m_tpscam.GetComponent<AudioListener> ().enabled = !tpsorfps;
			}
		}
		if (Input.GetKeyDown(KeyCode.F)) {
				lightsonoff = !lightsonoff;
				light1.SetActive (lightsonoff);
			light2.SetActive (lightsonoff);
		}


		if (Physics.Raycast (m_tpscam.transform.position, m_tpscam.transform.forward, out hitInfo,5000,5)) {
				#if UNITY_EDITOR
				// helper to visualise the ground check ray in the scene view
			Debug.DrawLine (m_tpscam.transform.position, hitInfo.point);
				#endif
			}
			gun.transform.LookAt(hitInfo.point);
			gun.transform.Rotate (-gun.transform.rotation.eulerAngles.x, 0, 0);
			gun_M.transform.LookAt(hitInfo.point);
			if (Input.GetMouseButtonDown(0))
			{
				GameObject bullet = Instantiate(Resources.Load("CompleteShell"),firePos.position,firePos.rotation) as GameObject;
				bullet.GetComponent<Rigidbody>().AddForce(gun_M.transform.forward*8000);
				fire.Play ();
			}

	}

}
