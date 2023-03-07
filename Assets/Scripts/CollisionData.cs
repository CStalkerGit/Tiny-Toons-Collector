using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionData
{
    public float top, bottom, right, left;
    public bool isSlope;

    public CollisionData()
    {
        isSlope = false;
    }

    public void Reset(Entity entity, Vector3 newPosition)
    {
        top = newPosition.y - entity.rh;
        bottom = newPosition.y + entity.rh;
        right = newPosition.x - entity.rw;
        left = newPosition.x + entity.rw;
        isSlope = false;
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

    //public void AddToTileSlope(float tileX, float tileY, Vector2 entityPoint, float maxTop, float maxBottom, float maxRight, float maxLeft)
    //{
    //    top = Mathf.Max(top, Mathf.Min(localTop, tileY + 1));

    //    if (top < localTop) top = Mathf.Min(localTop, tileY + 1); // newTop < tileY + 1 ? newTop : tileY + 1;
    //    if (bottom > localBottom) bottom = Mathf.Max(localBottom, tileY);
    //    if (right < localRight) right = Mathf.Min(localRight, tileX + 1f);
    //    if (left > localLeft) left = Mathf.Max(localLeft, tileX);
    //}
}