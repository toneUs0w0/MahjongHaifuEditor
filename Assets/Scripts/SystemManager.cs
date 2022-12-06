using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    public GameObject taikyokuView;
    public GameObject textFormView;
    public GameObject haipaisettingView;
    public GameObject showOutputView;

    public TaikyokuManager taikyokuManager;
    public HaipaiSettingManager haipaiSettingManager;
    public Text textUrl;

    // Start is called before the first frame update
    void Start()
    {
        taikyokuView.SetActive(false);
        textFormView.SetActive(true);
        haipaisettingView.SetActive(false);
        showOutputView.SetActive(false);
    }

    // 配牌入力後対局入力へ
    public void HaipaiSetting2Taikyoku()
    {
        haipaisettingView.SetActive(false);
        taikyokuView.SetActive(true);

        taikyokuManager.InitTaikyokuView();
    }

    //入力フォームから配牌入力
    public void Form2Haipaisetting()
    {
        haipaisettingView.SetActive(true);
        textFormView.SetActive(false);

        haipaiSettingManager.systemInitialize();
    }

     //入力フォームから対局入力
    public void Form2TaikyokuView()
    {
        taikyokuView.SetActive(true);
        textFormView.SetActive(false);

        taikyokuManager.InitTaikyokuView();
    }

    public void showOutput(string url)
    {
        textUrl.text = url;
        showOutputView.SetActive(true);
    }

}
