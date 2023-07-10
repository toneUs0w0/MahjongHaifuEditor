using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HaipaiSetting : MonoBehaviour
{
    [SerializeField] Text text_player_name;
    [SerializeField] HaifuData haifu;
    [SerializeField] GameObject panelHaipaiSetting;
    [SerializeField] List<GameObject> haipai_hai_image_list;

    private List<HaiEntity> haiEnts = new List<HaiEntity>(); 
    private List<string> index2id = new List<string>() {"none", "m1", "m2", "m3", "m4", "m5", "m6", "m7", "m8", "m9", "m5r", "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8", "p9", "p5r", "s1", "s2", "s3", "s4", "s5", "s6", "s7", "s8", "s9", "s5r", "j1", "j2", "j3", "j4", "j5", "j6", "j7"};

    private int turn_player;
    private int selected_iamge_id;
    private List<int> haipai_list;

    private LogMessager logMessager;

    private void Start() 
    {
        turn_player = 0;
        selected_iamge_id = 0;
        haipai_list = new List<int>();
        //InitHaipaiSetting();
    }

    public void InitHaipaiSetting(int turn_player_id = 0)
    {
        // インスペクタから牌譜を受け取る
        panelHaipaiSetting.SetActive(true);
        InitHaiEntity();
        print("Init Haipai Setting Mode");
        // print(haifu.HaifuLogStr());

        turn_player = turn_player_id;
        haipai_list = new List<int>(haifu.haipai[turn_player]);
        text_player_name.text = haifu.playerNames[turn_player];
        
        ShowHaipaiFromHaifuData();
        selected_iamge_id = DecideInitCursor();
        SetCursor();
    }

    public void CloseHaipaiSetting()
    {
        panelHaipaiSetting.SetActive(false);   
    }

    public void haiButtonInput(int haiId)
    {
        haipai_list[selected_iamge_id] = haiId;
        haipai_hai_image_list[selected_iamge_id].GetComponent<Image>().sprite = haiEnts[haiId].haiSprite;

        selected_iamge_id = DecideInitCursor(cursor:selected_iamge_id);
        SetCursor();
    }

    public void PushHaiMoveArea(int area_id)
    {
        selected_iamge_id = area_id;
        SetCursor();
    }

    public void PushSortButton()
    {
        SortHaipai();
        ShowHaipaiFromHaifuData();
        selected_iamge_id = DecideInitCursor();
        SetCursor();
    }

    public void PushReturnButton()
    {
        SortHaipai();
        haifu.haipai[turn_player] = new List<int>(haipai_list);
        CloseHaipaiSetting();
        GetComponent<TaikyokuManager>().TurnOffHaipaiSettingMode();
    }

    // 0を後ろに回したソート
    private void SortHaipai()
    {
        int pos = 0;
        int delete = 0;
        List<float> tmp_tehai = new List<float>();
        foreach (int int_tehai_num in haipai_list)
        {
            if (int_tehai_num == 10 | int_tehai_num == 20 | int_tehai_num == 30)
            {
                tmp_tehai.Add((float)int_tehai_num - (float)4.5);
            }
            else
            {
                tmp_tehai.Add((float)int_tehai_num);
            }
        }
        tmp_tehai.Sort();
        for (int j = 0; j < tmp_tehai.Count; j++)
        {
            float tId = tmp_tehai[j];
            if (tId == 0)
            {
                delete++;
            }
            else
            {
                //print("tehailength : " + tmp_tehai.Count.ToString());
                //print("pos : " + pos.ToString());

                tmp_tehai[pos] = tId;
                pos++;
            }
        }
        for (int k = pos; k < tmp_tehai.Count; k++)
        {
            tmp_tehai[k] = 0;
        }
        List<int> tmp_tehai2 = new List<int>();
        foreach (float float_hai_num in tmp_tehai)
        {
            if (float_hai_num % 1 != 0)
            {
                tmp_tehai2.Add((int)(float_hai_num + 4.5));
            }
            else
            {
                tmp_tehai2.Add((int)float_hai_num);
            }
        }


        haipai_list = new List<int>(tmp_tehai2);
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

    private void ShowHaipaiFromHaifuData(int cursor = 0)
    {
        if (haipai_list.Count != 13 || haipai_hai_image_list.Count != 13)
        {
            logMessager.LogR("ERROR! : HAIPAI DATA IS BROKEN");
        }
        for (int i = 0; i < 13; i ++)
        {
            haipai_hai_image_list[i].GetComponent<Image>().sprite = haiEnts[haipai_list[i]].haiSprite;
        }
    }

    private int DecideInitCursor(int cursor = 0)
    {
        int rtn = 0;
        if (cursor >= 13 || cursor < 0)
        {
            cursor = 0;
        }
        for (int i = cursor; i < 13; i ++)
        {
            if (haipai_list[i] == 0)
            {
                rtn = i;
                break;
            }
        }
        return rtn;
    }

    private void DisapearCorsor()
    {
        foreach (GameObject hai_obj in haipai_hai_image_list)
        {
            hai_obj.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        }
    }

    private void SetCursor()
    {
        DisapearCorsor();
        Vector3 cursorPos = new Vector3(0.0f, 40.0f, 0.0f);
        haipai_hai_image_list[selected_iamge_id].GetComponent<RectTransform>().anchoredPosition = cursorPos;
    }


}
