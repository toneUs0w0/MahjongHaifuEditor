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
}
