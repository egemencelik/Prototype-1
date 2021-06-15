using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTile : MyTile
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();

        if (player)
        {
            player.SetCurrentTile(this);

            if (player.CurrentPath.Contains(this))
            {
                player.GameOver();
                StartCoroutine(GameManager.LoadIndexAfterSeconds(0, 1));
                return;
            }

            player.CurrentPath.Add(this);
        }
    }
}