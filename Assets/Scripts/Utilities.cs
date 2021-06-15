using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utilities
{
    public static Vector3Int Left(this Vector3Int pos, int amount = 1)
    {
        return new Vector3Int(pos.x - amount, pos.y, pos.z);
    }

    public static Vector3Int Down(this Vector3Int pos, int amount = 1)
    {
        return new Vector3Int(pos.x, pos.y - amount, pos.z);
    }

    public static Vector3Int Right(this Vector3Int pos, int amount = 1)
    {
        return new Vector3Int(pos.x + amount, pos.y, pos.z);
    }

    public static Vector3Int Up(this Vector3Int pos, int amount = 1)
    {
        return new Vector3Int(pos.x, pos.y + amount, pos.z);
    }

    public static List<Vector3Int> GetFourDirections(this Vector3Int pos, int amount = 1)
    {
        return new List<Vector3Int> {pos.Up(amount), pos.Right(amount), pos.Down(amount), pos.Left(amount)};
    }

    public static List<Vector3Int> GetPositions(this List<PathTile> path)
    {
        return path.Select(p => p.Position).ToList();
    }
}