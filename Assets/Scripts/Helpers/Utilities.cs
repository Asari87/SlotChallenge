using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities 
{
    public static LineRenderer CreateLineRenderer(string objectName, List<Vector3> points)
    {
        GameObject renderer = new GameObject(objectName);
        LineRenderer line = renderer.AddComponent<LineRenderer>();
        line.SetPositions(points.ToArray());

        return line;

    }

    public static LineRenderer CreateLineRenderer(string objectName, Vector3 start, Vector3 end, Material material, int layer)
    {
        GameObject renderer = new GameObject(objectName);
        LineRenderer line = renderer.AddComponent<LineRenderer>();
        line.SetPosition(0,start);
        line.SetPosition(1,end);
        line.material = material;
        return line;

    }
}
