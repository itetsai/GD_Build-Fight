using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void sceneChange()
    {
		//NetworkLobby
		//Game
		//主要之scene
		StartCoroutine(change());
    }

	IEnumerator change()
	{
		yield return new WaitForSeconds (3f);
		SceneManager.LoadScene("NetworkLobby");
	}
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            sceneChange();
    }
}
