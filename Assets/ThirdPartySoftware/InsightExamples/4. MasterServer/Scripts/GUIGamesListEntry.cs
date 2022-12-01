using UnityEngine;
using UnityEngine.UI;

namespace Insight.Examples
{
    public class GUIGamesListEntry : MonoBehaviour
    {
        public PlayerClientGUI clientComp;

        public Text SceneNameText;
        public Text PlayerCountText;
        public Text RoomNameText;
        public Text RoomPasswordText;

        public string UniqueID;
        public string SceneName;
        public int CurrentPlayers;
        public int RoomMaxPlayers;
        public string RoomName;
        public string RoomCreatorText;
        public string CreatorID;
        public string RoomPassword;
        public string RoomExpireDate;

        private bool Init;

        private void LateUpdate()
        {
            if (!Init)
            {
                Init = true;

                SceneNameText.text = SceneName; 
                PlayerCountText.text = CurrentPlayers + "/" + RoomMaxPlayers;
                RoomNameText.text = RoomName + "/" + RoomCreatorText + "/" + CreatorID;
                RoomPasswordText.text = RoomPassword;
            }
        }

        public void HandleSelectButton()
        {
            clientComp.HandleJoinGameButton(UniqueID);
        }
    }
}
