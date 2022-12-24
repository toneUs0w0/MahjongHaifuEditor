using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeController : MonoBehaviour
{
    private float darkGrayInt = 30;

    // off
    private float rightGrayInt = 100;
    private float rightGrayFrameInt = 200;

    // green
    private List<int> greenInt = new List<int>{129, 255, 0, 255};

    // black
    private int black = 0;


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
        float rightGrayFrameFloat = rightGrayFrameInt/256;
        
        shape.settings.fillColor = new Color(rightGrayFloat, rightGrayFloat, rightGrayFloat, 1);
        shape.settings.outlineColor = new Color(rightGrayFrameFloat, rightGrayFrameFloat, rightGrayFrameFloat, 1);
    }

    public void onButtonGreen()
    {
        Shapes2D.Shape shape = GetComponent<Shapes2D.Shape>();
        
        shape.settings.fillColor = new Color(black, black, black, black);
        shape.settings.outlineColor = new Color(greenInt[0]/255.0f, greenInt[1]/255.0f, greenInt[2]/255.0f, greenInt[3]/255.0f);

    }
    

}
