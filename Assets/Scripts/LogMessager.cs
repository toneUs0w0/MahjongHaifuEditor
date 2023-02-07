using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// デバッグ用のログを作成
public class LogMessager
{
    public void LogY(string contents)
    {
        string logtext = "<color=yellow>" + contents + "</color>";
        Debug.Log(logtext);
    }

    public void LogR(string contents)
    {
        string logtext = "<color=red>" + contents + "</color>";
        Debug.Log(logtext);
    }


    public void LogG(string contents)
    {
        string logtext = "<color=green>" + contents + "</color>";
        Debug.Log(logtext);
    }
}
