using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFileSelectController : MonoBehaviour
{

    [SerializeField] SystemManager systemManager;
    [SerializeField] GameObject saveDataContentsPanel;
    [SerializeField] GameObject objSaveFileLoader;
    //[SerializeField] public SaveDataContentController saveDataContentController;
    [SerializeField] GameObject saveDataContentPrefab;
    [SerializeField] GameObject haifuPrefab;
    private LogMessager logMessager;

    private GameObject saveDataPrefab;
    private List<string> haifuFilenamesList = new List<string> () {"debug", "debug"};
    private List<SaveDataContentController> savedataControllers;

    private HaifuData viewingHaifuData;  // 展開する牌譜データ
    private GameObject viewingHaifu_obj;


    [SerializeField] GameObject haifuViewerView;

    public void InitSaveFileSelectView()
    {
        haifuFilenamesList = new List<string>();
        savedataControllers = new List<SaveDataContentController>();

        viewingHaifu_obj = Instantiate(haifuPrefab);
        viewingHaifuData = viewingHaifu_obj.GetComponent<HaifuData>();

        ClearAllContents();
        AddAllSaveData2ContensPanel();
    }

    private void ClearAllContents()
    {
        Transform _parentTransform = saveDataContentsPanel.GetComponent<Transform>();
        for (int index = 0; index < _parentTransform.childCount; index++)
        {
            print(_parentTransform.GetChild(index).gameObject);
            Destroy(_parentTransform.GetChild(index).gameObject);
        }
    }

    // リストのファイルを全てcontents panelに追加
    private void AddAllSaveData2ContensPanel()
    {
        SaveFileLoader saveFileLoader  = objSaveFileLoader.GetComponent<SaveFileLoader>();
        saveFileLoader.SavedHaifuInfosLoad(); // データのロード
        foreach (HaifuInfo haifuInfo in saveFileLoader.haifuInfoList)  // haifuInfoをcontentに追加
        {
            AddSaveDataContent(haifuInfo);
        }
    }

    //contentを追加
    private void AddSaveDataContent(HaifuInfo _HaifuInfo)
    {
        Transform _parentTransform = saveDataContentsPanel.GetComponent<Transform>();
        GameObject saveDataContent = Instantiate(saveDataContentPrefab, _parentTransform);
        SaveDataContentController saveDataContentController = saveDataContent.GetComponent<SaveDataContentController>();
        saveDataContentController.InitSaveDataContent(_HaifuInfo);
        
        savedataControllers.Add(saveDataContentController);

    }

    public void AllSaveDataContent2Untouched()
    {
        foreach(SaveDataContentController savedataController in savedataControllers)
        {
            savedataController.ShowSecondButton(false);
        }
    }

    // contentを押した際の動作
    public void PushReturnButtonFromSaveSelectView()
    {
        systemManager.SaveSelect2Title();
    }


    public void FileSelect2HaifuViewer(string filename)
    {
        // 牌譜オブジェクトの作成
        JsonFileGenerator jfg = new JsonFileGenerator();
        viewingHaifuData.InitHaifuData(jfg.LoadFile("test.txt")); // 牌譜データのロード
        
    }

    private void InitHaifuViewerView()
    {

    }
}