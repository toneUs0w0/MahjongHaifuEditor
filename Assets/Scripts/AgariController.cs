using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgariController : MonoBehaviour
{
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

    private int onPushPoint;
    private List<int> pointShift;
    private int selectedAgariPlayerId;
    private int selectedHoujuPlayerId;
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
    private List<int> fuId2FuNum = new List<int>() {20, 25, 30, 40, 50, 60, 70, 80, 90, 100, 110};

    private LogMessager logMessager;

    //  初期処理
    public void InitAgariController(int AgariPlayerId, int HoujuPlayerId)
    {
        AllPointButtonOff();
        InitPointShiftView();
        InitDropdown();
        onPushPoint = 0;

        // プルダウンにはfromとtoを代入しておく
        selectedAgariPlayerId = AgariPlayerId;
        selectedHoujuPlayerId = HoujuPlayerId;
        dropdownAgariPlayer.value = AgariPlayerId;
        dropdownHoujuPlayer.value = HoujuPlayerId;

        hanNum = 0; // 1ハン
        fuNum = 2;  // 30符
        isOya = false;
        SetValueToHansuuFuDropdown();

        // 供託、本場、リーチ棒の初期化 // haifuを参照して初期化する予定
        honba = haifu.honba;
        kyoutaku = haifu.kyoutaku;
        reachPlayer1 = false;
        reachPlayer2 = false;
        reachPlayer3 = false;
        reachPlayer4 = false;


        textHonbaNum.text = honba.ToString();  // 本場の初期後に表示する
        textKyoutakuNum.text = kyoutaku.ToString();


        // ログ
        logMessager = new LogMessager();
    }
    

    // ポイントボタン
    public void OnPushPointButton(int Point)
    {
        logMessager = new LogMessager();
        logMessager.LogY(Point.ToString());
        //AllPointButtonOff();

        onPushPoint = Point;
        CulcPointShift();

    }

    //  全てのボタンを初期化
    public void AllPointButtonOff()
    {
        for(int i = 0; i < pointButtons.Count; i++)
        {
            GameObject buttonPoint = pointButtons[i];
            ShapeController shapeController = buttonPoint.GetComponent<ShapeController>();
            shapeController.offButtonShapeChange();
        }
    }

    //-----------------------------------------------------
    //
    //                移動点数
    //
    //-----------------------------------------------------

    public Dropdown dropdownHansuu;
    public Dropdown dropdownFu;

    private void SetValueToHansuuFuDropdown()
    {
        dropdownHansuu.value = hanNum;
        dropdownFu.value = fuNum;
    }

    public void ClickHansuuFuDropdown()
    {
        hanNum = dropdownHansuu.value;
        fuNum = dropdownFu.value;
        CulcPointShift();
    }
    
    // csvから読み取った辞書のkeyを返す // 辞書にないなら空文字列を返却
    private string MakePointShiftKey()
    {
        string c_p = "C";
        string r_t = "R";
        string _han_st = (hanNum + 1).ToString();
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

        if(hanNum > 3)  //満貫以上なら符は0
        {
            _fu_st = "0";
        }

        _key = c_p + "_" + r_t + "_" + _han_st + "_" + _fu_st;  // keyを作成

        //print(_key);

        logMessager = new LogMessager();

        if (csvData.pointDict.ContainsKey(_key))
        {
            //logMessager.LogY( _key + " is exist in pointDict.");
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
    }

    private void CulcPointShift()
    {
        if (selectedAgariPlayerId == selectedHoujuPlayerId) // あがり==放銃の場合は計算しない
        {
            return;
        }

        string key = MakePointShiftKey();
        if (key == "")
        {
            return;
        }

        pointShift = new List<int>() {0, 0, 0, 0};

        int ronPoint = csvData.pointDict[key][0];

        // あがりの点数
        pointShift[selectedAgariPlayerId] += ronPoint;
        pointShift[selectedHoujuPlayerId] -= ronPoint;

        // 本場
        pointShift[selectedAgariPlayerId] += honba * 300;
        pointShift[selectedHoujuPlayerId] -= honba * 300;

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
    public Dropdown dropdownHoujuPlayer;

    private List<string> agariPlayerOptionlist = new List<string>();
    private List<string> houjuPlayerOptionlist = new List<string>();

    private void InitDropdown()
    {
        dropdownAgariPlayer.ClearOptions();
        dropdownHoujuPlayer.ClearOptions();

        int i =0;
        List<string> kaze = new List<string>() {"東", "南", "西", "北"};
        foreach (string playerName in haifu.playerNames)
        {
            agariPlayerOptionlist.Add("["+ kaze[i] + "] " + playerName);
            houjuPlayerOptionlist.Add("["+ kaze[i] + "] " + playerName);
            i++;
        }

        dropdownAgariPlayer.AddOptions(agariPlayerOptionlist);
        dropdownHoujuPlayer.AddOptions(houjuPlayerOptionlist);

    }

    public void ClickDropdown()
    {
        selectedAgariPlayerId = dropdownAgariPlayer.value;
        selectedHoujuPlayerId = dropdownHoujuPlayer.value;
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
        selectedHoujuPlayerId = dropdownHoujuPlayer.value;

        // finishType ロン
        haifu.finishType = 1;
        
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

        haifu.finishTitle = fu_str + dropdownHansuu.options[hanNum].text + csvData.pointDict[_key][0].ToString() + "点";

        taikyokuManager.CreateOutputData();
    }



}
