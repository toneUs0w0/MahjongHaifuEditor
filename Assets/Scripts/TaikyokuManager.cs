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
    public Text textTaikyokuName;
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

    public GameObject kawaHaiPrefab;
    public GameObject panelKawa1;
    public GameObject panelKawa2;
    public GameObject panelKawa3;
    public List<GameObject> panelKawas;

    private List<string> index2id = new List<string>() {"none", "m1", "m2", "m3", "m4", "m5", "m6", "m7", "m8", "m9", "m5r", "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8", "p9", "p5r", "s1", "s2", "s3", "s4", "s5", "s6", "s7", "s8", "s9", "s5r", "j1", "j2", "j3", "j4", "j5", "j6", "j7"};
    public List<HaiEntity> haiEnts = new List<HaiEntity>(); // private

    // Start is called before the first frame update
    void Start()
    {
        InitHaifuData();
        InitHaiEntity();
        panelKawas = new List<GameObject>();
        panelKawas.Add(panelKawa1);
        panelKawas.Add(panelKawa2);
        panelKawas.Add(panelKawa3);

        isTumoEditing = true;
        turnPlayerId = 0;

        ResetInput();
        FrameSetting();
        SetPlayerName();
        SetTaikyokuName();
    }

    // １ターン目のプレイヤー名と対局名が表示されない(Set関数がstart以降呼ばれない)問題のため今だけ置いている
    // 最終的には編集ボタンが押されたinitializeをsystemmanagerに記述するべき？
    //void Update()
   //{
        //SetTaikyokuName();
        //SetPlayerName();
    //}

    // startではなく対局編集が開始されるたびに呼び出す初期化
    public void InitTaikyokuView()
    {
        print("Init Taikyoku View");
        SetPlayerName();
        SetTaikyokuName();
    }

    // 牌譜の取得 
    public void InitHaifuData()
    {
        haifuData = haifuObj.GetComponent<HaifuData>();

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
        haifuData.kawa[PlayerId].Add(DahaiId);

        return turn;
    }

    private void SetPlayerName()
    {
        haifuData = haifuObj.GetComponent<HaifuData>();

        if ( turnPlayerId < 0 || 4 < turnPlayerId || haifuData.playerNames.Count < 4)
        {

            return;
        }

        textPlayer.text = haifuData.playerNames[turnPlayerId];

    }

    private void SetTaikyokuName()
    {
        haifuData = haifuObj.GetComponent<HaifuData>();
        // print("set taikyoku name : " + haifuData.taikyokuName);
        textTaikyokuName.text = haifuData.taikyokuName;
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

        PushNextbutton(); //牌譜登録処理全て
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
            PrepareNextHaifuInput();
   
        }
    }

    //次の牌譜入力の準備
    private void PrepareNextHaifuInput()
    {
        turnPlayerId = (turnPlayerId + 1) % 4;   // プレイヤーの更新
        SetPlayerName();    // プレイヤー名の表示変更
        ShowKawa();    // 河の表示
        ResetInput();       // ツモ打牌入力のリセット

    }

    // 河の表示
    private void ShowKawa()
    {
        // 一旦河を全削除
        for (int k = 0; k < 3; k ++)
        {
            foreach ( Transform n in panelKawas[k].transform )
            {
                GameObject.Destroy(n.gameObject);
            }

        }


        haifuData = haifuData.GetComponent<HaifuData>();

        for (int i = 0; i < haifuData.kawa[turnPlayerId].Count; i++)
        {
            int j = 0;

            if (i >= 12)
            {
                j = 2;
            }
            else if (i >= 6)
            {
                j = 1;
            }
            else
            {
                j = 0;
            }

            int haiId = haifuData.kawa[turnPlayerId][i];
            GameObject kawaHaiObj = Instantiate(kawaHaiPrefab, panelKawas[j].transform);
            Image kawaHaiImage = kawaHaiObj.GetComponent<Image>();
            kawaHaiImage.sprite = haiEnts[haiId].haiSprite;
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
