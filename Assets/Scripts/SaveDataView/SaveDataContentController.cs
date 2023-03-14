using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveDataContentController : MonoBehaviour
{
    [SerializeField] GameObject panelSecondButton;
    [SerializeField] Text textSubTitle;
    [SerializeField] Text textTitle;
    [SerializeField] Text textKyokuHonba;
    [SerializeField] Text textPlayer;
    [SerializeField] Text textEditDate;

    private string fileName;
    private string title;
    private string subtitle;
    private int kyoku;
    private int honba;
    private string player1;
    private string player2;
    private string player3;
    private string player4;
    private string date;

    bool isShownSecondButtonPanel;


    public void InitSaveDataContent(HaifuInfo haifuInfo)
    {
        fileName = haifuInfo.file_name;
        title = haifuInfo.title;
        subtitle = haifuInfo.sub_title;
        kyoku = haifuInfo.kyoku;
        honba = haifuInfo.honba;
        player1 = haifuInfo.player1;
        player2 = haifuInfo.player2;
        player3 = haifuInfo.player3;
        player4 = haifuInfo.player4;
        date = haifuInfo.date;

        ShowSecondButton(false);

        SetInformation4TextUi();
    }

    private void SetInformation4TextUi()
    {
        List<string> kyokuId2kyokuStr = new List<string> () {"東一局", "東二局", "東三局", "東四局", "南一局", "南二局", "南三局", "南四局", "西一局", "西二局", "西三局", "西四局", "北一局", "北二局", "北三局", "北四局"};
        textSubTitle.text = subtitle;
        textTitle.text = title;
        textKyokuHonba.text = kyokuId2kyokuStr[kyoku] + " " + honba.ToString() + "本場";
        textPlayer.text = "[東] " + player1 + "   [南] " + player2 +  "   [西] " + player3 +  "   [北] " + player4; 
        textEditDate.text = date;

    }

    public void ShowSecondButton(bool show)
    {
        panelSecondButton.SetActive(show);
        isShownSecondButtonPanel = show;
    }

    public void PushSaveDataContent()
    {
        if (isShownSecondButtonPanel)
        {
            ShowSecondButton(false);
        }

        else
        {
            // prefabからの呼び出し
            GameObject objSaveFileSelectController = GameObject.Find("SaveFileSelectController");
            SaveFileSelectController saveFileSelectController = objSaveFileSelectController.GetComponent<SaveFileSelectController>();
            saveFileSelectController.AllSaveDataContent2Untouched(); // 全てのボタンのsecondPanelを非表示にする

            ShowSecondButton(true);
        }
        
    }


    public void PushSaveDataEditButton()
    {
        // prefabからの呼び出し
        GameObject objSaveFileSelectController = GameObject.Find("SaveFileSelectController");
        SaveFileSelectController saveFileSelectController = objSaveFileSelectController.GetComponent<SaveFileSelectController>();
        saveFileSelectController.FileSelect2HaifuViewer(this.fileName);
    }
}
