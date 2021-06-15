using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Obstacle Tile")]
public class ObstacleCustomTile : CustomTile
{
    public MoveDirection Direction;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        TileType = TileType.Obstacle;
        base.GetTileData(position, tilemap, ref tileData);
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (Application.isPlaying)
        {
            var obstacle = go.GetComponent<ObstacleTile>();
            var tileManager = tilemap.GetComponent<Transform>().GetComponentInParent<TileManager>();
            
            if (obstacle != null)
            {
                tileManager.Obstacles.Add(obstacle);
                obstacle.TileManager = tileManager;
                obstacle.Direction = Direction;
            }
        }

        return base.StartUp(position, tilemap, go);
    }
}