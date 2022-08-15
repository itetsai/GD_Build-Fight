using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.Cameras;

namespace Prototype.NetworkLobby
{
    public class LobbyTopPanel : MonoBehaviour
    {
        public bool isInGame = false;
		NetPlayerController player=null;
        protected bool isDisplayed = true;
        protected Image panelImage;

        void Start()
        {
			
            panelImage = GetComponent<Image>();
        }


        void Update()
        {
            if (!isInGame)
                return;

			if (!player)
				player = GameMode.sInstance.localplayer;
			
            if (Input.GetKeyDown(KeyCode.Escape))
			{
                ToggleVisibility(!isDisplayed);
				if (isDisplayed) {
					Cursor.visible = true;
					Cursor.lockState = CursorLockMode.None;
				} else {
					Cursor.visible = false;
					Cursor.lockState = CursorLockMode.Locked;
				}
            }

        }

        public void ToggleVisibility(bool visible)
        {
            isDisplayed = visible;
            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(isDisplayed);
            }

            if (panelImage != null)
            {
                panelImage.enabled = isDisplayed;
            }
			player.GetComponent<FreeLookCam> ().m_LockCursor = !visible;
        }
    }
}