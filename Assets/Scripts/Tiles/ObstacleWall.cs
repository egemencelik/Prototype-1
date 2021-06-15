using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleWall : MyTile
{
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Taken Tile":
                break;
            case "Path Tile":
                break;
            case "Obstacle Wall":
                break;
            case "Obstacle":
                var obstacle = other.GetComponent<ObstacleTile>();
                obstacle.OnHitWall();
                break;
            case "Wall":
                break;
            case "Player":
                return;
        }
    }
}