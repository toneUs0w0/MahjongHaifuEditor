using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 牌譜データを格納するためのクラス
public class HaifuData : MonoBehaviour
{
    public string taikyokuName;
    public List<string> playerNames;

    public List<Turn> haifus;
    public List<List<int>> kawa;

    public List<int> dora;
    public List<int> uradora;

    //多分この実装は微妙なので変えたい
    private void Start() {
        this.haifus = new List<Turn>();
        this.playerNames = new List<string>() {"東", "南", "西", "北"};
        this.kawa = new List<List<int>>();
        for (int i = 0; i < 4; i ++)
        {
            List<int> k = new List<int>();
            this.kawa.Add(k);
        }
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
