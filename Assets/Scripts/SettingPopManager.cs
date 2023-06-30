using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPopManager : MonoBehaviour
{
    [SerializeField] SystemManager systemManager;

    [SerializeField] GameObject panelBlocker;
    [SerializeField] GameObject panelSettingBellow;
    [SerializeField] GameObject popReturnTitle;

    bool setting_bellow_is_shown = false;
    bool blocker_is_shown = false;


    public void PushThreeDots()
    {
        SetSettingBellow(true);
        SetBlocker(true);
    }

    // 牌譜情報の入力
    public void PushEditInfo()
    {
        SetSettingBellow(false);
        SetBlocker(false);
    }

    // 入力の中断
    public void PushSaveAndFinish()
    {
        SetSettingBellow(false);
        SetBlocker(false);
    }

    // タイトルに戻る
    public void PushCancel()
    {
        SetSettingBellow(false);
        SetPopReturnTitle(true);
    }

    // やっぱりタイトルに戻る
    public void PushReturnToTitleOnPop()
    {
        // 牌譜データを削除
        // タイトルに戻る
        systemManager.TaikyokuView2Title();
    }

    public void PushBlocker()
    {
        SetSettingBellow(false);
        SetPopReturnTitle(false);
        SetBlocker(false);
    }

    private void SetSettingBellow(bool set_or_not)
    {
        panelSettingBellow.SetActive(set_or_not);
        setting_bellow_is_shown = set_or_not;
    }

    private void SetBlocker(bool set_or_not)
    {
        panelBlocker.SetActive(set_or_not);
        blocker_is_shown = set_or_not;
    }

    private void SetPopReturnTitle(bool set_or_not)
    {
        popReturnTitle.SetActive(set_or_not);
    }


}
