using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFormController : MonoBehaviour
{
    public SystemManager systemManager;

    public GameObject taikyokuView;
    public TaikyokuManager taikyokuManager;
    public GameObject inputFormView;
    public HaifuData haifu;

    public InputField inputFieldTaikyokuName;
    public InputField inputFieldTaikyokuSubTitle;
    public InputField gameObjPlayerName1;
    public InputField gameObjPlayerName2;
    public InputField gameObjPlayerName3;
    public InputField gameObjPlayerName4;

    public List<GameObject> GmOyaIcons;

    public Dropdown dropdownKyoku;
    public Dropdown dropdownHonba;
    public Dropdown dropdownKyotaku;

    public string taikyokuStr;
    private string taikyokuSubTitle;
    public List<string> playerNames;
    private int kyokuNum;
    private int honbaNum;
    private int kyotakuNum;
    private int oyaId;

    private void Start() {
        playerNames = new List<string>() {"東家", "西家", "南家", "北家"};
        taikyokuStr = "NoTitle";
        taikyokuSubTitle = "";
        kyokuNum = 0;
        honbaNum = 0;
        kyotakuNum = 0;
        oyaId = 0;
        ShowOyaIcon();
    }

    public void PushStartButton()
    {
        if (playerNames.Count < 4)
        {
            print("playerName List ERROR");
            return;
        }

        inputFieldTaikyokuName = inputFieldTaikyokuName.GetComponent<InputField>();
        taikyokuStr = inputFieldTaikyokuName.text;

        playerNames[0] = gameObjPlayerName1.GetComponent<InputField>().text;
        playerNames[1] = gameObjPlayerName2.GetComponent<InputField>().text;
        playerNames[2] = gameObjPlayerName3.GetComponent<InputField>().text;
        playerNames[3] = gameObjPlayerName4.GetComponent<InputField>().text;

        haifu = haifu.GetComponent<HaifuData>(); // ここで牌譜のインスタンスを作った方が良さそう
        haifu.taikyokuName = taikyokuStr;

        for (int i = 0; i < 4; i ++)
        {
            if (this.playerNames[i] == "")
            {
                haifu.playerNames[i] = "player" + (i + 1).ToString();   
            }
            else
            {
                haifu.playerNames[i] = this.playerNames[i]; 
            }
            
        }

        // このへんはもっと大枠のclassが必要な気がする
        taikyokuManager.InitTaikyokuView();
        taikyokuView.SetActive(true);
        inputFormView.SetActive(false);

    }

    public void PushStartButtonWithHaipaiSetting()
    {
        if (playerNames.Count < 4)
        {
            print("playerName List ERROR");
            return;
        }

        inputFieldTaikyokuName = inputFieldTaikyokuName.GetComponent<InputField>();
        taikyokuSubTitle = inputFieldTaikyokuSubTitle.text;
        taikyokuStr = inputFieldTaikyokuName.text;

        playerNames[0] = gameObjPlayerName1.GetComponent<InputField>().text;
        playerNames[1] = gameObjPlayerName2.GetComponent<InputField>().text;
        playerNames[2] = gameObjPlayerName3.GetComponent<InputField>().text;
        playerNames[3] = gameObjPlayerName4.GetComponent<InputField>().text;

        haifu = haifu.GetComponent<HaifuData>(); // ここで牌譜のインスタンスを作った方が良さそう
        haifu.taikyokuName = taikyokuStr;
        haifu.taikyokuSubTitle = taikyokuSubTitle;

        for (int i = 0; i < 4; i ++)
        {
            if (this.playerNames[i] == "")
            {
                haifu.playerNames[i] = "player" + (i + 1).ToString();   
            }
            else
            {
                haifu.playerNames[i] = this.playerNames[i]; 
            }
            
        }

        haifu.kyoku = kyokuNum;
        haifu.honba = honbaNum;
        haifu.kyoutaku = kyotakuNum;

        // このへんはもっと大枠のclassが必要な気がする
        systemManager.Form2Haipaisetting();
        

    }

    public void OnClickDropdownHonba()
    {
        honbaNum = dropdownHonba.value;
    }

    public void OnClickDropdownKyoku()
    {
        kyokuNum = dropdownKyoku.value;
        ShowOyaIcon();
    }

    public void OnClickDropdownKyotaku()
    {
        kyotakuNum = dropdownKyotaku.value;
    }


    private void ShowOyaIcon()
    {
        oyaId = kyokuNum % 4;

        foreach(GameObject gmOyaIcon in GmOyaIcons)
        {
            gmOyaIcon.SetActive(false);
        }
        GmOyaIcons[oyaId].SetActive(true);

    }

}
