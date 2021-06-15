using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Basic Tile")]
public class CustomTile : Tile
{
    public TileType TileType;

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (Application.isPlaying)
        {
            switch (TileType)
            {
                case TileType.Wall:
                    tilemap.GetComponent<Transform>().GetComponentInParent<TileManager>().WallPositions.Add(position);
                    break;
                case TileType.Normal:
                    tilemap.GetComponent<Transform>().GetComponentInParent<TileManager>().NormalTilePositions.Add(position);
                    break;
            }

            go.GetComponent<MyTile>().Position = position;
        }

        return base.StartUp(position, tilemap, go);
    }
}