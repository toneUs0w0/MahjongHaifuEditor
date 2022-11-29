using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 牌譜データを格納するためのクラス
public class HaifuData : MonoBehaviour
{
    public List<string> playerNames;
    public List<Turn> haifus;
    public List<int> dora;
    public List<int> uradora;

    // コンストラクタ
    public HaifuData()
    {
        this.haifus = new List<Turn>();
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
