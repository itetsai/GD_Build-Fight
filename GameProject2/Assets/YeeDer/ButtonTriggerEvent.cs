using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTriggerEvent : MonoBehaviour {

    public Text Text1;
    public float fadeTime;
	// Use this for initialization
	void Start ()
    {
       Text1.color = Color.clear;
    }
    // Update is called once per frame
    public void TextIn()
    {
        Text1.color = Color.white;
    }
    public void TextOut()
    {
        Text1.color = Color.clear;
    }
}
