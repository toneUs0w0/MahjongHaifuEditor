using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaikyokuManager : MonoBehaviour
{
    public GameObject haifuObj;  // インスペクタでhaifuObjectを登録
    public Image imageTumo;  // ツモ牌の画面表示
    public Image imageDahai;     // 打牌の画面表示
    public Text textPlayer;
    public Text textBanner;

    public Image imageFrameTumo;
    public Image imageFrameDahai;
    public int settedTumoHaiId;  // ツモ牌の牌id
    public int settedDahaiId;

    private HaifuData haifuData;
    public bool isTumoEditing;
    public int turnPlayerId;

    public GameObject panelHaifuLog;
    public GameObject turnLogPrefab;
    public Color[] logColors;

    private List<string> index2id = new List<string>() {"none", "m1", "m2", "m3", "m4", "m5", "m6", "m7", "m8", "m9", "m5r", "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8", "p9", "p5r" };
    public List<HaiEntity> haiEnts = new List<HaiEntity>(); // private

    // Start is called before the first frame update
    void Start()
    {
        InitHaifuData();
        InitHaiEntity();

        isTumoEditing = true;
        turnPlayerId = 0;

        ResetInput();
        FrameSetting();
        SetPlayerName();
    }

    // 牌譜の取得 
    public void InitHaifuData()
    {
        haifuData = haifuObj.GetComponent<HaifuData>();
        haifuData.playerNames.Add("一郎");
        haifuData.playerNames.Add("次郎");
        haifuData.playerNames.Add("三郎");
        haifuData.playerNames.Add("四郎");
    }

    private void InitHaiEntity()
    {
        //haiEntityのリストをロード
        for (int n = 0; n < index2id.Count; n++)
        {
            HaiEntity haiEnt = (HaiEntity)Resources.Load("Hai/" + index2id[n]);
            haiEnts.Add(haiEnt);
        }
    }

    private void ResetInput()
    {
        imageTumo.sprite = haiEnts[0].haiSprite;
        settedTumoHaiId = 0;
        imageDahai.sprite = haiEnts[0].haiSprite;
        settedDahaiId = 0;
    }

    private void FrameSetting()
    {
        if (isTumoEditing)
        {
         imageFrameTumo.enabled = true;
         imageFrameDahai.enabled = false;
         textBanner.text = "ツモ牌選択";
        }
        else
        {
         imageFrameTumo.enabled = false;
         imageFrameDahai.enabled = true;
         textBanner.text = "打牌選択";
        }
    }


    public Turn AddTrun2Haifu(int PlayerId, int TumoHaiId, int DahaiId, string Action)
    {
        haifuData = haifuObj.GetComponent<HaifuData>();
        Turn turn = new Turn();
        turn.playerId = PlayerId;
        turn.tumoHaiId = TumoHaiId;
        turn.dahaiId = DahaiId;
        turn.actionType = Action;
        haifuData.haifus.Add(turn);
        //print(turn.playerId.ToString() + turn.tumoHaiId.ToString() + turn.dahaiId.ToString());
        print(haifuData.HaifuLogStr());
        return turn;
    }

    private void SetPlayerName()
    {
        if ( turnPlayerId < 0 || 4 < turnPlayerId )
        {
            print("turnPlayerId ERROR");
            return;
        }

        textPlayer.text = haifuData.playerNames[turnPlayerId];

    }

    public void PushHaiButton(int HaiId)
    {
        if (isTumoEditing)
        {
            imageTumo.sprite = haiEnts[HaiId].haiSprite;
            settedTumoHaiId = HaiId;
        }
        else
        {
            imageDahai.sprite = haiEnts[HaiId].haiSprite;
            settedDahaiId = HaiId;
        }

        PushNextbutton();
    }

    public void SwitchEditingMode()
    {
        isTumoEditing = !isTumoEditing;
        FrameSetting();
 
    }

    public void PushNextbutton()
    {
        if (isTumoEditing)
        {
            isTumoEditing = false;
            FrameSetting();
        }
        else
        {
            isTumoEditing = true;
            FrameSetting();
            Turn turn = AddTrun2Haifu(PlayerId: turnPlayerId, TumoHaiId: settedTumoHaiId, DahaiId: settedDahaiId, Action: "Nomal");
            CreateTurnLog(turn);
            turnPlayerId = (turnPlayerId + 1) % 4;
            SetPlayerName();
            ResetInput();
        }
    }

    public void PushFrame(bool IsTumoButton)
    {
        isTumoEditing = IsTumoButton;
        FrameSetting();
    }

    private void CreateTurnLog(Turn Turn)
    {
        
        GameObject turnLogObj = Instantiate(turnLogPrefab, panelHaifuLog.transform);
        turnLogObj.transform.SetAsFirstSibling();   // 上から表示
        TurnLogContents turnLogContents = turnLogObj.GetComponent<TurnLogContents>();

        turnLogContents.textTurnLog.text = haifuData.playerNames[Turn.playerId];
        turnLogContents.imageTumo.sprite = haiEnts[Turn.tumoHaiId].haiSprite;
        turnLogContents.imageDahai.sprite = haiEnts[Turn.dahaiId].haiSprite;

        // 色変更
        Image imageTurnLogObj = turnLogObj.GetComponent<Image>();
        imageTurnLogObj.color =logColors[Turn.playerId];


    }


}
