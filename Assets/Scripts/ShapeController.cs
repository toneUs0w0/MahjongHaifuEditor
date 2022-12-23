using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeController : MonoBehaviour
{
    private float darkGrayInt = 30;
    private float rightGrayInt = 200;

    public void onButtonShapeChange()
    {
        Shapes2D.Shape shape = GetComponent<Shapes2D.Shape>();
        float darkGrayFloat = darkGrayInt/256;
        shape.settings.fillColor = new Color(darkGrayFloat, darkGrayFloat, darkGrayFloat, 1);
    }

    public void offButtonShapeChange()
    {
        Shapes2D.Shape shape = GetComponent<Shapes2D.Shape>();
        float rightGrayFloat = rightGrayInt/256;
        shape.settings.fillColor = new Color(rightGrayFloat, rightGrayFloat, rightGrayFloat, 1);
    }
    

}
