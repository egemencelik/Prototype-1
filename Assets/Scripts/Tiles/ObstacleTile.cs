using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleTile : MyTile
{
    public MoveDirection Direction;
    public TileManager TileManager;
    private Rigidbody rb;

    private readonly HashSet<Vector3Int> tilesVisited = new HashSet<Vector3Int>();
    private Vector3Int currentPosition;

    private Vector3 moveDirection
    {
        get
        {
            switch (Direction)
            {
                case MoveDirection.UP:
                    return Vector3.up;
                case MoveDirection.RIGHT:
                    return Vector3.right;
                case MoveDirection.DOWN:
                    return Vector3.down;
                case MoveDirection.LEFT:
                    return Vector3.left;
                default:
                    return Vector3.zero;
            }
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private bool ShouldReverseDirection()
    {
        if (tilesVisited.Count < 2) return false;
        var list = tilesVisited.ToList();
        return currentPosition == list[tilesVisited.Count - 1] || currentPosition == list[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Taken Tile":
                TileManager.Obstacles.Remove(this);
                Destroy(gameObject);
                return;
            case "Normal Tile":
                var tile = other.GetComponent<MyTile>();

                currentPosition = tile.Position;

                if (ShouldReverseDirection())
                {
                    StartCoroutine(ReverseWhenInPosition(tile.transform.position));
                }
                else
                {
                    tilesVisited.Add(tile.Position);
                }

                break;
            case "Wall":
                OnHitWall();
                break;
            case "Path Tile":
                StartCoroutine(GameManager.LoadIndexAfterSeconds(0, 1));
                return;
            case "Player":
                var player = other.GetComponent<Player>();
                player.GameOver();
                StartCoroutine(GameManager.LoadIndexAfterSeconds(0, 1));
                return;
        }
    }

    private IEnumerator ReverseWhenInPosition(Vector3 pos)
    {
        while (Vector3.Distance(transform.position, pos) > .1f)
        {
            yield return null;
        }

        TileManager.ReverseAllObstacles();

        yield return 0;
    }
    
    private void FixedUpdate()
    {
        rb.velocity = 200 * Time.deltaTime * moveDirection;
    }

    public void ReverseDirection()
    {
        Reverse();
    }

    private void Reverse()
    {
        switch (Direction)
        {
            case MoveDirection.UP:
                Direction = MoveDirection.DOWN;
                break;
            case MoveDirection.RIGHT:
                Direction = MoveDirection.LEFT;
                break;
            case MoveDirection.DOWN:
                Direction = MoveDirection.UP;
                break;
            case MoveDirection.LEFT:
                Direction = MoveDirection.RIGHT;
                break;
        }
    }

    public void OnHitWall()
    {
        TileManager.ReverseAllObstacles();
    }
}