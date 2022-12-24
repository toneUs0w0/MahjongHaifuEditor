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
        public int mode;

        // mode -> 0:pon, 1:daiminkan, 2:ron

        public void SetContent(int PlayerId, int CallingPlayerId, string ShowString, int Mode)
        {
            playerId = PlayerId;
            callingPlayerId = CallingPlayerId;
            showString = ShowString;
            textButton.text = ShowString;
            mode = Mode;
        }

        public void PushPlayerButton()
        {
            if (mode == 1)
            {
                taikyokuManager.DaiminkanInput(from:playerId, to:callingPlayerId);
            }
            else if (mode == 0)
            {
                taikyokuManager.PonInput(from:playerId, to:callingPlayerId);
            }
            else if (mode == 2)
            {
                taikyokuManager.RonInput(from:playerId, to:callingPlayerId);
            }
            
            
        }
}
