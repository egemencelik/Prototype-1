using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakenTile : MyTile
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();

        if (player)
        {
            player.SetCurrentTile(this);
            player.FreeMovement = true;
            
            if (player.CurrentPath.Count > 0)
                player.StartPaint(false);
        }
    }
}