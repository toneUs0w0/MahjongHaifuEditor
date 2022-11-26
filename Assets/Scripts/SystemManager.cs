using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    public List<GameObject> objTehai;
    public Sprite tmpHaiSprite;

    public List<int> tehaiIdList;      // 手牌のidリスト privateの方がいいかも
    private List<string> index2id = new List<string>() {"none", "m1", "m2", "m3", "m4", "m5", "m6", "m7", "m8", "m9"};
    public List<HaiEntity> haiEnts = new List<HaiEntity>(); // private

    // Start is called before the first frame update
    void Start()
    {
        systemInitialize();
        TehaiInit();
        TehaiMaking();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 初期化
    void systemInitialize()
    {
        //haiEntityのリストをロード
        for (int n = 0; n < index2id.Count; n++)
        {
            HaiEntity haiEnt = (HaiEntity)Resources.Load("Hai/" + index2id[n]);
            haiEnts.Add(haiEnt);
        }
    }

    // 手牌の初期化
    void TehaiInit()
    {
        tehaiIdList = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }

    // ランダムに14枚の手牌を生成
    private void TehaiMaking()
    {
        for(int i = 0; i < 14; i++)
        {
            int randId = Random.Range(1, 10);
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


}
