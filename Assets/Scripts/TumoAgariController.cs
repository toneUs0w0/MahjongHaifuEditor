using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TumoAgariController : MonoBehaviour
{

    //----------------------------------------
    //
    //            ツモあがり
    //
    //----------------------------------------

    // inspectorから数値ボタンを登録
    public TaikyokuManager taikyokuManager;
    public CSVReader csvData;

    public List<GameObject> pointButtons;
    public Text textKyoutakuNum;
    public Text textHonbaNum;

    public GameObject buttonReachPlayer1;
    public GameObject buttonReachPlayer2;
    public GameObject buttonReachPlayer3;
    public GameObject buttonReachPlayer4;

    public List<Text> textPlayerBoxName;
    public List<Text> textPlayerBoxPointShift;

    public HaifuData haifu;
    public List<Image> imageDoraOriginal; // ドラ選択画面の方と区別


    private List<int> pointShift;
    private int selectedAgariPlayerId;
    private int hanNum; // 0-12
    private int fuNum; // 0-10
    private bool isOya;
    private int honba;
    private int kyoutaku;
    private bool reachPlayer1;
    private bool reachPlayer2;
    private bool reachPlayer3;
    private bool reachPlayer4;


    // fuNumの変換用
    private List<int> fuId2FuNum = new List<int>() {0, 20, 25, 30, 40, 50, 60, 70, 80, 90, 100, 110};

    private List<string> hansuuOptionList = new List<string>() {"-test-翻", "1翻", "2翻", "3翻", "4翻", "満貫", 
                                                                    "跳満", "倍満", "役満", "2倍役満", 
                                                                    "3倍役満", "4倍役満",  "5倍役満"};

    private List<string> fuOptionList = new List<string>() {"-test符", "20符", "25符", "30符", "40符", "50符", "60符",
                                                                "70符", "80符", "90符", "100符", "110符"};
    private Dictionary<string, int> fuStr2fuId = new Dictionary<string, int>();
    private LogMessager logMessager;

    //  初期処理
    public void InitAgariControllerTumo(int AgariPlayerId)
    {
        InitFuDic();
        InitPointShiftView();
        InitDropdown();

        // プルダウンにはfromとtoを代入しておく
        selectedAgariPlayerId = AgariPlayerId;
        dropdownAgariPlayer.value = AgariPlayerId;

        hanNum = 0;  // できれば手牌から予想したい
        fuNum = 0;  // できれば手牌から予想したい

        if (AgariPlayerId == haifu.oyaId)
        {
            isOya = true;
        }
        else
        {
            isOya  = false;
        }

        SetFuOption();

        // 供託、本場、リーチ棒の初期化 // haifuを参照して初期化する予定
        honba = haifu.honba;
        kyoutaku = haifu.kyoutaku;
        reachPlayer1 = false;
        reachPlayer2 = false;
        reachPlayer3 = false;
        reachPlayer4 = false;


        textHonbaNum.text = honba.ToString();  // 本場の初期後に表示する
        textKyoutakuNum.text = kyoutaku.ToString();

        InitDoraSelectPanel();
        ShowDoraSelectPanel(false);


        // ログ
        logMessager = new LogMessager();
    }

    //  符のdropdownのための辞書作成
    private void InitFuDic()
    {
        for(int  i = 0;  i < fuOptionList.Count; i++)
        {
            fuStr2fuId.Add(fuOptionList[i], i);
        }
    }
    

    //-----------------------------------------------------
    //
    //                ハンと符のdropdown
    //
    //-----------------------------------------------------

    public Dropdown dropdownHansuu;
    public Dropdown dropdownFu;
    public GameObject goDropdownFu;

    private void SetValueToHansuuFuDropdown()
    {
        dropdownHansuu.value = hanNum;
        dropdownFu.value = fuNum;
    }

    public void ClickHansuuDropdown()
    {
        hanNum = dropdownHansuu.value;
        SetFuOption();  //  符の項目を絞る
        dropdownFu.value = 0;  // 符については仕切り直し
        if (dropdownFu.options.Count > 0)   // 場合によっては符のドロップアウトの要素がないこともあるので (満貫以上とか)
        {
            string _st = dropdownFu.options[0].text;  // fuのdropdownの最初の項目を取得
            fuNum = fuStr2fuId[_st];
        }
        else
        {
            fuNum = 0;
        }


        CulcPointShift();
    }

    public void ClickFuDropdown()
    {
        string selectedFuString = dropdownFu.options[dropdownFu.value].text;
        fuNum = fuStr2fuId[selectedFuString];

        CulcPointShift();
    }

    // 選択可能なoptionのみを表示
    private void SetFuOption()
    {
        List<int> _hansuuOptionInt = new List<int>();
        List<string> _hansuuOption = new List<string>();


        // ハンを設定していない時と満貫以上の時はそもそも符を表示しない
        if (hanNum == 0 || hanNum > 4 )
        {
            ShowFuDropdown(false);
        }
        else
        {
            ShowFuDropdown(true);
            dropdownFu.ClearOptions();
            switch (hanNum)
            {
                case 1:
                    _hansuuOptionInt = new List<int>() {3, 4, 5, 6, 7, 8, 9, 10, 11};
                    break;
                case 2:
                    _hansuuOptionInt = new List<int>() {1, 3, 4, 5, 6, 7, 8, 9, 10, 11};
                    break;
                case 3:
                    _hansuuOptionInt = new List<int>() {1, 2, 3, 4, 5, 6};
                    break;
                case 4:
                    _hansuuOptionInt = new List<int>() {1, 2, 3};
                    break;
                default:
                    break;
            }
            for (int i = 0; i < _hansuuOptionInt.Count; i++)
            {
                 _hansuuOption.Add(fuOptionList[_hansuuOptionInt[i]]);
            }
            dropdownFu.AddOptions( _hansuuOption);
        }
        
    }

    private void ShowFuDropdown(bool Show)
    {
        goDropdownFu.SetActive(Show);
    }

    //-----------------------------------------------------
    //
    //                移動点数
    //
    //-----------------------------------------------------
    
    // csvから読み取った辞書のkeyを返す // 辞書にないなら空文字列を返却
    private string MakePointShiftKey()
    {
        string c_p = "C";
        string r_t = "T";   // ツモあがり
        string _han_st = (hanNum).ToString();
        string _fu_st = fuId2FuNum[fuNum].ToString();
        string _key;

        if(isOya)
        {
            c_p = "P";
        }
        else
        {
            c_p = "C";
        }

        if(hanNum > 4)  //満貫以上なら符は0
        {
            _fu_st = "0";
        }

        _key = c_p + "_" + r_t + "_" + _han_st + "_" + _fu_st;  // keyを作成

        //print(_key);

        logMessager = new LogMessager();

        if (csvData.pointDict.ContainsKey(_key))
        {
            logMessager.LogY("SEARCH : " + _key );
            return _key;
        }
        else
        {
            logMessager.LogR( _key + " is Not exist in pointDict.");
            return "";
        }

        
    }



    //-----------------------------------------------------
    //
    //                点棒移動表示
    //
    //-----------------------------------------------------

    private void InitPointShiftView()
    {
        if(textPlayerBoxName.Count != 4)
        {
            logMessager.LogY("Texts for player box names are not selected in inspector.");
        }
        for(int i = 0; i < 4; i++)
        {
            textPlayerBoxName[i].text = haifu.playerNames[i];
        }

        pointShift = new List<int>() {0, 0, 0, 0};  // 後で変更予定

        for (int i = 0; i < 4; i++)
        {
            if (pointShift[i] == 0)
            {
                textPlayerBoxPointShift[i].text = "";
            }
            else if (pointShift[i] > 0)
            {
                textPlayerBoxPointShift[i].text = "+ " + pointShift[i].ToString();;
                textPlayerBoxPointShift[i].color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            }
            else
            {
                textPlayerBoxPointShift[i].text = "- " + (pointShift[i] * -1 ).ToString();;
                textPlayerBoxPointShift[i].color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            }
        }

    }

    private void CulcPointShift()
    {

        string key = MakePointShiftKey();
        if (key == "")
        {
            return;
        }

        pointShift = new List<int>() {0, 0, 0, 0};
        List<int> tumorarePlayerIds = new List<int>();

        for(int i = 1; i < 4; i++)
        {
            tumorarePlayerIds.Add((selectedAgariPlayerId + i) % 4);
        }

        int tumoPoint1 = csvData.pointDict[key][0]; // 子の支払い
        int tumoPoint2 = csvData.pointDict[key][1]; // 親の支払い

        // あがりの点数
        if (isOya)
        {
            pointShift[selectedAgariPlayerId] += tumoPoint1 * 3;
            foreach(int pid in tumorarePlayerIds)
            {
                pointShift[pid] -= tumoPoint1;
            }
        }
        else
        {
            int oyaId = haifu.oyaId;
            pointShift[selectedAgariPlayerId] += tumoPoint2 + (tumoPoint1) * 2 ;
            foreach(int pid in tumorarePlayerIds)
            {
                pointShift[pid] -= tumoPoint1;
            }
            pointShift[oyaId] += tumoPoint1;   // 親だけは子の分はプラスで戻して親の分を追加で引く
            pointShift[oyaId] -= tumoPoint2;
        }

        // 本場
        pointShift[selectedAgariPlayerId] += honba * 300;
        foreach(int pid in  tumorarePlayerIds)
        {
            pointShift[pid] -= honba * 100;
        }

        // 供託
        pointShift[selectedAgariPlayerId] += kyoutaku * 1000;

        // 対局中のリーチ
        if (reachPlayer1)
        {
            pointShift[0] -= 1000;
            pointShift[selectedAgariPlayerId] += 1000;
        }
        if (reachPlayer2)
        {
            pointShift[1] -= 1000;
            pointShift[selectedAgariPlayerId] += 1000;
        }
        if (reachPlayer3)
        {
            pointShift[2] -= 1000;
            pointShift[selectedAgariPlayerId] += 1000;
        }
        if (reachPlayer4)
        {
            pointShift[3] -= 1000;
            pointShift[selectedAgariPlayerId] += 1000;
        }

        for (int i = 0; i < 4; i++)
        {
            if (pointShift[i] == 0)
            {
                textPlayerBoxPointShift[i].text = "";
            }
            else if (pointShift[i] > 0)
            {
                textPlayerBoxPointShift[i].text = "+ " + pointShift[i].ToString();;
                textPlayerBoxPointShift[i].color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            }
            else
            {
                textPlayerBoxPointShift[i].text = "- " + (pointShift[i] * -1 ).ToString();;
                textPlayerBoxPointShift[i].color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            }
        }

    }

    //-----------------------------------------------------
    //
    //                ドロップダウン
    //
    //-----------------------------------------------------

    public Dropdown dropdownAgariPlayer;

    private List<string> agariPlayerOptionlist = new List<string>();

    private void InitDropdown()
    {
        dropdownAgariPlayer.ClearOptions();

        int i =0;
        List<string> kaze = new List<string>() {"東", "南", "西", "北"};
        foreach (string playerName in haifu.playerNames)
        {
            agariPlayerOptionlist.Add("["+ kaze[i] + "] " + playerName);
            i++;
        }

        dropdownAgariPlayer.AddOptions(agariPlayerOptionlist);

    }

    public void ClickDropdown()
    {
        selectedAgariPlayerId = dropdownAgariPlayer.value;
        if (selectedAgariPlayerId == haifu.oyaId)
        {
            isOya = true;
        }
        else
        {
            isOya = false;   
        }
        CulcPointShift();
    }

    //-----------------------------------------------------
    //
    //                本場
    //
    //-----------------------------------------------------
    
    public void OnPushHonba(bool IsInc)
    {
        if(IsInc)
        {
            honba ++;
        }
        else if (honba > 0)
        {
            honba --;
        }
        textHonbaNum.text = honba.ToString();

        CulcPointShift();
    }

    //-----------------------------------------------------
    //
    //                供託
    //
    //-----------------------------------------------------

    public void OnPushKyoutaku(bool IsInc)
    {
        if(IsInc)
        {
            kyoutaku ++;
        }
        else if (kyoutaku > 0)
        {
            kyoutaku --;
        }
        textKyoutakuNum.text = kyoutaku.ToString();

        CulcPointShift();
    }

    //-----------------------------------------------------
    //
    //                リーチ者
    //
    //-----------------------------------------------------

    public void OnPushReachPlayer(int playerId)
    {
        switch (playerId)
        {
            case 0:
                reachPlayer1 = !reachPlayer1;
                ShapeController sc1 = buttonReachPlayer1.GetComponent<ShapeController>();
                if(reachPlayer1)
                {
                    sc1.onButtonGreen();
                }
                else
                {
                    sc1.offButtonShapeChange();
                }
                break;
            case 1:
                reachPlayer2 = !reachPlayer2;
                ShapeController sc2 = buttonReachPlayer2.GetComponent<ShapeController>();
                if(reachPlayer2)
                {
                    sc2.onButtonGreen();
                }
                else
                {
                    sc2.offButtonShapeChange();
                }
                break;
            case 2:
                reachPlayer3 = !reachPlayer3;
                ShapeController sc3 = buttonReachPlayer3.GetComponent<ShapeController>();
                if(reachPlayer3)
                {
                    sc3.onButtonGreen();
                }
                else
                {
                    sc3.offButtonShapeChange();
                }
                break;
            case 3:
                reachPlayer4 = !reachPlayer4;
                ShapeController sc4 = buttonReachPlayer4.GetComponent<ShapeController>();
                if(reachPlayer4)
                {
                    sc4.onButtonGreen();
                }
                else
                {
                    sc4.offButtonShapeChange();
                }
                break;
            default:
                break;
        }

        CulcPointShift();
    }

    //-----------------------------------------------------
    //
    //                決定ボタン
    //
    //-----------------------------------------------------

    public void OnPushFinishEditiing()
    {

        selectedAgariPlayerId = dropdownAgariPlayer.value;

        // finishType ツモ
        haifu.finishType = 2;
        
        // 本場と供託をhaifuに追加  // ゲーム中に変わらないはず
        haifu.honba = this.honba;
        haifu.kyoutaku = this.kyoutaku;

        haifu.pointShift = new List<int>(this.pointShift);

        string _key = MakePointShiftKey();
        string fu_str = "";
        if (hanNum < 3)
        {
            fu_str = dropdownFu.options[fuNum].text;
        }
        if (isOya) // oyatumo
        {
            haifu.finishTitle = fu_str + dropdownHansuu.options[hanNum].text + csvData.pointDict[_key][0].ToString() + "点∀";

        }
        else  //kotumo
        {
            haifu.finishTitle = fu_str + dropdownHansuu.options[hanNum].text + csvData.pointDict[_key][0].ToString() + "-" + csvData.pointDict[_key][1].ToString() + "点";
        }
       
        taikyokuManager.CreateOutputData();
    }


    //-----------------------------------------------------
    //
    //                ドラ指定
    //
    //-----------------------------------------------------

    public GameObject doraSelectPanel;
    public List<ShapeController> shapeControllerDoras;
    public List<Image> imageDora;

    private List<HaiEntity> haiEnts = new List<HaiEntity>();

    private int selectedDoraImageId; // 0-7
    public List<int> selectedDoraIds;

    public void InitDoraSelectPanel()
    {
        selectedDoraImageId = 0; // ゲーム開始時に指定していない場合は0スタート

        selectedDoraIds = new List<int>();
        for (int i  = 0; i < 8; i++)
        {
            selectedDoraIds.Add(0);
        }
    
        List<string> index2id = new List<string>() {"none", "m1", "m2", "m3", "m4", "m5", "m6", "m7", "m8", "m9", "m5r", "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8", "p9", "p5r", "s1", "s2", "s3", "s4", "s5", "s6", "s7", "s8", "s9", "s5r", "j1", "j2", "j3", "j4", "j5", "j6", "j7"};

        //haiEntityのリストをロード
        for (int n = 0; n < index2id.Count; n++)
        {
            HaiEntity haiEnt = (HaiEntity)Resources.Load("Hai/" + index2id[n]);
            haiEnts.Add(haiEnt);
        }

        ShowDoraPanelSelectFrame();
        DoraImageSetting();

    }

    public void ShowDoraSelectPanel(bool Show)
    {
        doraSelectPanel.SetActive(Show);
    }


    private void ShowDoraPanelSelectFrame()
    {
        foreach(ShapeController sc in shapeControllerDoras)
        {
            sc.haiButtonInitColor();
        }
        shapeControllerDoras[selectedDoraImageId].haiButtonSelectedColorChange();
    }


    public void PushDoraSelect(int DoraImageId)
    {
        selectedDoraImageId = DoraImageId;
        ShowDoraPanelSelectFrame();
    }


    public void PushHaiButtonForDoraSelect(int HaiId)
    {
        if (selectedDoraIds.Count != 8)
        {
            logMessager = new LogMessager();
            logMessager.LogR("selectedDoraIds.Count is not 8. ");
            return;
        }
        selectedDoraIds[selectedDoraImageId] = HaiId;
        DoraImageSetting();

    }
    

    // ドラ選択画面下部のドラimageに現在選択されている牌imageを挿入
    private void DoraImageSetting()
    {
        for(int i = 0; i < 8; i++)
        {
            imageDora[i].sprite = haiEnts[selectedDoraIds[i]].haiSprite;
        }
    }

    // ドラ選択パネルの戻るボタン
    public void ReturnFromDoraSelectPanel()
    {
        ShowDoraSelectPanel(false);
        SetDoraIdToHaifuData();
        OriginalDoraImageSetting();
    }

    // ドラ選択パネルでの結果を牌譜にパス
    private void SetDoraIdToHaifuData()
    {
        for (int i = 0; i < 4; i++)
        {
            haifu.dora[i] = selectedDoraIds[i];
        }
        for (int i = 4; i < 8; i++)
        {
            haifu.uradora[i-4] = selectedDoraIds[i];
        }
    }

    // 元のパネルにドラのimageを挿入
    private void OriginalDoraImageSetting()
    {
        for(int i = 0; i < 4; i++)
        {
            imageDoraOriginal[i].sprite = haiEnts[haifu.dora[i]].haiSprite;
        }
        for(int i = 4; i < 8; i++)
        {
            imageDoraOriginal[i].sprite = haiEnts[haifu.uradora[i-4]].haiSprite;
        }
    }






}

