using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgariController : MonoBehaviour
{
    // inspectorから数値ボタンを登録
    public List<GameObject> pointButtons;
    public HaifuData haifu;


    private int onPushPoint;
    private int selectedAgariPlayerId;
    private int selectedHoujuPlayerId;

    private LogMessager logMessager;

    //  初期処理
    public void InitAgariController(int AgariPlayerId, int HoujuPlayerId)
    {
        AllPointButtonOff();
        InitDropdown();
        onPushPoint = 0;
        selectedAgariPlayerId = AgariPlayerId;
        selectedHoujuPlayerId = HoujuPlayerId;
        dropdownAgariPlayer.value = AgariPlayerId;
        dropdownHoujuPlayer.value = HoujuPlayerId;

        logMessager = new LogMessager();
    }
    

    // ポイントボタン
    public void OnPushPointButton(int Point)
    {
        logMessager = new LogMessager();
        logMessager.LogY(Point.ToString());
        //AllPointButtonOff();

        onPushPoint = Point;

    }

    //  全てのボタンを初期化
    public void AllPointButtonOff()
    {
        for(int i = 0; i < pointButtons.Count; i++)
        {
            GameObject buttonPoint = pointButtons[i];
            ShapeController shapeController = buttonPoint.GetComponent<ShapeController>();
            shapeController.offButtonShapeChange();
        }
    }

    //-----------------------------------------------------
    //
    //                ドロップダウン
    //
    //-----------------------------------------------------

    public Dropdown dropdownAgariPlayer;
    public Dropdown dropdownHoujuPlayer;

    private List<string> agariPlayerOptionlist = new List<string>();
    private List<string> houjuPlayerOptionlist = new List<string>();

    private void InitDropdown()
    {
        dropdownAgariPlayer.ClearOptions();
        dropdownHoujuPlayer.ClearOptions();

        int i =0;
        List<string> kaze = new List<string>() {"東", "南", "西", "北"};
        foreach (string playerName in haifu.playerNames)
        {
            agariPlayerOptionlist.Add("["+ kaze[i] + "] " + playerName);
            houjuPlayerOptionlist.Add("["+ kaze[i] + "] " + playerName);
            i++;
        }

        dropdownAgariPlayer.AddOptions(agariPlayerOptionlist);
        dropdownHoujuPlayer.AddOptions(houjuPlayerOptionlist);

    }

    //-----------------------------------------------------
    //
    //                決定ボタン
    //
    //-----------------------------------------------------

    public void OnPushFinishEditiing()
    {

        selectedAgariPlayerId = dropdownAgariPlayer.value;
        selectedHoujuPlayerId = dropdownHoujuPlayer.value;
        
        logMessager.LogY(selectedAgariPlayerId.ToString());
    }



}
