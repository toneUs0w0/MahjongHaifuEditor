using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFileSelectController : MonoBehaviour
{

    [SerializeField] public SystemManager systemManager;
    [SerializeField] public GameObject saveDataContentsPanel;

    private GameObject saveDataPrefab;
    private List<string> haifuFilenamesList;

    public void InitSaveFileSelectView()
    {
        haifuFilenamesList = new List<string>();
    }

    // リストのファイルを全てcontents panelに追加
    private void AddAllSaveData2ContensPanel()
    {
        foreach (string haifuFileName in haifuFilenamesList)
        {
            AddSaveDataContent(haifuFileName);
        }
    }

    //contentを追加
    private void AddSaveDataContent(string HaifuFileName)
    {

    }

    // contentを押した際の動作
    public void PushReturnButtonFromSaveSelectView()
    {
        systemManager.SaveSelect2Title();
    }
}
