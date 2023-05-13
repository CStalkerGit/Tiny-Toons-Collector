using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionData
{
    public float top, bottom, right, left;
    public bool isSlope = false;
    public bool isCollision = false;

    public CollisionData()
    {

    }

    public void Reset(Entity entity, Vector3 newPosition)
    {
        top = newPosition.y - entity.rh;
        bottom = newPosition.y + entity.rh;
        right = newPosition.x - entity.rw;
        left = newPosition.x + entity.rw;
        isSlope = false;
    }
    public void UnionTile(float tileX, float tileY)
    {
        if (top < tileY + 1) top = tileY + 1;
        if (bottom > tileY) bottom = tileY;
        if (right < tileX + 1f) right = tileX + 1f;
        if (left > tileX) left = tileX;
    }

    // увеличиваем ограничивающий прямоугольник, при этом не превышая максимальных границ тайла
    public void UnionTile(float tileX, float tileY, float localTop, float localBottom, float localRight, float localLeft)
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

    public void Intersect(float _top, float _bottom, float _right, float _left)
    {
        top = Mathf.Max(top, _top);
        bottom = Mathf.Min(bottom, _bottom);
        right = Mathf.Max(right, _right);
        left = Mathf.Min(left, _left);
    }

    public void Intersect(Rect rect)
    {
        Intersect(rect.yMax, rect.yMin, rect.xMax, rect.xMin);
    }
}