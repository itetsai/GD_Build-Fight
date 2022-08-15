using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Prototype.NetworkLobby
{
    //List of players in the lobby
    public class LobbyPlayerList : MonoBehaviour
    {
        public static LobbyPlayerList _instance = null;

        public RectTransform playerListTeamRedContentTransform;
		public RectTransform playerListTeamBlueContentTransform;
        public GameObject warningDirectPlayServer;
		public Transform addButtonRowRed,addButtonRowBlue;
		public Dropdown modeSelect;

        protected VerticalLayoutGroup _layout;
        protected List<LobbyPlayer> _players = new List<LobbyPlayer>();

        public void OnEnable()
        {
            _instance = this;
            _layout = playerListTeamRedContentTransform.GetComponent<VerticalLayoutGroup>();
        }

        public void DisplayDirectServerWarning(bool enabled)
        {
            if(warningDirectPlayServer != null)
                warningDirectPlayServer.SetActive(enabled);
        }

        void Update()
        {
            //this dirty the layout to force it to recompute evryframe (a sync problem between client/server
            //sometime to child being assigned before layout was enabled/init, leading to broken layouting)
            
            if(_layout)
                _layout.childAlignment = Time.frameCount%2 == 0 ? TextAnchor.UpperCenter : TextAnchor.UpperLeft;
        }

        public void AddPlayer(LobbyPlayer player)
        {
            if (_players.Contains(player))
                return;

            _players.Add(player);

            player.transform.SetParent(playerListTeamRedContentTransform, false);
			addButtonRowRed.transform.SetAsLastSibling();
			modeSelect.interactable = false;

            PlayerListModified();
        }

        public void RemovePlayer(LobbyPlayer player)
        {
            _players.Remove(player);
            PlayerListModified();
        }

		/*public void changeToTeamRed(LobbyPlayer player)
		{
			
			player.transform.SetParent(playerListTeamRedContentTransform, false);
			addButtonRowRed.transform.SetAsLastSibling();
			PlayerListModified();
		}*/
		public void changeToTeamRedButton()
		{
			foreach (LobbyPlayer p in _players)
				if (p.isLocalPlayer)
					p.ChangeToTeamRed ();
		}

		public void changeToTeamBlueButton()
		{
			foreach (LobbyPlayer p in _players)
				if (p.isLocalPlayer)
					p.ChangeToTeamBlue ();
		}
			
		public void changeToTeamRed(LobbyPlayer player)
		{
			player.team = 1;
			player.transform.SetParent(playerListTeamRedContentTransform, false);
			addButtonRowRed.transform.SetAsLastSibling();
			PlayerListModified();
		}
		public void changeToTeamBlue(LobbyPlayer player)
		{
			player.team = 0;
			player.transform.SetParent(playerListTeamBlueContentTransform, false);
			addButtonRowBlue.transform.SetAsLastSibling();
			PlayerListModified();
		}

        public void PlayerListModified()
        {
            int i = 0;
            foreach (LobbyPlayer p in _players)
            {
                p.OnPlayerListChanged(i);
                ++i;
            }
        }
    }
}
