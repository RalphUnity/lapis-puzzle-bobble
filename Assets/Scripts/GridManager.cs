using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, length;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private new Camera camera;
    [SerializeField] private Transform parent;

    public int Width { get { return width; } }
    public int Length { get { return length; } }

    private Dictionary<Vector2, Tile> _tiles = new Dictionary<Vector2, Tile>();
    public Dictionary<Vector2, Tile> Tiles { get { return _tiles; } }

    private void Start() =>  GenerateGrid();

    private void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector2(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.transform.SetParent(parent);

                // Generate random balls in board
                if((x >= 1 && x <= 5) && (y >= 4 || y == 8))
                {
                    var prefabs = GameManager.Instance.ObjectPool.BallPrefabs;
                    int rand = Random.Range(0, prefabs.Length);
                    spawnedTile.ball = Instantiate(prefabs[rand], spawnedTile.transform.position
                        , spawnedTile.transform.rotation).GetComponent<Ball>();
                    spawnedTile.ball.GetComponent<Collider2D>().enabled = true;
                    spawnedTile.ball.transform.SetParent(parent);
                }

                _tiles[new Vector2(x,y)] = spawnedTile;
            }
        }

        camera.transform.position = new Vector3(width / 1.5f, length / 1.75f - 0.5f, -10);
    }

    public Tile GetTileAtPosition(Vector2 position)
    {
        if (_tiles.TryGetValue(position, out var tile))
        {
            return tile;
        }

        return null;
    }

}
