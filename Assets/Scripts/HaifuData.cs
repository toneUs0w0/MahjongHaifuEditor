using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 牌譜データを格納するためのクラス
public class HaifuData : MonoBehaviour
{
    public string taikyokuName;     // 対局名
    public string taikyokuSubTitle; // サブタイトル
    public List<string> playerNames;  // プレイヤー名

    public List<Turn> haifus;
    public List<List<int>> kawa;
    public List<List<int>> haipai;

    public List<int> dora;
    public List<int> uradora;

    public int ruleAka;
    public List<int> mochiten;
    public int kyoku;
    public int honba;
    public int kyoutaku;
    public int oyaId;

    public int finishType;  //0:流局 1:ロン
    public List<int> pointShift;
    public string finishTitle;

    //多分この実装は微妙なので変えたい
    private void Start() {
        this.taikyokuName = "title";
        this.taikyokuSubTitle = "";
        this.haifus = new List<Turn>();
        this.playerNames = new List<string>() {"東", "南", "西", "北"};

        // 河の初期化
        this.kawa = new List<List<int>>();
        for (int i = 0; i < 4; i ++)
        {
            List<int> k = new List<int>();
            this.kawa.Add(k);
        }

        // 手牌の初期化
        List<int> tehaiIdList = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

        // 配牌の初期化
        this.haipai = new List<List<int>>();
        for (int i = 0; i < 4; i ++)
        {
            this.haipai.Add(new List<int>(tehaiIdList));
        }

        // ドラの初期化
        dora = new List<int>() {0, 0, 0, 0};
        uradora = new List<int>() {0, 0, 0, 0};

        // 持ち点とポイントシフトの初期化
        this.mochiten = new List<int>();
        this.pointShift = new List<int>();
        for (int i = 0; i < 4; i ++)
        {
            this.mochiten.Add(25000);
            this.pointShift.Add(0);
        } 

        // 本場と供託
        this.honba = 0;
        this.kyoutaku = 0;

        finishType = 0;
        finishTitle = "30符3飜2000点∀";

    }

    // コンストラクタ
    public HaifuData()
    {
        this.haifus = new List<Turn>();
        this.playerNames = new List<string>() {"東", "南", "西", "北"};
    }

    // log
    public string HaifuLogStr()
    {
        string outputLog = "";
        for (int i = 0; i < this.haifus.Count; i++)
        {
            Turn turn = this.haifus[i];
            outputLog += "[" + i.ToString() + "] " + "player: " + turn.playerId.ToString() + ", tumo: " + turn.tumoHaiId.ToString() + ", dahai: " + turn.dahaiId.ToString() + " , Action: " + turn.actionType + "\n";
        }
        return outputLog;
    }



}
