using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveDataContentController : MonoBehaviour
{
    [SerializeField] GameObject panelSecondButton;

    public void ShowSecondButton(bool show)
    {
        panelSecondButton.SetActive(show);
    }

    public void PushSaveDataEditButton()
    {
        
    }
}
