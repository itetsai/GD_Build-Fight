using UnityEngine;
using UnityEngine.SceneManagement;
using Prototype.NetworkLobby;

public class OnOffbyScene : MonoBehaviour
{
	// called zero
	void Awake()
	{
		Debug.Log("Awake");
	}

	// called first
	void OnEnable()
	{
		Debug.Log("OnEnable called");
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	// called second
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Debug.Log("OnSceneLoaded: " + scene.name);
		if (scene.name == "loadaing scene") {
			LobbyManager.s_Singleton.background.gameObject.SetActive (false);
			LobbyManager.s_Singleton.backgroundEffect.gameObject.SetActive (false);
			LobbyManager.s_Singleton.ChangeTo (null);
			LobbyManager.s_Singleton.topPanel.isInGame = false;
		}
		if (scene.name == "NetworkLobby") {
			LobbyManager.s_Singleton.background.gameObject.SetActive (true);
			LobbyManager.s_Singleton.backgroundEffect.gameObject.SetActive (true);
			LobbyManager.s_Singleton.ChangeTo (LobbyManager.s_Singleton.mainMenuPanel);
			LobbyManager.s_Singleton.topPanel.isInGame = false;
		}
		Debug.Log(mode);
	}

	// called third
	void Start()
	{
		Debug.Log("Start");
	}

	// called when the game is terminated
	void OnDisable()
	{
		Debug.Log("OnDisable");
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}