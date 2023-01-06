using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHaifuUrl : MonoBehaviour
{
    public HaifuData haifuData;

    public string CreateHaifuUrlFromHaifuData()
    {
        string title_str = "\"title\":[\"" + haifuData.taikyokuName + "\",\"" + haifuData.taikyokuSubTitle + "\"]";
        string player_str = "\"name\":[\"" + haifuData.playerNames[0] + "\",\"" + haifuData.playerNames[1] + "\",\"" + haifuData.playerNames[2] + "\",\"" + haifuData.playerNames[3] + "\"]";
        string rule_str = "\"rule\":{\"aka\":0}";
        string honba_str = "[" + haifuData.kyoku.ToString() + "," + haifuData.honba.ToString() + "," + haifuData.kyoutaku.ToString() + "]";
        string mochiten_str = "[" + haifuData.mochiten[0] + "," + haifuData.mochiten[1] + "," + haifuData.mochiten[2] + "," + haifuData.mochiten[3] + "]";
        string dora_str = "[" + makeDoraString() + "]";
        string uradora_str = "[" + makeUradoraString() + "]";
        List<string> haipai_str_list = new List<string>();
        List<List<List<string>>> tumo_dahai_str_list = new List<List<List<string>>>(DevideTurnData());
        string finish_str = "";

        foreach(List<int> h in haifuData.haipai)
        {
            List<string> h_str = new List<string>(HaiIds2TenhouHaiIdStrs(h));
            haipai_str_list.Add("[" + string.Join(",", h_str) + "]");
        }

    
        string finishType_str = "";
        List<int> finishPointShift = new List<int>();
        string finishPointShift_str = "";
        string finish_other_str = "";
        string finish_title_str = haifuData.finishTitle;
        switch (haifuData.finishType)
        {
            case 0:
                finishType_str = "\"流局\"";
                finishPointShift = new List<int>(){3000, -1000, -1000, -1000};
                finishPointShift_str = "[" + string.Join(",", finishPointShift) + "]";
                finish_str = "[" + finishType_str + "," + finishPointShift_str + "]";
                break;

            case 1:  // ロン  // 最初の3つの数字にバグあり
                finishType_str = "\"和了\"";
                finishPointShift = new List<int>(haifuData.pointShift);
                finishPointShift_str = "[" + string.Join(",", finishPointShift) + "]";
                finish_other_str = "[0,2,0,\"" + finish_title_str + "\"]";
                finish_str = "[" + finishType_str + "," + finishPointShift_str + "," + finish_other_str + "]";
                break;
            
            case 2:  // ツモ  // 最初の3つの数字にバグあり
                finishType_str = "\"和了\"";
                finishPointShift = new List<int>(haifuData.pointShift);
                finishPointShift_str = "[" + string.Join(",", finishPointShift) + "]";
                finish_other_str = "[3,3,3,\"" + finish_title_str + "\"]";
                finish_str = "[" + finishType_str + "," + finishPointShift_str + "," + finish_other_str + "]";
                break;
            
            default:
                break;
        }

        // log文字列リストの作成
        List<string> log_str_list = new List<string>();
        log_str_list.Add(honba_str);
        log_str_list.Add(mochiten_str);
        log_str_list.Add(dora_str);
        log_str_list.Add(uradora_str);
        for (int i = 0; i < 4; i++)
        {
            log_str_list.Add(haipai_str_list[i]);
            log_str_list.Add("[" + string.Join(',', tumo_dahai_str_list[0][i]) + "]");
            log_str_list.Add("[" + string.Join(',', tumo_dahai_str_list[1][i]) + "]");
        }
        log_str_list.Add(finish_str);

        string log_str = "\"log\":[[" + string.Join(',', log_str_list) + "]]";
        //print(log_str);

        // urlの作成
        List<string> url_eles = new List<string>();
        url_eles.Add(title_str);
        url_eles.Add(player_str);
        url_eles.Add(rule_str);
        url_eles.Add(log_str);
        string rtn_url = "https://tenhou.net/5/#json={" + string.Join(',', url_eles) + "}";
        print(rtn_url);

        return rtn_url;
    }

    // 牌id(int)から天鳳のid(string)への変換
    private string HaiId2TenhouHaiIdStr(int haiId)
    {
        int tenhouHaiId = haiId + 10;
        return tenhouHaiId.ToString();
    }

    // 牌idList(List<int>)から天鳳のidList(List<string>)への変換
    private List<string> HaiIds2TenhouHaiIdStrs(List<int> haiIds)
    {
        List<string> rtn_str_list = new List<string>();
        foreach(int haiId in haiIds)
        {
            rtn_str_list.Add(HaiId2TenhouHaiIdStr(haiId));
        }
        return rtn_str_list;
    }

    // 一旦鳴きや上がりを無視して実装
    private List<List<List<string>>> DevideTurnData()
    {
        List<List<string>> tumos = new List<List<string>>();
        List<List<string>> dahais = new List<List<string>>();

        for (int i = 0; i < 4; i++)
        {
            tumos.Add(new List<string>());
            dahais.Add(new List<string>());
        }

        foreach(Turn turn in haifuData.haifus)
        {
            // ツモ牌の登録
            if (turn.actionType == "Normal")
            {
                tumos[turn.playerId].Add(HaiId2TenhouHaiIdStr(turn.tumoHaiId));
            }
            else if (turn.actionType == "Pon")
            {
                tumos[turn.playerId].Add("\"" + string.Join("", turn.furoHaiId) + "\"");
            }
            else if (turn.actionType == "Chi")
            {
                tumos[turn.playerId].Add("\"" + string.Join("", turn.furoHaiId) + "\"");                
            }
            else if (turn.actionType == "Daiminkan")
            {
                tumos[turn.playerId].Add("\"" + string.Join("", turn.furoHaiId) + "\"");
            }

            // 打牌の登録
            if (turn.dahaiActionType == "Normal" || turn.dahaiActionType == "Tumo_finish")
            {
                dahais[turn.playerId].Add(HaiId2TenhouHaiIdStr(turn.dahaiId));
            }
            else if (turn.dahaiActionType == "Reach")
            {
                dahais[turn.playerId].Add("\"r" + HaiId2TenhouHaiIdStr(turn.dahaiId) + "\"");
            }
            else if (turn.dahaiActionType == "Ankan")
            {
                dahais[turn.playerId].Add(makeAnkanString(turn.dahaiId));
            }
        }

        List<List<List<string>>> rtn = new List<List<List<string>>>();
        rtn.Add(tumos);
        rtn.Add(dahais);

        return rtn;
    }

    // アンカンの文字列作成
    private string makeAnkanString(int HaiId)
    {
        string haiId_str = HaiId2TenhouHaiIdStr(HaiId);
        return "\"" + haiId_str + haiId_str + haiId_str + "a" + haiId_str + "\"";
    }

    // ドラの文字列作成
    private string makeDoraString()
    {
        
        List<string> rtn_dora_string_list = new List<string>();
        foreach(int DoraId in haifuData.dora)
        {
            if(DoraId != 0)
            {
                rtn_dora_string_list.Add(HaiId2TenhouHaiIdStr(DoraId));
            }
        }
        return string.Join(",", rtn_dora_string_list);
    }

    // 裏ドラの文字列作成
    private string makeUradoraString()
    {
        List<string> rtn_uradora_string_list = new List<string>();;
        foreach(int UraId in haifuData.uradora)
        {
            if(UraId != 0)
            {
                rtn_uradora_string_list.Add(HaiId2TenhouHaiIdStr(UraId));
            }
        }
        return string.Join(",", rtn_uradora_string_list);
    }

}
