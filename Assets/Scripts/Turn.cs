using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn
{
    public int playerId;
    public int tumoHaiId;
    public int dahaiId;
    public string actionType;
    public string dahaiActionType;
    public List<string> furoHaiId;
    public int dahaiActionHaiId;

    public string log_str()
    {
        string log_s = "";
        log_s += "player id : " + playerId.ToString() + "\n";
        log_s += "tumo : " + tumoHaiId.ToString() + ", dahai : " + dahaiId.ToString() + "\n";
        log_s += "action :" + actionType + ", " + dahaiActionType + ": " + dahaiActionHaiId.ToString();
        return log_s;
    }

}
