using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButtonContent : MonoBehaviour
{
        public int playerId;
        public int callingPlayerId;
        public string showString;
        public Text textButton;
        public TaikyokuManager taikyokuManager;

        public void SetContent(int PlayerId, int CallingPlayerId, string ShowString)
        {
            playerId = PlayerId;
            callingPlayerId = CallingPlayerId;
            showString = ShowString;
            textButton.text = ShowString;
        }

        public PushPlayerButton()
        {
            taikyokuManager.PonInput(from:playerId, to:CallingPlayerId);
        }
}
