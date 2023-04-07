using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointbuttonColorChange : MonoBehaviour
{
    public AgariController agariController;

    // 全てのボタンにアタッチ
    public void ToGreen()
    {
        // 数値的な処理ではなくみための処理
        
        ShapeController shapeController = this.GetComponent<ShapeController>();
        shapeController.onButtonGreen();
    }
}
