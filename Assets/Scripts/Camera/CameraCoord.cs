using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCoord : MonoBehaviour
{
    public int X => Mathf.FloorToInt(transform.position.x);
    public int Y => Mathf.FloorToInt(transform.position.y);
}
