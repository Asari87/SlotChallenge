using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Utilities 
{
    public static LineRenderer CreateLineRenderer(string objectName, List<Vector3> points)
    {
        GameObject renderer = new GameObject(objectName);
        LineRenderer line = renderer.AddComponent<LineRenderer>();
        line.SetPositions(points.ToArray());

        return line;

    }

    public static LineRenderer CreateLineRenderer(string objectName, Vector3 start, Vector3 end, Material material, int width)
    {
        GameObject renderer = new GameObject(objectName);
        LineRenderer line = renderer.AddComponent<LineRenderer>();
        line.SetPosition(0,start);
        line.SetPosition(1,end);
        line.material = material; 
        line.startWidth = width;
        line.endWidth = width;
        return line;

    }

    public static GameObject CreateUILine(Vector3 start, Vector3 end, Color color, float width) 
    {
        GameObject line = new GameObject();
        Image img = line.AddComponent<Image>();
        img.color = color;
        img.rectTransform.anchoredPosition = start;
        Vector3 scale = end - start;
        scale.y = width;
        img.rectTransform.localScale = scale;

        return line;
    }
}
