using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 direction = Vector3.zero;

    private readonly WaitForSeconds inputCooldown = new WaitForSeconds(.1f);
    private bool isInputInCooldown;
    private MyTile currentTile, previousTile;

    [SerializeField]
    private TileManager tileManager;

    [SerializeField]
    private GameManager gameManager;

    public List<PathTile> CurrentPath { get; private set; }
    public bool FreeMovement { get; set; }

    private Vector3 currentTilePosition => currentTile == null ? Vector3.zero : currentTile.transform.position;

    private Vector3 previousTilePosition = new Vector3();

    void Start()
    {
        CurrentPath = new List<PathTile>();
        rb = GetComponent<Rigidbody>();
        var pos = tileManager.GetTileCenter(transform.position);
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }

    void Update()
    {
        InputChecks();
    }

    private void InputChecks()
    {
        if (isInputInCooldown) return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (direction == Vector3.down && !FreeMovement) return;
            direction = Vector3.up;
            StartCoroutine(StartInputCooldown());
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (direction == Vector3.right && !FreeMovement) return;
            direction = Vector3.left;
            StartCoroutine(StartInputCooldown());
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (direction == Vector3.up && !FreeMovement) return;
            direction = Vector3.down;
            StartCoroutine(StartInputCooldown());
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (direction == Vector3.left && !FreeMovement) return;
            direction = Vector3.right;
            StartCoroutine(StartInputCooldown());
        }
    }

    private IEnumerator StartInputCooldown()
    {
        isInputInCooldown = true;
        yield return inputCooldown;
        isInputInCooldown = false;
    }

    private void FixedUpdate()
    {
        if (currentTile == null)
        {
            return;
        }

        if (direction == Vector3.up || direction == Vector3.down)
        {
            if (Math.Abs(transform.position.x - currentTilePosition.x) > .1f) return;
        }

        if (direction == Vector3.left || direction == Vector3.right)
        {
            if (Math.Abs(transform.position.y - currentTilePosition.y) > .1f) return;
        }

        rb.velocity = 300 * Time.deltaTime * direction;
    }

    public void StartPaint(bool stopMovement)
    {
        tileManager.StartPaint(CurrentPath);
        CurrentPath.Clear();
        gameManager.UpdateSlider();

        if (stopMovement)
        {
            Stop(false);
        }
    }

    public void GameOver()
    {
        Stop(true);
    }

    private void Stop(bool returnToPreviousTile)
    {
        direction = Vector3.zero;
        rb.velocity = Vector3.zero;
        transform.position = returnToPreviousTile
            ? new Vector3(previousTilePosition.x, previousTilePosition.y, transform.position.z)
            : new Vector3(currentTilePosition.x, currentTilePosition.y, transform.position.z);
    }

    public void SetTileTaken(Vector3 pos)
    {
        tileManager.SetTileTaken(pos);
    }

    public void SetCurrentTile(MyTile tile)
    {
        if (currentTile != null)
        {
            previousTilePosition = currentTile.transform.position;
        }

        currentTile = tile;
    }
}