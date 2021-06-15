using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTile : MyTile
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();

        if (player)
        {
            player.StartPaint(true);
        }
    }
}