using System.Collections.Generic;
using UnityEngine;

public struct AreaFillOption
{
    public List<Vector3Int> Up;
    public List<Vector3Int> Right;
    public List<Vector3Int> Down;
    public List<Vector3Int> Left;

    public Vector3Int UpOrigin;
    public Vector3Int RightOrigin;
    public Vector3Int DownOrigin;
    public Vector3Int LeftOrigin;

    public AreaFillOption(Vector3Int origin)
    {
        UpOrigin = origin.Up();
        RightOrigin = origin.Right();
        DownOrigin = origin.Down();
        LeftOrigin = origin.Left();
        Up = new List<Vector3Int>();
        Right = new List<Vector3Int>();
        Down = new List<Vector3Int>();
        Left = new List<Vector3Int>();
    }

    public List<Vector3Int> GetSmallestArea()
    {
        var min = 1000;
        var temp = new List<Vector3Int>();
        var list = new List<List<Vector3Int>> {Up, Left, Down, Right};
        var zeroCount = 0;

        foreach (var l in list)
        {
            if (l.Count == 0)
            {
                zeroCount++;
                continue;
            }
            if (l.Count < min)
            {
                min = l.Count;
                temp = l;
            }
        }
        
        return zeroCount == 3 ? new List<Vector3Int>() : temp;
    }
}