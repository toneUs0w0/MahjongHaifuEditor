using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 牌譜データを格納するためのクラス
public class HaifuData : MonoBehaviour
{
    public string taikyokuName;     // 対局名
    public string taikyokuSubTitle; // サブタイトル
    public List<string> playerNames;  // プレイヤー名

    public List<Turn> haifus;   // turnのリスト
    public List<List<int>> kawa;  // 河
    public List<List<int>> haipai;  // 配牌

    public List<int> dora;  //ドラ
    public List<int> uradora;  //裏ドラ

    public int ruleAka;   // ルールid
    public List<int> mochiten;  //開始時の持ち点
    public int kyoku;  // 局id
    public int honba;   // 本場
    public int kyoutaku;  // 供託本数
    public int oyaId;  //スタート親

    public int finishType;  //0:流局 1:ロン 2:ツモ
    public List<int> pointShift;  //ポイントシフト
    public string finishTitle;  // あがり時の画面表示
    public int finishPlayerId;  //上がったプレイヤーid
    public int houjuPlayerId;   // 放銃したプレイヤーid
    public List<int> agariYaku; // あがり役

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
        this.mochiten = new List<int>() {25000, 25000, 25000, 25000};
        this.pointShift = new List<int>() {0, 0, 0, 0};

        // 本場と供託
        this.honba = 0;
        this.kyoutaku = 0;

        finishType = 0;
        finishTitle = "30符3飜2000点∀";
        finishPlayerId = 0;
        houjuPlayerId = 0;

    }

    // コンストラクタ
    public HaifuData()
    {
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
        this.mochiten = new List<int>() {25000, 25000, 25000, 25000};
        this.pointShift = new List<int>() {0, 0, 0, 0};

        // 本場と供託
        this.honba = 0;
        this.kyoutaku = 0;

        finishType = 0;
        finishTitle = "30符3飜2000点∀";
        finishPlayerId = 0;
        houjuPlayerId = 0;

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
