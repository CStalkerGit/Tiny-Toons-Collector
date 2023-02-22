using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignRect
{
    public float top, bottom, right, left;
    public float rightSlope;
    public float leftSlope;

    public AlignRect()
    {
        rightSlope = 0;
        leftSlope = 0;
    }

    public void Reset(Entity entity, Vector3 newPosition)
    {
        top = newPosition.y - entity.rh;
        bottom = newPosition.y + entity.rh;
        right = newPosition.x - entity.rw;
        left = newPosition.x + entity.rw;
    }
    public void Inflate(float tileX, float tileY)
    {
        if (top < tileY + 1) top = tileY + 1;
        if (bottom > tileY) bottom = tileY;
        if (right < tileX + 1f) right = tileX + 1f;
        if (left > tileX) left = tileX;
    }
    // увеличиваем ограничивающий прямоугольник, при этом не превышая максимальных границ тайла
    public void Inflate(float tileX, float tileY, float _top, float _bottom, float _right, float _left)
    {
        if (top < _top) top = _top < tileY + 1 ? _top : tileY + 1;
        if (bottom > _bottom) bottom = _bottom > tileY ? _bottom : tileY;
        if (right < _right) right = _right < tileX + 1f ? _right : tileX + 1f;
        if (left > _left) left = _left > tileX ? _left : tileX;
    }

    public void InflateLocal(float tileX, float tileY, float localTop, float localBottom, float localRight, float localLeft)
    {
        localTop += tileY;
        localBottom += tileY;
        localRight += tileX;
        localLeft += tileX;
        if (top < localTop) top = Mathf.Min(localTop, tileY + 1); // newTop < tileY + 1 ? newTop : tileY + 1;
        if (bottom > localBottom) bottom = Mathf.Max(localBottom, tileY);
        if (right < localRight) right = Mathf.Min(localRight, tileX + 1f);
        if (left > localLeft) left = Mathf.Max(localLeft, tileX);
    }

    public void UpdateSlopes(bool toRight, float angle) // 1 = 45 degrees, -0.5 = -25.5 degrees etc
    {
        float angle2 = -angle;
        if (!toRight)
        {
            angle2 = angle;
            angle = -angle;
        }
        if (rightSlope < angle) rightSlope = angle;
        if (leftSlope < angle2) leftSlope = angle2;
    }
}