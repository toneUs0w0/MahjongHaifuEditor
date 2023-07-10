using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text.RegularExpressions;



public class JsonFileGenerator
{
    private string SAVE_DATA_PATH = "Assets/Resources/Datas/SavedHaifu/";
    private string SAVE_DATA_INFO_PATH = "Assets/Resources/Datas/SavedHaifuInfos/";
    private string SAVED_INFO_FILE_NAMES_LIST_TEXT = "Assets/Resources/Datas/savedFileNames.txt";
    private string HAIFU_URL = "";
    private HaifuData haifuData;



    // 牌譜URLを保存
    public void SaveFile(HaifuData haifu, string file_name = "")
    {
        string file_num;

        if (file_name != "")
        {
            file_num = file_name;
        }
        else
        {
            DateTime dt = DateTime.Now;
            file_num = dt.ToString("yyyy-MM-dd-HH-mm-ss");
        }

        haifuData = haifu;
        HAIFU_URL = CreateHaifuUrl();
        string haifu_info_conts = CreateHaifuInfoContent(haifu, file_num);
        File.WriteAllText(SAVE_DATA_PATH + file_num + ".txt", HAIFU_URL);      // haifu file
        File.WriteAllText(SAVE_DATA_INFO_PATH + file_num + ".txt", haifu_info_conts);  // info file
        File.AppendAllText(SAVED_INFO_FILE_NAMES_LIST_TEXT, file_num+"\n");


    }


    // 牌譜URLファイルをロード
    public HaifuData LoadFile(string filename)
    {
        string path = SAVE_DATA_PATH + filename;
        HaifuData rtnHaifu = new HaifuData();
        string _str_data = File.ReadAllText(path);
        rtnHaifu = LoadsHaifuFromUrl(_str_data);

        return rtnHaifu;

    }

    // make haifu infos file
    private string CreateHaifuInfoContent(HaifuData haifu, string haifu_name)
    {
        List<string> conts = new List<string> { haifu_name , haifu.taikyokuName ,
            haifu.taikyokuSubTitle, haifu.honba.ToString(), haifu.kyoutaku.ToString(),
            haifu.playerNames[0], haifu.playerNames[1], haifu.playerNames[2], haifu.playerNames[3], haifu_name};

        string rtn_str = string.Join(",", conts);
        return rtn_str;
    }

