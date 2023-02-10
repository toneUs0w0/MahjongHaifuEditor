using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForDebug : MonoBehaviour
{
    public void PushDebugButton()
    {
        string PATH = "Assets/Resources/Datas/SavedHaifu/" + "test.txt";
        JsonFileGenerator jfg = new JsonFileGenerator();
        HaifuData haifu = jfg.LoadFile(PATH);
        print(haifu.taikyokuName);
        print(haifu.taikyokuSubTitle);
        print(haifu.playerNames[0]);
        print(haifu.playerNames[1]);
        print(haifu.playerNames[2]);
        print(haifu.playerNames[3]);
        print(haifu.ruleAka.ToString());
        print(haifu.kyoku.ToString());
        print(haifu.honba.ToString());
        print(haifu.kyoutaku.ToString());
        print(haifu.oyaId.ToString());
        print(haifu.mochiten[0].ToString());
        print(haifu.dora[0]);
        print(haifu.uradora[0]);
        print(string.Join(", ", haifu.haipai[0]));
        print(string.Join(", ", haifu.haipai[1]));
        print(string.Join(", ", haifu.haipai[2]));
        print(string.Join(", ", haifu.haipai[3]));
    }


}
