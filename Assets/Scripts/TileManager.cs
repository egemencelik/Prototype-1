using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    private Grid grid;

    [SerializeField]
    private Tilemap tilemap, obstacles;

    [SerializeField]
    private Tile TakenTile, PathTile;

    private int TotalNormalTileCount { get; set; }
    public List<Vector3Int> WallPositions { get; private set; }
    public List<Vector3Int> NormalTilePositions { get; private set; }
    public List<ObstacleTile> Obstacles { get; private set; }

    public float CompletionPercentage
    {
        get
        {
            if (TotalNormalTileCount == 0) return 0;
            return 100 - (NormalTilePositions.Count * 100 / TotalNormalTileCount);
        }
    }

    private static readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
    private static readonly WaitForSeconds reverseCD = new WaitForSeconds(.5f);
    private readonly List<Vector3Int> checkedTiles = new List<Vector3Int>();
    
    private bool canReverseObstacles = true;

    private void Awake()
    {
        grid = GetComponent<Grid>();
        WallPositions = new List<Vector3Int>();
        NormalTilePositions = new List<Vector3Int>();
        Obstacles = new List<ObstacleTile>();
        obstacles.GetComponent<TilemapRenderer>().sortingOrder = -1;
    }

    private void Start()
    {
        StartCoroutine(InitValues());
    }

    private IEnumerator InitValues()
    {
        yield return waitForEndOfFrame;
        TotalNormalTileCount = NormalTilePositions.Count;
    }
    
    private IEnumerator StartReverseCooldown()
    {
        yield return reverseCD;
        canReverseObstacles = true;
    }

    public Vector3 GetTileCenter(Vector3 pos)
    {
        var cellPos = grid.WorldToCell(pos);

        return tilemap.GetCellCenterWorld(cellPos);
    }

    private bool IsNextToWall(Vector3Int pos)
    {
        var neighbors = pos.GetFourDirections();
        return neighbors.Count(n => WallPositions.Contains(n)) > 0;
    }

    private bool ShouldRemainingTilesBePainted()
    {
        foreach (var p in NormalTilePositions)
        {
            if (IsNextToWall(p))
            {
                return false;
            }
        }

        return true;
    }

    public void SetTileTaken(Vector3 pos)
    {
        var cellPos = grid.WorldToCell(pos);
        StartCoroutine(SetTile(tilemap, cellPos, PathTile));
    }

    // Paints a single tile
    private static IEnumerator SetTile(Tilemap map, Vector3Int pos, TileBase tile)
    {
        yield return waitForEndOfFrame;
        map.SetTile(pos, tile);
    }

    // Paints the given points and removes them from normal tile list.
    private void PaintPoints(List<Vector3Int> points)
    {
        foreach (var pos in points)
        {
            StartCoroutine(SetTile(tilemap, pos, TakenTile));
            NormalTilePositions.Remove(pos);
        }
    }

    public void StartPaint(List<PathTile> path)
    {
        checkedTiles.Clear();

        if (path.Count < 1)
        {
            return;
        }

        // Get appropriate tile to search the neighbors
        var searchOrigin = GetTileWithMoreThanOneAvailableNeighbor(path);

        // Get smallest area to paint
        var option = new AreaFillOption(searchOrigin);
        SearchAreas(ref option);
        var area = option.GetSmallestArea();

        // Paint
        PaintPoints(path.GetPositions());
        PaintPoints(area);

        // If remaining tiles are surrounded by taken tiles, paint them
        if (ShouldRemainingTilesBePainted())
        {
            PaintPoints(NormalTilePositions.ToList());
        }
    }


    private Vector3Int GetTileWithMoreThanOneAvailableNeighbor(List<PathTile> path)
    {
        foreach (var p in path)
        {
            // If has more than 1 neighbor that is a normal tile, return it
            if (HasNeighbors(p.Position)) return p.Position;
        }

        return path[0].Position;
    }

    // Checks neighbors in 4 directions
    private bool HasNeighbors(Vector3Int pos)
    {
        var positions = new List<Vector3Int> {pos.Up(), pos.Right(), pos.Down(), pos.Left()};
        var availableNeighbors = positions.Count(position => tilemap.GetTile<CustomTile>(position).TileType == TileType.Normal);
        return availableNeighbors > 1;
    }

    // Searchs possible areas to paint in 4 directions
    private void SearchAreas(ref AreaFillOption option)
    {
        SearchForPossibleTiles(option.UpOrigin, ref option.Up);
        SearchForPossibleTiles(option.RightOrigin, ref option.Right);
        SearchForPossibleTiles(option.DownOrigin, ref option.Down);
        SearchForPossibleTiles(option.LeftOrigin, ref option.Left);
    }


    // Recursive function to search normal tiles
    private void SearchForPossibleTiles(Vector3Int pos, ref List<Vector3Int> possibleTiles)
    {
        if (WallPositions.Contains(pos)) return;
        if (checkedTiles.Contains(pos)) return;

        checkedTiles.Add(pos);

        var tile = tilemap.GetTile<CustomTile>(pos);
        if (tile.TileType != TileType.Normal) return;

        possibleTiles.Add(pos);

        SearchForPossibleTiles(pos.Right(), ref possibleTiles);
        SearchForPossibleTiles(pos.Left(), ref possibleTiles);
        SearchForPossibleTiles(pos.Up(), ref possibleTiles);
        SearchForPossibleTiles(pos.Down(), ref possibleTiles);
    }

    public void ReverseAllObstacles()
    {
        if (!canReverseObstacles) return;
        canReverseObstacles = false;
        
        foreach (var obstacleTile in Obstacles)
        {
            obstacleTile.ReverseDirection();
        }

        StartCoroutine(StartReverseCooldown());
    }
    
}