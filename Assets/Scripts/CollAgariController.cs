using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// あがりパネルからコントローラーを呼び出す


public class CollAgariController : MonoBehaviour
{
    public AgariController agariController;
    public TumoAgariController tumoAgariController;

    public void CollInitController(int AgariPlayerId, int HoujuPlayerId, int mode = 0)
    {
        switch(mode)
        {   case 0:
                agariController.InitAgariControllerRon(AgariPlayerId, HoujuPlayerId);
                break;
            case 1:
                tumoAgariController.InitAgariControllerTumo(AgariPlayerId);
                break;
            default:
                break;
        }
        
    }
}
