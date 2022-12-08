using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChiCandButtonContent : MonoBehaviour
{
    public Image haiImage1;
    public Image haiImage2;
    public Image haiImage3;

    private int haiId1;
    private int haiId2;
    private int haiId3;

    private List<string> index2id = new List<string>() {"none", "m1", "m2", "m3", "m4", "m5", "m6", "m7", "m8", "m9", "m5r", "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8", "p9", "p5r", "s1", "s2", "s3", "s4", "s5", "s6", "s7", "s8", "s9", "s5r", "j1", "j2", "j3", "j4", "j5", "j6", "j7"};
    private List<HaiEntity> haiEnts = new List<HaiEntity>();

    private void InitHaiEntity()
    {
        //haiEntityのリストをロード
        for (int n = 0; n < index2id.Count; n++)
        {
            HaiEntity haiEnt = (HaiEntity)Resources.Load("Hai/" + index2id[n]);
            haiEnts.Add(haiEnt);
        }
    }


    public void SetChiCandImage(int HaiId1, int HaiId2, int HaiId3)
    {
        InitHaiEntity(); // 毎回ロードしたく無いが
        print(HaiId1.ToString() + " " + HaiId2.ToString()+ " " + HaiId3.ToString());
        print(haiEnts.Count);
        haiId1 = HaiId1;
        haiId2 = HaiId2;
        haiId3 = HaiId3;
        haiImage1.sprite = haiEnts[HaiId1].haiSprite;
        haiImage2.sprite = haiEnts[HaiId2].haiSprite;
        haiImage3.sprite = haiEnts[HaiId3].haiSprite;
    }

    public void PushChiCandButton()
    {
        GameObject objTaikyokuManager = GameObject.Find("TaikyokuManager");
        TaikyokuManager taikyokuManager = objTaikyokuManager.GetComponent<TaikyokuManager>();
        taikyokuManager.ChiInput(haiId1, haiId2, haiId3);
    }

}
