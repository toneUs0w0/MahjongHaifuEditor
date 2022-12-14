using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaikyokuManager : MonoBehaviour
{
    public SystemManager systemManager;

    public GameObject haifuObj;  // インスペクタでhaifuObjectを登録
    public Image imageTumo;  // ツモ牌の画面表示
    public Image imageDahai;     // 打牌の画面表示
    public Text textPlayer;
    public Text textTaikyokuName;
    public Text textBanner;
    public Text frameText;

    public Image imageFrameTumo;
    public Image imageFrameDahai;
    public int settedTumoHaiId;  // ツモ牌の牌id
    public int settedDahaiId;
    public List<string> settedFuroHai;

    private HaifuData haifuData;
    public bool isTumoEditing;
    public bool ponFlag;
    public bool chiFlag;
    public bool daiminkanFlag;
    public int turnPlayerId;

    public GameObject panelHaifuLog;
    public GameObject turnLogPrefab;
    public Color[] logColors;

    public int yamaNum;       // 山の枚数
    public Text textHaiyama;

    public GameObject kawaHaiPrefab;
    public GameObject panelKawa1;
    public GameObject panelKawa2;
    public GameObject panelKawa3;
    public List<GameObject> panelKawas;

    public GameObject nakiPanel;
    public GameObject nakiFirstPanel;
    public GameObject ponPanel;
    public GameObject ryuukyokuButton;
    public Image imageNakiHai;
    public bool nakiPanelShown;

    private List<string> index2id = new List<string>() {"none", "m1", "m2", "m3", "m4", "m5", "m6", "m7", "m8", "m9", "m5r", "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8", "p9", "p5r", "s1", "s2", "s3", "s4", "s5", "s6", "s7", "s8", "s9", "s5r", "j1", "j2", "j3", "j4", "j5", "j6", "j7"};
    public List<HaiEntity> haiEnts = new List<HaiEntity>(); // private

    static int RYUUKYOKU_YAMANUM = 65;

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
        ponFlag = false;
        chiFlag = false;
        daiminkanFlag = false;
        turnPlayerId = 0;

        ResetInput();
        FrameSetting();
        SetPlayerName();
        SetTaikyokuName();
        ShowRyukyokuButton(false);
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
        ShowRyukyokuButton(false);
        ShowNakiPanel(false);
        InitTehai(UsingHaipai:true);
        ShowTehai();
        yamaNum = 69;  // 山枚数の初期化
        AllPanelClose();
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

    private void FrameTextSetting()
    {
        if (ponFlag)
        {
            frameText.text = "ポン";
            return;
        }
        else if (chiFlag)
        {
            frameText.text = "チー";
            return;
        }
        else if (daiminkanFlag)
        {
            frameText.text = "大明槓";
            return;
        }

        frameText.text = "ツモ";
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


    public Turn AddTrun2Haifu(int PlayerId, int TumoHaiId, int DahaiId, string Action, List<string> HuroHaiId)
    {
        haifuData = haifuObj.GetComponent<HaifuData>();
        Turn turn = new Turn();
        turn.playerId = PlayerId;
        turn.tumoHaiId = TumoHaiId;
        turn.dahaiId = DahaiId;
        turn.actionType = Action;
        turn.furoHaiId = new List<string>(HuroHaiId);
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

    // エディットモードの切り替え
    public void SwitchEditingMode()
    {
        isTumoEditing = !isTumoEditing;
        FrameSetting();
 
    }

    // Nextボタンの処理 (ログを作成して次の準備)
    public void PushNextbutton()
    {
        if (isTumoEditing)  //ツモ牌入力の処理
        {
            isTumoEditing = false;
            FrameSetting();
        }
        else  //打牌入力の処理
        {
            // 手牌表示のエラーチェック
            if (tehaiCorrect)
            {
                bool continueNextButton = TehaiChange(settedTumoHaiId, settedDahaiId);
                if (continueNextButton == false)  // 手牌入力でエラーを起こして戻る場合
                {
                    return;
                }
            }

            isTumoEditing = true;
            FrameSetting();
            Turn turn = new Turn();

            if (ponFlag)
            {
                turn = AddTrun2Haifu(PlayerId: turnPlayerId, TumoHaiId: settedTumoHaiId, DahaiId: settedDahaiId, Action: "Pon", HuroHaiId: settedFuroHai);
            }
            else if (chiFlag)
            {
                turn = AddTrun2Haifu(PlayerId: turnPlayerId, TumoHaiId: settedTumoHaiId, DahaiId: settedDahaiId, Action: "Chi", HuroHaiId: settedFuroHai);
            }
            else if (daiminkanFlag)
            {
                turn = AddTrun2Haifu(PlayerId: turnPlayerId, TumoHaiId: settedTumoHaiId, DahaiId: settedDahaiId, Action: "Daiminkan", HuroHaiId: settedFuroHai);
            }
            else
            {
                turn = AddTrun2Haifu(PlayerId: turnPlayerId, TumoHaiId: settedTumoHaiId, DahaiId: settedDahaiId, Action: "Nomal", HuroHaiId: new List<string>());
            }
            CreateTurnLog(turn);
            PrepareNextHaifuInput();           
   
        }
    }

    //次の牌譜入力の準備
    private void PrepareNextHaifuInput()
    {
        turnPlayerId = (turnPlayerId + 1) % 4;   // プレイヤーの更新
        SetPlayerName();    // プレイヤー名の表示変更
        if (useTehaiHyouji)
        {
            ShowTehai(); // 手牌表示
        }
        ShowKawa();    // 河の表示
        ResetInput();       // ツモ打牌入力のリセット
        ShowHaiyama(decl:true);  // 残り山枚数の表示
        ponFlag = false;  // 各種フラグの修正
        chiFlag = false;  
        daiminkanFlag = false;
        RefreshAllDahaiFlag();
        FrameTextSetting();
        // 山が0枚なら流局ボタンを表示
        if (yamaNum == RYUUKYOKU_YAMANUM)
        {
            ShowRyukyokuButton(true);
        }

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

    // 牌山の残り枚数の表示
    private void ShowHaiyama(bool decl)
    {
        if (decl)
        {
            yamaNum -= 1;
        }
        textHaiyama = textHaiyama.GetComponent<Text>();
        textHaiyama.text = "残：" + yamaNum.ToString();
    }


    // 流局ボタンの表示
    private void ShowRyukyokuButton(bool show)
    {
        ryuukyokuButton.SetActive(show);
    }

    public void PushFrame(bool IsTumoButton)
    {
        isTumoEditing = IsTumoButton;
        FrameSetting();
    }

    // 流局ボタンの処理
    public void PushRyuukyokuButton()
    {
        CreateOutputData();
    }

    //--------------------------------------------------------
    //
    //                 鳴きの処理
    //
    //--------------------------------------------------------

    public bool ponPanelShown;
    public GameObject daiminkanPanel;
    public GameObject dahaiNakiPanel;
    public GameObject chiPanel;
    public GameObject chiCandButtonPrefab;
    public GameObject chiCandButtonParent;
    public List<GameObject> ponButtons;
    public List<GameObject> daiminkanButtons;

    public bool onReach;
    public bool onTumo;
    public bool onAnkan;
    public bool onKakan;

    private void AllPanelClose()
    {
        ShowNakiPanel(false);
        ShowPonPanel(false);
        ShowChiPanel(false);
        ShowDaiminkanPanel(false);
        ShowNakiFirstPanel(false);
        ShowHaiNotInTehaiDialog(HaiId:0, show:false);
    }

   // 鳴きパネルの表示
    public void ShowNakiPanel(bool show, int mode = 1)
    {
        nakiPanel.SetActive(show);
        nakiPanelShown = show;

        if (!show)
        {
            ShowPonPanel(false);
            ShowChiPanel(false);
            ShowNakiFirstPanel(false);
            ShowHaiNotInTehaiDialog(HaiId:0, show:false);
        }

        // 鳴きパネルの表示
        if ((mode == 1) && show && haifuData.haifus.Count > 0)
        {
            // パネルコンストラクタは別で実装
            ShowNakiFirstPanel(true);
            ShowPonPanel(false);

            int dahaiId = haifuData.haifus[haifuData.haifus.Count - 1].dahaiId;
            imageNakiHai.sprite = haiEnts[dahaiId].haiSprite;
        }

        // 手牌表示のエラーダイアログ
        if (mode == 2)
        {
            ShowNakiFirstPanel(false); // 一応
            ShowPonPanel(false); // 一応
            ShowHaiNotInTehaiDialog(HaiId:settedDahaiId, show:true);
        }

    }
    // 鳴きパネルの表示
    private void ShowNakiFirstPanel(bool show)
    {
        nakiFirstPanel.SetActive(show);
    }

    // 鳴きボタン
    public void PushNakiButton()
    {
        if (nakiPanelShown)
        {
            ShowNakiPanel(false);
        }
        else
        {
            ShowNakiPanel(true);
        }
    }

    // 戻るボタン
    public void PushReturnFromNakiButton()
    {
        ShowNakiPanel(false);
        // 全フラグを切る
        ponFlag = false;
        chiFlag = false;
        daiminkanFlag = false;
    } 



    // - - - - - - - - - - - - - - - - - - - -
    //                   ポン
    // - - - - - - - - - - - - - - - - - - - - 

    // ポンボタン
    public void PushPonButton()
    {
        ShowNakiFirstPanel(false);
        ShowPonPanel(true);

    }

    // 鳴きパネルでポンを選択した際の表示
    private void ShowPonPanel(bool show)
    {
        ponPanel.SetActive(show);
        if (show)
        {
            int ignorePlayerId = haifuData.haifus[haifuData.haifus.Count-1].playerId;
            int playerId = (ignorePlayerId + 1) % 4;
            List<string> position = new List<string>() {"東", "南", "西", "北"};
            for (int i = 0; i < 3; i++)
            {
                PlayerButtonContent playerButtonContent = ponButtons[i].GetComponent<PlayerButtonContent>();
                playerButtonContent.SetContent(playerId, ignorePlayerId, "[" + position[playerId] + "]  " + haifuData.playerNames[playerId], IsDaiminkan:false);
                print(haifuData.playerNames[playerId]);
                playerId = (playerId + 1) % 4;
            }
        }
    }

    public void PonInput(int from, int to)
    {
        ShowNakiPanel(false); // パネルを閉じる
        int dId = haifuData.haifus[haifuData.haifus.Count - 1].dahaiId;
        int dahaiId = dId + 10;
        List<string> furo = new List<string> {dahaiId.ToString(), dahaiId.ToString(), dahaiId.ToString()};
        int from_to_switch = (from - to) % 4;

        switch (from_to_switch)
        {
            case 1:
                // 上家から鳴き
                furo[0] = 'p' + dahaiId.ToString();
                break;
            case 2:
                // 対面から鳴き
                furo[1] = 'p' + dahaiId.ToString();
                break;
            case 3:
                // 下家から鳴き
                furo[2] = 'p' + dahaiId.ToString();
                break;
            default:
                break;
        }
        settedFuroHai = new List<string>(furo);

        // ポンの情報を持ったまま打牌入力に行く
        yamaNum += 1; // 山を一枚増やす(ツモってないので)
        ShowHaiyama(decl:false);
        isTumoEditing = true;  // 一応
        PushHaiButton(dId);  // ポンした牌を登録
        ponFlag = true;     // チーフラグを立てておく
        isTumoEditing = false;
        FrameTextSetting();
        FrameSetting();
        turnPlayerId = from;
        SetPlayerName();
        //turn = AddTrun2Haifu(from, )
        //(int PlayerId, int TumoHaiId, int DahaiId, string Action, List<int> HuroHaiId)

    }

    // - - - - - - - - - - - - - - - - - - - -
    //                   大明かん
    // - - - - - - - - - - - - - - - - - - - - 

        // 大民間ボタン
    public void PushDaiminkanButton()
    {
        ShowNakiFirstPanel(false);
        ShowDaiminkanPanel(true);

    }

    // 鳴きパネルで大民間を選択した際の表示
    private void ShowDaiminkanPanel(bool show)
    {
        daiminkanPanel.SetActive(show);
        if (show)
        {
            int ignorePlayerId = haifuData.haifus[haifuData.haifus.Count-1].playerId;
            int playerId = (ignorePlayerId + 1) % 4;
            List<string> position = new List<string>() {"東", "南", "西", "北"};
            for (int i = 0; i < 3; i++)
            {
                PlayerButtonContent playerButtonContent = daiminkanButtons[i].GetComponent<PlayerButtonContent>();
                playerButtonContent.SetContent(playerId, ignorePlayerId, "[" + position[playerId] + "]  " + haifuData.playerNames[playerId], IsDaiminkan:true);
                print(haifuData.playerNames[playerId]);
                playerId = (playerId + 1) % 4;
            }
        }
    }

    public void DaiminkanInput(int from, int to)
    {
        ShowNakiPanel(false); // パネルを閉じる
        int dId = haifuData.haifus[haifuData.haifus.Count - 1].dahaiId;
        int dahaiId = dId + 10;
        List<string> furo = new List<string> {dahaiId.ToString(), dahaiId.ToString(), dahaiId.ToString(), dahaiId.ToString()};
        int from_to_switch = (from - to) % 4;

        switch (from_to_switch)
        {
            case 1:
                // 上家から鳴き
                furo[0] = 'm' + dahaiId.ToString();
                break;
            case 2:
                // 対面から鳴き
                furo[1] = 'm' + dahaiId.ToString();
                break;
            case 3:
                // 下家から鳴き
                furo[3] = 'm' + dahaiId.ToString();
                break;
            default:
                break;
        }
        settedFuroHai = new List<string>(furo);

        yamaNum += 1; // 山を一枚増やす(ツモってないので)
        ShowHaiyama(decl:false);
        isTumoEditing = true;  // 一応
        PushHaiButton(dId);  // ポンした牌を登録
        daiminkanFlag = true;    
        isTumoEditing = false;
        FrameTextSetting();
        FrameSetting();
        turnPlayerId = from;
        SetPlayerName();

    }



    // - - - - - - - - - - - - - - - - - - - -
    //                   チー
    // - - - - - - - - - - - - - - - - - - - - 

    // チーボタン
    public void PushChiButton()
    {
        ShowNakiFirstPanel(false);
        ShowChiPanel(true);

    }

    // 鳴きパネルでチーを選択した際の表示
    private void ShowChiPanel(bool show)
    {
        chiPanel.SetActive(show);

        if (show)
        {
            // 一旦候補を全消し
            foreach ( Transform n in chiCandButtonParent.transform )
            {
                GameObject.Destroy(n.gameObject);
            }
            // チーの形の候補を取得
            List<List<int>> chiCandList = new List<List<int>>(AllChiCandidate());
            int dahaiId = haifuData.haifus[haifuData.haifus.Count - 1].dahaiId;
            foreach (List<int> cand in chiCandList)
            {
                print(dahaiId.ToString() + " + : " + cand[0].ToString() + " " + cand[1].ToString()+ " " + cand[2].ToString());
                GameObject chiCandButton = Instantiate(chiCandButtonPrefab, chiCandButtonParent.transform);
                ChiCandButtonContent chiCandButtonContent = chiCandButton.GetComponent<ChiCandButtonContent>();
                chiCandButtonContent.SetChiCandImage(dahaiId + cand[0], dahaiId + cand[1], dahaiId + cand[2]);
            }
        }
    }

    // 一旦赤のことは無視する
    private List<List<int>> AllChiCandidate()
    {
        List<List<int>> candidate = new List<List<int>>();
        int dahaiId = haifuData.haifus[haifuData.haifus.Count - 1].dahaiId;
        if (dahaiId == 0 || dahaiId > 30)  //  候補なし
        {
            return candidate;
        }
        if (dahaiId % 10 > 2)
        {
            candidate.Add(new List<int>() {0, -1, -2});
        } 
        if (dahaiId % 10 > 1 && dahaiId % 10 < 9)
        {
            candidate.Add(new List<int>() {0, -1, 1});
        } 
        if (dahaiId % 10 < 8)
        {
            candidate.Add(new List<int>() {0, 1, 2});
        } 

        return candidate;

    }

    public void ChiInput(int HaiId1, int HaiId2, int HaiId3)
    {
        List<string> furo = new List<string> {"c" + (HaiId1 + 10).ToString(), (HaiId2 + 10).ToString(), (HaiId3 + 10).ToString()};
        ShowNakiPanel(false); // パネルを閉じる
        settedFuroHai = new List<string>(furo);
        yamaNum += 1; // 山を一枚増やす(ツモってないので)
        ShowHaiyama(decl:false);
        isTumoEditing = true;  // 一応
        PushHaiButton(HaiId1);  // ポンした牌を登録
        chiFlag = true;     // ポンフラグを立てておく
        isTumoEditing = false;
        FrameTextSetting();
        FrameSetting();

    }

    // - - - - - - - - - - - - - - - - - - - -
    //                   打牌時の鳴き
    // - - - - - - - - - - - - - - - - - - - - 

    private void RefreshAllDahaiFlag()
    {
        onReach = false;
        onTumo = false;
        onAnkan = false;
        onKakan = false;
    }

    // - - - - - - - - - - - - - - - - - - - -
    //                   リーチ
    // - - - - - - - - - - - - - - - - - - - -  

    public void PushDahaiModeselect(int mode)
    {
        RefreshAllDahaiFlag();
        switch (mode)
        {
            case 0:
                // リーチ
                onReach = true;
                break;
            case 1:
                // ツモ
                onTumo = true;
                break;
            case 2:
                // あんかん
                onAnkan = true;
                break;
            case 3:
                // かかん
                onKakan = true;
                break;
            
            default:
                break;
        }
    }


    //--------------------------------------------------------
    //
    //                 手牌表示
    //
    //--------------------------------------------------------

    public bool useTehaiHyouji;
    public bool tehaiCorrect; // private
    private List<List<int>> tehaiIds;
    public List<Image> imageTehaiList;
    public GameObject haiNotInTehaiPanel;
    public Image image_HaiNotInTehaiPanel;

    // tehaiIdsの初期化
    private void InitTehai(bool UsingHaipai)
    {
        tehaiCorrect = true;
        useTehaiHyouji = true; // 後ほど変更

        tehaiIds = new List<List<int>>();

        if (UsingHaipai)
        {
            for (int i = 0; i < 4; i++)
            {
                if (haifuData.haipai[i].Count != 13)
                {
                    print("Haipai not exist ERROR");
                    return;
                }

                tehaiIds.Add(new List<int>(haifuData.haipai[i]));
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                tehaiIds.Add(new List<int>() {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0});
            }
        }
    }
    // tehaiIds[turnPlayerId]の情報をImageに反映
    private void ShowTehai()
    {
        for(int i = 0; i < tehaiIds[turnPlayerId].Count; i++)
        {
            ShowHai2Tehai(i, tehaiIds[turnPlayerId][i]);
        }
    }

    // 手牌の一枚にspriteをセット
    private void ShowHai2Tehai(int TehaiIndex, int HaiId)
    {
        imageTehaiList[TehaiIndex].sprite = haiEnts[HaiId].haiSprite;
    }

    // turnPlayerのtehaiIdsの更新
    private bool TehaiChange(int TumoId, int DahaiId)
    {
        if (!tehaiCorrect) // 手牌が正しくない場合はそもそも実施しない
        {
            return true; // 今エラーを検知したわけでは無いのでtrueを返す
        }

        if (TumoId == DahaiId) // ツモと打牌が同じ場合は手牌に無いが問題無い // 赤についても条件を足すべき
        {
            return true;
        }

        bool dahaiExist = false;
        for (int i = 0; i < tehaiIds[turnPlayerId].Count; i++)
        {
            if (tehaiIds[turnPlayerId][i] == DahaiId)
            {
                tehaiIds[turnPlayerId][i] = TumoId;  //打牌の代わりにツモ牌を入れる  //鳴きの場合は後で
                dahaiExist = true;
                break;
            }
        }

        if (!dahaiExist)  // 打牌が手牌にない場合
        {
            ShowNakiPanel(show:true, mode:2); // エラーダイアログ表示
            return false; // 一旦処理は終了

        }

        SortAllTehai();  // 全ての手牌をソート
        return true;
    
    }

    // 0を後ろに回したソート
    private void SortAllTehai()
    {
        int pos = 0;
        int delete = 0;
        List<int> tmp_tehai;
        for(int i = 0; i < 4; i++)
        {
            pos = 0;
            delete = 0;
            tmp_tehai = new List<int>(tehaiIds[i]);
            tmp_tehai.Sort();
            for (int j = 0; j < tmp_tehai.Count; j++)
            {
                int tId = tmp_tehai[j];
                if (tId == 0)
                {
                    delete++;
                }
                else
                {
                    print("tehailength : " + tmp_tehai.Count.ToString());
                    print("pos : " + pos.ToString());
                    
                    tmp_tehai[pos] = tId;
                    pos++;
                }
            }
            for (int k = pos; k < tmp_tehai.Count; k++)
            {
                tmp_tehai[k] = 0;
            }
            tehaiIds[i] = new List<int>(tmp_tehai);
        }
        

    }

    // 手牌に無い牌を打牌した時のエラー表示
    // showNakiPanelから呼び出す
    private void ShowHaiNotInTehaiDialog(int HaiId, bool show)
    {
        if (!show)
        {
            haiNotInTehaiPanel.SetActive(false);
            return;
        }
        else
        {
            haiNotInTehaiPanel.SetActive(true);
            image_HaiNotInTehaiPanel.sprite = haiEnts[HaiId].haiSprite;
        }
    }

    //  コンテニューボタン
    public void PushHaiNotInTehaiContinue()
    {
        // ダイアログを閉じる
        ShowHaiNotInTehaiDialog(HaiId:0, show: false);
        ShowNakiPanel(false);

        // 次に呼び出さないように手牌入力をoffに
        tehaiCorrect = false;
        // 通常のnextbuttonの処理
        PushNextbutton();
    }

    public void PushHaiNotInTehaiBack()
    {
        // ダイアログを閉じる
        ShowHaiNotInTehaiDialog(HaiId:0, show: false);
        ShowNakiPanel(false);

    }


    //--------------------------------------------------------
    //
    //                 戻るボタン
    //
    //--------------------------------------------------------

    // 戻る場合に手牌管理ができずバグが起こるのを発見

    public void PushReturnButton()
    {
        if (haifuData.haifus.Count == 0) //牌譜が0なら戻れない
        {
            return;
        }
        string act = haifuData.haifus[haifuData.haifus.Count - 1].actionType;
        haifuData.haifus.RemoveAt(haifuData.haifus.Count - 1); // 末尾の削除

        turnPlayerId =  (turnPlayerId - 1) % 4;
        SetPlayerName();    // プレイヤー名の表示変更
        tehaiCorrect = false; // 一旦
        if (useTehaiHyouji)
        {
            ShowTehai(); // 手牌表示をどう戻すかは課題 // 配牌から遡って手牌を再構築する関数が必要
        }
        ShowKawa();    // 河の表示
        ResetInput();       // ツモ打牌入力のリセット
        if (act == "Nomal")
        {
            yamaNum += 1;
            ShowHaiyama(decl:false);  // 鳴き以外なら牌山を増やす
        }
        ponFlag = false; 
        daiminkanFlag = false;
        chiFlag = false;  
        FrameTextSetting();
         // ログの再表示
         AllLogApdate();


    }

    

    //--------------------------------------------------------
    //
    //                 ログ画面
    //
    //--------------------------------------------------------

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

    private void AllLogApdate()
    {
        // 一旦候補を全消し
        foreach ( Transform n in panelHaifuLog.transform )
        {
            GameObject.Destroy(n.gameObject);
        }
        // 全てのhaifuをログに追加
        foreach(Turn turn in haifuData.haifus)
        {
            CreateTurnLog(turn);
        }
    }

    //--------------------------------------------------------
    //
    //                 URL作成
    //
    //--------------------------------------------------------

    private void CreateOutputData()
    {  
        CreateHaifuUrl createHaifuUrl = haifuData.GetComponent<CreateHaifuUrl>();
        string stringUrl = createHaifuUrl.CreateHaifuUrlFromHaifuData();
        //Application.OpenURL(stringUrl);
        OpenWeb(stringUrl);
        systemManager.showOutput(stringUrl);
    }

    private void OpenWeb(string Url)
    {
        var uri = new System.Uri(Url);
        Application.OpenURL(uri.AbsoluteUri);
    }


}