    // 牌譜URLを作成
    private string CreateHaifuUrl()
    {
        string title_str = "\"title\":[\"" + haifuData.taikyokuName + "\",\"" + haifuData.taikyokuSubTitle + "\"]";
        int front_player_id = (20 - haifuData.kyoku) % 4;
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
            case 0:    // テンパイ料は未実装
                finishType_str = "\"流局\"";
                finishPointShift = new List<int>(){3000, -1000, -1000, -1000};
                finishPointShift_str = "[" + string.Join(",", finishPointShift) + "]";
                finish_str = "[" + finishType_str + "," + finishPointShift_str + "]";
                break;

            case 1:  // ロン  // 最初の3つの数字にバグあり  //おそらく解決
                finishType_str = "\"和了\"";
                finishPointShift = new List<int>(haifuData.pointShift);
                finishPointShift_str = "[" + string.Join(",", finishPointShift) + "]";
                finish_other_str = "[" + haifuData.finishPlayerId.ToString() + "," + haifuData.houjuPlayerId.ToString() + "," + haifuData.finishPlayerId.ToString() + ",\"" + finish_title_str + "\"]";
                finish_str = "[" + finishType_str + "," + finishPointShift_str + "," + finish_other_str + "]";
                break;
            
            case 2:  // ツモ  // 最初の3つの数字にバグあり  //おそらく解決
                finishType_str = "\"和了\"";
                finishPointShift = new List<int>(haifuData.pointShift);
                finishPointShift_str = "[" + string.Join(",", finishPointShift) + "]";
                finish_other_str = "[" + haifuData.finishPlayerId.ToString() + "," + haifuData.finishPlayerId.ToString() + "," + haifuData.finishPlayerId.ToString() + ",\"" + finish_title_str + "\"]";
                finish_str = "[" + finishType_str + "," + finishPointShift_str + "," + finish_other_str + "]";
                tumo_dahai_str_list[1][haifuData.finishPlayerId].RemoveAt(tumo_dahai_str_list[1][haifuData.finishPlayerId].Count - 1);  // ツモあがりの場合は末尾の打牌を削除

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




    private HaifuData LoadsHaifuFromUrl(string haifus)
    {
        // 牌譜データはmonoなのでこれはまずい
        HaifuData haifu = new HaifuData();

        // タイトルとサブタイトル  // タイトルが無い場合の確認は後で記述予定
        var _rt = Regex.Match(haifus, "\"title\".*?]");
        var _rt_v = _rt.Value.Substring(9);
        string[] _rt_v_tit_sub = Regex.Match(_rt_v, "\".*\"").Value.Split(",");
        haifu.taikyokuName =  _rt_v_tit_sub[0].Substring(1, _rt_v_tit_sub[0].Length-2);
        haifu.taikyokuSubTitle = _rt_v_tit_sub[1].Substring(1, _rt_v_tit_sub[1].Length-2);


        // プレイヤー名
        var _rp = Regex.Match(haifus, "\"name\".*?]");
        var _rp_v = _rp.Value.Substring(8);
        string[] _rt_pns = Regex.Match(_rp_v, "\".*\"").Value.Split(",");
        haifu.playerNames[0] = _rt_pns[0].Substring(1, _rt_pns[0].Length-2);
        haifu.playerNames[1] = _rt_pns[1].Substring(1, _rt_pns[1].Length-2);
        haifu.playerNames[2] = _rt_pns[2].Substring(1, _rt_pns[2].Length-2);
        haifu.playerNames[3] = _rt_pns[3].Substring(1, _rt_pns[3].Length-2);

        // ルール
        var _rr = Regex.Match(haifus, "\"rule\".*?]");
        var _rr_v = Regex.Match(_rr.Value, "\"aka\":[0-9]*").Value;
        haifu.ruleAka = int.Parse(_rr_v.Substring(6));

        // ログ
        string _log_v = Regex.Match(haifus, "\"log\":.*]").Value;
        string _log_all_content = Regex.Match(_log_v, "\\[.*\\]").Value;
        MatchCollection results = Regex.Matches(_log_all_content, "\\[.*?\\]");
        //haifu.taikyokuName = results[16].Value;
        //haifu.taikyokuSubTitle = results[17].Value;

        //局id, 本場, 供託
        string[] _log_kht = Regex.Match(results[0].Value, "[0-9]*,[0-9]*,[0-9]*").Value.Split(",");
        haifu.kyoku = int.Parse(_log_kht[0]);
        haifu.honba = int.Parse(_log_kht[1]);
        haifu.kyoutaku = int.Parse(_log_kht[2]);

        // 親プレイヤー
        haifu.oyaId = haifu.kyoku % 4;

        // 持ち点
        string[] _log_mochi = Regex.Match(results[1].Value, "[0-9]*,[0-9]*,[0-9]*,[0-9]*").Value.Split(",");
        haifu.taikyokuSubTitle = _log_mochi[0];
        haifu.mochiten[0] = int.Parse(_log_mochi[0]);
        haifu.mochiten[1] = int.Parse(_log_mochi[1]);
        haifu.mochiten[2] = int.Parse(_log_mochi[2]);
        haifu.mochiten[3] = int.Parse(_log_mochi[3]);

        // ドラ表示  // 赤ドラどうしよう
        MatchCollection results_dora = Regex.Matches(results[2].Value, "[0-9]+");
        int n = 0;
        foreach(Match m in results_dora)
        {
            haifu.dora[n] = TenhouHaiId2HaiId(m.Value);
            n ++;
        }

        // 裏ドラ表示  // 赤ドラどうしよう
        MatchCollection results_uradora = Regex.Matches(results[3].Value, "[0-9]+");
        n = 0;
        foreach(Match m in results_uradora)
        {
            haifu.uradora[n] = TenhouHaiId2HaiId(m.Value);
            n ++;
        }

        // 配牌
        int[] _log_haipai_index = {4, 7, 10, 13};
        string[] _log_haipai;
        List<List<int>> haipai = new List<List<int>>();
        List<int> haipai_p = new List<int>();
        foreach(int h in _log_haipai_index)
        {
            _log_haipai = results[h].Value.Substring(1, results[h].Length-2).Split(",");
            haipai_p = new List<int>();
            foreach (string tId in _log_haipai)
            {
                haipai_p.Add(TenhouHaiId2HaiId(tId));
            }
            haipai.Add(haipai_p);
        }
        haifu.haipai = new List<List<int>>(haipai);


        // ツモ牌リスト
        int[] _log_tumo_index = {5, 8, 11, 14};
        string[] _log_tumo;
        List<List<int>> tumo = new List<List<int>>();
        List<int> tumo_p = new List<int>();
        foreach(int h in _log_tumo_index)
        {
            _log_tumo = results[h].Value.Substring(1, results[h].Length-2).Split(",");
            tumo_p = new List<int>();
            foreach (string tId in _log_tumo)
            {
                tumo_p.Add(TenhouHaiId2HaiId(tId));
            }
            tumo.Add(tumo_p);
        }


        // 打牌リスト
        int[] _log_dahai_index = {6, 9, 12, 15};
        string[] _log_dahai;
        List<List<int>> dahai = new List<List<int>>();
        List<int> dahai_p = new List<int>();
        foreach(int h in _log_dahai_index)
        {
            _log_dahai = results[h].Value.Substring(1, results[h].Length-2).Split(",");
            dahai_p = new List<int>();
            foreach (string tId in _log_dahai)
            {
                dahai_p.Add(TenhouHaiId2HaiId(tId));
            }
            dahai.Add(dahai_p);
        }


        // ターン処理  // 鳴きの処理を加筆する必要がある
        int p_id = haifu.oyaId;
        int[] tumo_start_index = {0, 0, 0, 0};
        int[] dahai_start_index = {0, 0, 0, 0};

        int tumo_haiId = 0;
        int dahai_haiId = 0;
        bool isRon;

        Turn turn = new Turn();

        // 修正予定
        while(true)
        {
            //ツモ牌
            turn = new Turn();

            // ツモ牌が足りない場合
            if (tumo_start_index[p_id] >= tumo[p_id].Count)
            {
                isRon = true;
                break;
            }

            tumo_haiId = tumo[p_id][tumo_start_index[p_id]];
            tumo_start_index[p_id]++;

            //打牌

            // 打牌が足りない場合 -> ツモあがり 
            if (dahai_start_index[p_id] >= dahai[p_id].Count)
            {
                isRon = false;
                turn.playerId = p_id;
                turn.tumoHaiId = tumo_haiId;
                turn.dahaiId = 0;
                turn.actionType = "Tumo_finish";

                haifu.haifus.Add(turn);
                break;
            }

            dahai_haiId = dahai[p_id][dahai_start_index[p_id]];
            dahai_start_index[p_id]++;

            turn.playerId = p_id;
            turn.tumoHaiId = tumo_haiId;
            turn.dahaiId = dahai_haiId;
            turn.actionType = "Normal";

            haifu.haifus.Add(turn);

            p_id = (p_id + 1) % 4;
        }


        // 終局方法
        string _log_finishtag_piontshift = results[16].Value;
        string _finish_tag = Regex.Match(_log_finishtag_piontshift, "\".*?\"").Value;
        _finish_tag = _finish_tag.Substring(1, _finish_tag.Length - 2);
        if (_finish_tag == "流局")
        {
            haifu.finishType = 0;
        }
        else if (_finish_tag == "和了")
        {
            if (isRon)
            {
                haifu.finishType = 1;
            }
            else
            {
                haifu.finishType = 2;
            }
        }

        // 点数移動
        string[] _point_shift_str = Regex.Match(_log_finishtag_piontshift, "-*[0-9]+,-*[0-9]+,-*[0-9]+,-*[0-9]+").Value.Split(",");
        for(int i = 0; i < 4; i++)
        {
            haifu.pointShift[i] = int.Parse(_point_shift_str[i]);
        }

        // Additional
        if (haifu.finishType != 0)  // 流局の場合は考えない
        {
            string _fisish_add_str = Regex.Match(results[17].Value, "[0-9]+,[0-9]+,[0-9]+,\".*\"").Value;
            string[] _fin_str_tags = _fisish_add_str.Split(",");
            if (isRon)
            {
                haifu.finishPlayerId = int.Parse(_fin_str_tags[0]);
                haifu.houjuPlayerId = int.Parse(_fin_str_tags[1]);
            }
            else
            {
                haifu.finishPlayerId = int.Parse(_fin_str_tags[0]);
            }
            string finish_title =  Regex.Match(_fisish_add_str, "\".*\"").Value;
            haifu.finishTitle = finish_title;
        }
        
        
        //haifu.taikyokuName =  _fisish_add_str;
        //haifu.taikyokuSubTitle = results[17].Value;




        return haifu;

    }


    // 牌id(int)から天鳳のid(string)への変換
    private int TenhouHaiId2HaiId(string tenhouId)
    {
        int HaiId = int.Parse(tenhouId) - 10;
        return HaiId;
    }


}
