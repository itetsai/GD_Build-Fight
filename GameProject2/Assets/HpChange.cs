using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpChange : MonoBehaviour {
	public NetPlayerController player;
	public Image hpbar;
	// Use this for initialization
	void Start () {
		player = transform.root.GetComponent<NetPlayerController> ();
		hpbar = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		hpbar.fillAmount = player.main.currentHp / player.main.totalHp;
	}
}
