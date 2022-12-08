using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HaipaiSettingManager : MonoBehaviour
{
    public SystemManager systemManager;

    public Text textPlayerName;
    public HaifuData haifuData;
    public List<GameObject> objTehai; // インスペクタで登録
    public Sprite tmpHaiSprite;
    private int settingPlayerId;

    public List<int> tehaiIdList;      // 現在表示している手牌のidリスト privateの方がいいかも
    private List<List<int>> haipaiIdList;
    private List<string> index2id = new List<string>() {"none", "m1", "m2", "m3", "m4", "m5", "m6", "m7", "m8", "m9", "m5r", "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8", "p9", "p5r", "s1", "s2", "s3", "s4", "s5", "s6", "s7", "s8", "s9", "s5r", "j1", "j2", "j3", "j4", "j5", "j6", "j7"};
    public List<HaiEntity> haiEnts = new List<HaiEntity>(); // private

    private void Start() {
        systemInitialize();

    }


    // 初期化
    public void systemInitialize()
    {
        //haiEntityのリストをロード
        for (int n = 0; n < index2id.Count; n++)
        {
            HaiEntity haiEnt = (HaiEntity)Resources.Load("Hai/" + index2id[n]);
            haiEnts.Add(haiEnt);
        }
        // 手牌の初期化
        tehaiIdList = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        // 配牌の初期化
        haipaiIdList = new List<List<int>>();
        for (int i = 0; i < 4; i ++)
        {
            haipaiIdList.Add(new List<int>(tehaiIdList));
        }
        // playerIdの初期化
        settingPlayerId = 0;
        // playerNameの表示
        SetPlayerName();
        // 手牌の表示
        SetTehai();

    }

    // 手牌の初期化
    void TehaiInit()
    {
        tehaiIdList = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    }

    // ランダムに13枚の手牌を生成
    private void TehaiMaking()
    {
        for(int i = 0; i < 13; i++)
        {
            int randId = Random.Range(1, 20);
            tehaiIdList[i] = randId;
        }
        tehaiIdList.Sort();
        SetTehai();

    }

    // tehaiIdListをtehaiにセット
    private void SetTehai()
    {
        int pos = 0;
        int delete = 0;
        for (int i = 0; i < tehaiIdList.Count; i++)
         {
            int tehaiId = tehaiIdList[i];
            if (tehaiId == 0)
            {
                delete++;
            }
            else
            {
                SetHai2Tehai(TehaiPos: pos, HaiId: tehaiId);
                pos++;
            }
         }
        for (int j = pos; j < tehaiIdList.Count; j++)
        {
            SetHai2Tehai(TehaiPos: j, HaiId: 0);
        }
    }

    // TehaiPosにHaiIdのimageを挿入
    // HaiId = 0は削除
    private void SetHai2Tehai(int TehaiPos, int HaiId)
    {
        Image imageTehai = objTehai[TehaiPos].GetComponent<Image>();
        imageTehai.sprite = haiEnts[HaiId].haiSprite;
    }

    // 手牌の枚数を返す
    private int TehaiLength()
    {
        int rtn = 0;
        for(int i = 0; i < tehaiIdList.Count; i++)
        {
            if (tehaiIdList[i] != 0)
            {
                rtn++;
            } 
        }
        return rtn;
    }

    // 0を後ろに回したソート
    private void SortTehai()
    {
        int pos = 0;
        int delete = 0;
        tehaiIdList.Sort();
        for (int i = 0; i < tehaiIdList.Count; i++)
        {
            int tehaiId = tehaiIdList[i];
            if (tehaiId == 0)
            {
                delete++;
            }
            else
            {
                tehaiIdList[pos] = tehaiId;
                pos++;
            }
        }
        for (int j = pos; j < tehaiIdList.Count; j++)
        {
            tehaiIdList[j] = 0;
        }
    }

    // 現在のtehaiId列を配牌に登録する
    private void SaveTehaiId2HaipaiList()
    {
        haipaiIdList[settingPlayerId] = new List<int>(tehaiIdList);
    }

    // 配牌から手牌Id列にコピー
    private void LoadTehaiId()
    {
        tehaiIdList = new List<int> (haipaiIdList[settingPlayerId]);
    }


    // 消去ボタン
    public void PushDeleteButton()
    {
        if (TehaiLength() == 0)
        {
            return;
        }
        tehaiIdList[TehaiLength() - 1] = 0;
        SetTehai();
    }

    // ソートボタン
    public void PushSortButton()
    {
        SortTehai();
        SetTehai();
    }


    public void PushHaiButton(int HaiId)
    {
        if (TehaiLength() >= tehaiIdList.Count)
        {
            return;
        }
        tehaiIdList[TehaiLength()] = HaiId;
        SetTehai();
    }

    public void PushNextButton()
    {
        SaveTehaiId2HaipaiList();
        settingPlayerId = (settingPlayerId + 1) % 4;
        LoadTehaiId();
        SetTehai();
        SetPlayerName();
    }

    private void SetPlayerName()
    {
        haifuData = haifuData.GetComponent<HaifuData>();
        textPlayerName.text = haifuData.playerNames[settingPlayerId];
    }

    // 牌譜データに配牌を登録し対局入力へ
    public void PushFinishButton()
    {
        // 最後に表示している手牌をhaipaiIdListに記録
        SaveTehaiId2HaipaiList();

        for (int i = 0; i < 4; i ++)
        {
            haifuData.haipai[i] = new List<int>(haipaiIdList[i]);
        }

        systemManager.HaipaiSetting2Taikyoku();
    }
}
