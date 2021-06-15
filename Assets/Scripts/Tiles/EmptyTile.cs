using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTile : MyTile
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();

        if (player)
        {
            player.SetCurrentTile(this);
            player.FreeMovement = false;
            player.SetTileTaken(transform.position);
        }
    }
}