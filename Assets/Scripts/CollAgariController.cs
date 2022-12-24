using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// あがりパネルからコントローラーを呼び出す


public class CollAgariController : MonoBehaviour
{
    public AgariController agariController;

    public void CollInitController(int AgariPlayerId, int HoujuPlayerId)
    {
        agariController.InitAgariController(AgariPlayerId, HoujuPlayerId);
    }
}
