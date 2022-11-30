using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFormController : MonoBehaviour
{
    public GameObject taikyokuView;
    public GameObject inputFormView;
    public HaifuData haifu;

    public InputField inputFieldTaikyokuName;
    public InputField gameObjPlayerName1;
    public InputField gameObjPlayerName2;
    public InputField gameObjPlayerName3;
    public InputField gameObjPlayerName4;

    public string taikyokuStr;
    public List<string> playerNames;

    private void Start() {
        playerNames = new List<string>() {"東家", "西家", "南家", "北家"};
        taikyokuStr = "NoTitle";
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
        taikyokuView.SetActive(true);
        inputFormView.SetActive(false);

    }


}
