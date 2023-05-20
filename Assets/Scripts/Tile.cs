using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private List<Tile> _connectedTiles;

    public Ball ball;

    /// <summary>
    /// Every ball when snapped, interacts with its eight neighbours, which are the cells that are horizontally,
    /// vertically, or diagonally adjacent.
    /// </summary>
    public List<Tile> GetConnectedTiles(Vector2 pos, List<Tile> excludeTiles = null)
    {
        _connectedTiles = new List<Tile> { this, };

        if (excludeTiles == null)
            excludeTiles = new List<Tile> { this, };
        else
            excludeTiles.Add(this);

        CheckNeighbors(pos, excludeTiles);
        
        return _connectedTiles;
    }

    private void CheckNeighbors(Vector2 pos, List<Tile> excludeTiles)
    {
        // North
        if (pos.y + 1 < GameManager.Instance.GridManager.Length)
        {
            Vector2 newPos = new Vector2(pos.x, pos.y + 1);
            ConnectTile(newPos, excludeTiles);
        }
        // East
        if (pos.x + 1 < GameManager.Instance.GridManager.Width)
        {
            Vector2 newPos = new Vector2(pos.x + 1, pos.y);
            ConnectTile(newPos, excludeTiles);
        }
        // South
        if (pos.y - 1 >= 0)
        {
            Vector2 newPos = new Vector2(pos.x, pos.y - 1);
            ConnectTile(newPos, excludeTiles);
        }
        // West
        if (pos.x - 1 >= 0)
        {
            Vector2 newPos = new Vector2(pos.x - 1, pos.y);
            ConnectTile(newPos, excludeTiles);
        }
        // NorthEast
        if (pos.x + 1 < GameManager.Instance.GridManager.Width && pos.y + 1 < GameManager.Instance.GridManager.Length)
        {
            Vector2 newPos = new Vector2(pos.x + 1, pos.y + 1);
            ConnectTile(newPos, excludeTiles);
        }
        // NorthWest
        if (pos.x - 1 >= 0 && pos.y + 1 < GameManager.Instance.GridManager.Length)
        {
            Vector2 newPos = new Vector2(pos.x - 1, pos.y + 1);
            ConnectTile(newPos, excludeTiles);
        }
        // SouthEast
        if (pos.x + 1 < GameManager.Instance.GridManager.Width && pos.y - 1 >= 0)
        {
            Vector2 newPos = new Vector2(pos.x + 1, pos.y - 1);
            ConnectTile(newPos, excludeTiles);
        }
        // SouthWest
        if (pos.x - 1 >= 0 && pos.y - 1 >= 0)
        {
            Vector2 newPos = new Vector2(pos.x - 1, pos.y - 1);
            ConnectTile(newPos, excludeTiles);
        }
    }

    private void ConnectTile(Vector2 pos, List<Tile> excludeTiles)
    {
        Tile tile = GameManager.Instance.GridManager.GetTileAtPosition(pos);

        if (tile == null || !excludeTiles.Contains(tile) && tile.ball != null)
        {
            if (tile.ball.BallInfo.Color == ball.BallInfo.Color)
                _connectedTiles.AddRange(tile.GetConnectedTiles(pos, excludeTiles));
        }
    }

    /// <summary>
    /// Any balls that are then not connected to the top row should fall due to no longer being supported
    /// </summary>
    /// <returns></returns>
    public List<Tile> NotConnectedTiles()
    {
        List<Tile> notConnectedTiles = new List<Tile>();
        List<Tile> tempTiles;

        foreach (var item in GameManager.Instance.GridManager.Tiles)
        {
            if (item.Value.ball == null)
                continue;

            tempTiles = new List<Tile>();
            Vector2 pos = item.Key;

            // North
            if (pos.y + 1 < GameManager.Instance.GridManager.Length)
            {
                Vector2 newPos = new Vector2(pos.x, pos.y + 1);
                Tile tile = GameManager.Instance.GridManager.GetTileAtPosition(newPos);
                if (tile.ball == null)
                {
                    tempTiles.Add(tile);
                }
            }
            // NorthEast
            if (pos.x + 1 < GameManager.Instance.GridManager.Width && pos.y + 1 < GameManager.Instance.GridManager.Length)
            {
                Vector2 newPos = new Vector2(pos.x + 1, pos.y + 1);
                Tile tile = GameManager.Instance.GridManager.GetTileAtPosition(newPos);
                if (tile.ball == null)
                {
                    tempTiles.Add(tile);
                }
            }
            // NorthWest
            if (pos.x - 1 >= 0 && pos.y + 1 < GameManager.Instance.GridManager.Length)
            {
                Vector2 newPos = new Vector2(pos.x - 1, pos.y + 1);
                Tile tile = GameManager.Instance.GridManager.GetTileAtPosition(newPos);
                if (tile.ball == null)
                {
                    tempTiles.Add(tile);
                }
            }

            if (tempTiles.Count == 3)
                notConnectedTiles.Add(item.Value);
        }

        return notConnectedTiles;
    }
}
