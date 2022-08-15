using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
public class PauseGame : MonoBehaviour {


    public Transform panel;
    public Transform Player;
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(panel.gameObject.activeInHierarchy==false)
            {
                panel.gameObject.SetActive(true);
                Time.timeScale = 0;
                Player.GetComponent<FirstPersonController>().enabled = false;
            }
            else
            {
                panel.gameObject.SetActive(false);
                Time.timeScale = 1;
                Player.GetComponent<FirstPersonController>().enabled = true;

            }
        }




	}
}
