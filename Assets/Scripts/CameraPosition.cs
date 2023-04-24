using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BackgroundColor
{
    Black,
    Blue 
}

public class CameraPosition : MonoBehaviour
{
    public BackgroundColor backgroundColor; //AAE7FF

    public int X => Mathf.FloorToInt(transform.position.x);
    public int Y => Mathf.FloorToInt(transform.position.y);

    public Color GetBackgroundColor()
    {
        switch (backgroundColor)
        {
            case BackgroundColor.Black:
                return Color.black;
            case BackgroundColor.Blue:
                return new Color32(0xAA, 0xE7, 0xFF, 0xFF);
            default:
                return Color.gray;
        }
        //AAE7FF
    }
}
