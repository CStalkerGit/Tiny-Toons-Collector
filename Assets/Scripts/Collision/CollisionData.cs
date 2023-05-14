using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionData
{
    public float top, bottom, right, left;
    public bool isSlope = false;
    public bool isCollision = false;

    public CollisionData(Entity entity)
    {
        top = entity.pos.y - entity.rh;
        bottom = entity.pos.y + entity.rh;
        right = entity.pos.x - entity.rw;
        left = entity.pos.x + entity.rw;
        isSlope = false;
    }

    public void IntersectTile(float tileX, float tileY)
    {
        Intersect(tileY + 1, tileY, tileX + 1, tileX);
    }

    public void IntersectTile(float tileX, float tileY, float localTop, float localBottom, float localRight, float localLeft)
    {
        localTop = Mathf.Clamp(localTop, 0, 1);
        localBottom = Mathf.Clamp(localBottom, 0, 1);
        localRight = Mathf.Clamp(localRight, 0, 1);
        localLeft = Mathf.Clamp(localLeft, 0, 1);
        Intersect(tileY + localTop, tileY + localBottom, tileX + localRight, tileX + localLeft);
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