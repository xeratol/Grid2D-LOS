using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHelper : MonoBehaviour
{
    public static List<Vector2Int> GetAllPosBetween(Vector2Int from, Vector2Int to)
    {
        // http://playtechs.blogspot.com/2007/03/raytracing-on-grid.html

        var list = new List<Vector2Int>();

        var diff = to - from;
        var inc = new Vector2Int((diff.x > 0) ? 1 : -1, (diff.y > 0) ? 1 : -1);
        diff.x = Mathf.Abs(diff.x);
        diff.y = Mathf.Abs(diff.y);
        var point = from;
        var error = diff.x - diff.y;
        diff *= 2;

        while (point != to)
        {
            if (error > 0)
            {
                point.x += inc.x;
                error -= diff.y;
            }
            else if (error < 0)
            {
                point.y += inc.y;
                error += diff.x;
            }
            else
            {
                if (diff.x < diff.y)
                {
                    list.Add(new Vector2Int(point.x + inc.x, point.y));
                    list.Add(new Vector2Int(point.x, point.y + inc.y));
                }
                else
                {
                    list.Add(new Vector2Int(point.x, point.y + inc.y));
                    list.Add(new Vector2Int(point.x + inc.x, point.y));
                }
                point += inc;
                error += -diff.y + diff.x;
            }
            list.Add(point);
        }

        return list;
    }

    public static Vector2Int GetGridPosFromScreenPoint(Vector3 screenPoint)
    {
        var ray = Camera.main.ScreenPointToRay(screenPoint);
        var plane = new Plane(Vector3.up, 0);
        var t = 0.0f;

        plane.Raycast(ray, out t);
        Debug.Assert(!Mathf.Approximately(t, 0), "Ray is parallel to Plane");

        var pt = ray.GetPoint(t);
        var c = Mathf.RoundToInt(pt.x);
        var r = Mathf.RoundToInt(pt.z);

        return new Vector2Int(c, r);
    }

    public static Vector2Int GetGridPosFromWorldPoint(Vector3 worldPoint)
    {
        var ray = new Ray(worldPoint, Vector3.down);
        var plane = new Plane(Vector3.up, 0);
        var t = 0.0f;

        Debug.Assert(plane.Raycast(ray, out t) || !!Mathf.Approximately(t, 0), "Ray is parallel to Plane");

        var pt = ray.GetPoint(t);
        var c = Mathf.RoundToInt(pt.x);
        var r = Mathf.RoundToInt(pt.z);

        return new Vector2Int(c, r);
    }

    public static Vector3 GetWorldPosFromGridPos(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x, 0, gridPos.y);
    }

    public static float GetOctileDistance(Vector2Int from, Vector2Int to)
    {
        var dx = Mathf.Abs(from.x - to.x);
        var dy = Mathf.Abs(from.y - to.y);
        var min = Mathf.Min(dx, dy);
        var max = Mathf.Max(dx, dy);
        var diff = max - min;

        return min * 1.41421356237f + diff;
    }
}
