using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BackgroundColor
{
    Black,
    Blue 
}

public class CameraArea : MonoBehaviour
{
    public BackgroundColor backgroundColor; //AAE7FF
    public CameraCoord topRightCorner;

    public int X1 => Mathf.FloorToInt(transform.position.x);
    public int Y1 => Mathf.FloorToInt(transform.position.y);
    public int X2 => topRightCorner ? topRightCorner.X : Mathf.FloorToInt(transform.position.x + 8);
    public int Y2 => topRightCorner ? topRightCorner.Y : Mathf.FloorToInt(transform.position.y + 6);

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
