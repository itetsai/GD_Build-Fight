using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class deathTime : MonoBehaviour {
    public float delTime;
    public Text time;
    public Image filled;
	// Use this for initialization
	void Start () {
        delTime=10;
        filled.fillAmount = 0;
	}
	
	// Update is called once per frame
	void Update () {
        delTime -= Time.deltaTime;
        time.text = ((int)delTime).ToString();
        filled.fillAmount = (10 - delTime) / 10;
    }
}
