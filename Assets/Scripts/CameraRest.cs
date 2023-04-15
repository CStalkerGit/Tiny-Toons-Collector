using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRest : MonoBehaviour
{
    public Color cameraBG; //AAE7FF

    public int X => Mathf.FloorToInt(transform.position.x);
    public int Y => Mathf.FloorToInt(transform.position.y);
}
