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
        public bool isDaiminkan;

        public void SetContent(int PlayerId, int CallingPlayerId, string ShowString, bool IsDaiminkan)
        {
            playerId = PlayerId;
            callingPlayerId = CallingPlayerId;
            showString = ShowString;
            textButton.text = ShowString;
            isDaiminkan = IsDaiminkan;
        }

        public void PushPlayerButton()
        {
            if (isDaiminkan)
            {
                taikyokuManager.DaiminkanInput(from:playerId, to:callingPlayerId);
            }
            else
            {
                taikyokuManager.PonInput(from:playerId, to:callingPlayerId);
            }
            
            
        }
}
