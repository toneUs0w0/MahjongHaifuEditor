using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveFileLoader : MonoBehaviour
{
    public List<HaifuInfo> haifuInfoList;
    public List<string> haifuInfoFileNames;
    private string SAVED_HAIFU_INFO_DIR = "Datas/SavedHaifuInfos/";
    private string SAVED_INFO_FILE_NAMES_LIST_TEXT = "Datas/savedFileNames";
    private int SAVED_HAIFU_INFO_ELEMENT_SIZE = 10;
    private LogMessager logMessager;

    private void Start() 
    {
        logMessager = new LogMessager();
        haifuInfoList = new List<HaifuInfo>();
        haifuInfoFileNames = new List<string>();
    }

    // import info file names from Datas/savedFileNames
    private void haifuInfoFileNamesLoad()
    {
        haifuInfoFileNames = new List<string>(); //clear
        var info_file_names_textf = Resources.Load<TextAsset>(SAVED_INFO_FILE_NAMES_LIST_TEXT) as TextAsset;
        string info_file_names_textf_st = info_file_names_textf.text;
        string[] haifuinfo_filename_list = info_file_names_textf_st.Split("\n");
        foreach(string filenam in haifuinfo_filename_list)
        {
            if (filenam == "")
            {
                continue;
            }
            haifuInfoFileNames.Add(filenam);
        }

    }

    public void SavedHaifuInfosLoad()
    {
        // delete all haifuInfos
        haifuInfoList = new List<HaifuInfo>();

        // get haifu Info File Names
        haifuInfoFileNamesLoad();

        // load
        foreach (string haifuInfoFileName in haifuInfoFileNames)
        {
            SavedHaifuInfoLoad(haifuInfoFileName);
        }
    }

    private void SavedHaifuInfoLoad(string HaifuInfoFileName)
    {
        print(HaifuInfoFileName);
        HaifuInfo haifuInfo = new HaifuInfo();

        var _saved_haifu_info = Resources.Load<TextAsset>(SAVED_HAIFU_INFO_DIR + HaifuInfoFileName) as TextAsset;
        string _saved_haifu_info_string = _saved_haifu_info.text;
        string[] _saved_haifu_info_elements = _saved_haifu_info_string.Split(",");
        // print(_saved_haifu_info_elements[0]);

        if (_saved_haifu_info_elements.Length != SAVED_HAIFU_INFO_ELEMENT_SIZE)
        {
            logMessager.LogR("HAIFU INFO FILE IS BROKEN!");
            // ここの処置は後で記述する予定
        }

        // infoファイルとhaifuファイルに差がないかチェックする必要がある

        haifuInfo.file_name = _saved_haifu_info_elements[0];
        haifuInfo.sub_title = _saved_haifu_info_elements[1];
        haifuInfo.title = _saved_haifu_info_elements[2];
        haifuInfo.kyoku = int.Parse(_saved_haifu_info_elements[3]);
        haifuInfo.honba = int.Parse(_saved_haifu_info_elements[4]);
        haifuInfo.player1 = _saved_haifu_info_elements[5];
        haifuInfo.player2 = _saved_haifu_info_elements[6];
        haifuInfo.player3 = _saved_haifu_info_elements[7];
        haifuInfo.player4 = _saved_haifu_info_elements[8];
        haifuInfo.date = _saved_haifu_info_elements[9];

        haifuInfoList.Add(haifuInfo);
        logMessager.LogG("haifu info :" + haifuInfo.title + " is loaded.");

    }
    
}
