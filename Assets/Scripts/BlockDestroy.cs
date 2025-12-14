using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SimpleTileBreaker_LeftRight : MonoBehaviour
{
    public Tilemap targetTilemap;             // Your Tilemap
    public TilemapCollider2D tilemapCollider; // The TilemapCollider2D on the same object
    public float breakDistance = 1.0f;        // How far to check
    public KeyCode breakKey = KeyCode.E;

    void Update()
    {
        if (Input.GetKeyDown(breakKey))
            BreakTileOnEitherSide();
    }

    void BreakTileOnEitherSide()
    {
        if (!targetTilemap || !tilemapCollider) return;

        // World positions to check left & right of player
        Vector3 rightCheck = transform.position + Vector3.right * breakDistance;
        Vector3 leftCheck  = transform.position + Vector3.left  * breakDistance;

        Vector3Int rightCell = targetTilemap.WorldToCell(rightCheck);
        Vector3Int leftCell  = targetTilemap.WorldToCell(leftCheck);

        bool rightTile = targetTilemap.HasTile(rightCell);
        bool leftTile  = targetTilemap.HasTile(leftCell);

        // Prefer whichever side has a tile (if both, prioritize closer one)
        if (rightTile && !leftTile)
        {
            RemoveTile(rightCell, "right");
        }
        else if (leftTile && !rightTile)
        {
            RemoveTile(leftCell, "left");
        }
        else if (rightTile && leftTile)
        {
            // If both sides have tiles, pick the closer one
            float rightDist = Vector2.Distance(transform.position, targetTilemap.CellToWorld(rightCell));
            float leftDist  = Vector2.Distance(transform.position, targetTilemap.CellToWorld(leftCell));
            if (rightDist <= leftDist) RemoveTile(rightCell, "right");
            else RemoveTile(leftCell, "left");
        }
        else
        {
            Debug.Log("No tiles found on either side.");
        }
    }

    void RemoveTile(Vector3Int cell, string side)
    {
        targetTilemap.SetTile(cell, null);
        targetTilemap.RefreshTile(cell);
        StartCoroutine(RefreshColliderNextFrame());
        Debug.Log($"Destroyed tile on {side} side at {cell}");
    }

    IEnumerator RefreshColliderNextFrame()
    {
        yield return null; // wait a frame for the change
        tilemapCollider.enabled = false;
        yield return null;
        tilemapCollider.enabled = true;
    }
}
