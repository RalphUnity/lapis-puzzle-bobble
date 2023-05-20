using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D _rb2d;
    private Collider2D _collider2d;
    private Vector3 _lastVelocity;
    private GridManager _gridManager;

    [HideInInspector] public bool newBullet;

    [SerializeField] private BallInfo ballInfo;
    
    public BallInfo BallInfo { get { return ballInfo; } }

    // Start is called before the first frame update
    void Start()
    {
        _gridManager = GameManager.Instance.GridManager;
        _collider2d = GetComponent<Collider2D>();
        _rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() => _lastVelocity = _rb2d.velocity;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ceiling" || collision.gameObject.tag == "Ball")
        {
            _rb2d.isKinematic = true;
            GameManager.Instance.AudioManager.PlaySFX(GameManager.Instance.AudioManager.hitClip);
        }

        Movement(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!newBullet)
        {
            if (collision.gameObject.tag == "Lose")
            {
                // Player lose; restart or quit
                Debug.LogError("Player Lose");
                GameManager.Instance.Ceiling.offset = 0;
            }
        }
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    private void Movement(Collision2D collision)
    {
        // Stop ball movement and snap to grid tile based on ball position
        if (_rb2d.isKinematic)
        {
            _rb2d.velocity = Vector2.zero;
            _rb2d.SetRotation(0);

            Vector2 pos = new Vector2(Mathf.RoundToInt(transform.position.x)
                , Mathf.RoundToInt(transform.position.y + GameManager.Instance.Ceiling.offset));
            Tile tile = _gridManager.GetTileAtPosition(pos);
            _rb2d.position = tile.transform.position;
            tile.ball = this;
            GameManager.Instance.ObjectPool.PooledObjects.Add(tile.ball.gameObject);

           BobbleLogic(tile, pos);

            newBullet = false;
            GameManager.Instance.Player.isFiring = true;
        }
        else
        {
            // Bounce on wall
            float speed = _lastVelocity.magnitude;
            Vector3 direction = Vector3.Reflect(_lastVelocity.normalized, collision.contacts[0].normal);

            _rb2d.velocity = direction * Mathf.Max(speed, 0f);
        }
    }

    private void BobbleLogic(Tile tile, Vector2 pos)
    {
        List<Tile> connectedTiles = tile.GetConnectedTiles(pos);

        if (connectedTiles.Count >= 3)
        {
            GameManager.Instance.AudioManager.PlaySFX(GameManager.Instance.AudioManager.connectClip);

            foreach (var connectedTile in connectedTiles)
            {
                if (connectedTile.ball.BallInfo.Color == GameManager.Instance.firedColor)
                {
                    GameManager.Instance.ScoreManager.SetScore(connectedTile.ball.ballInfo.Score);
                    connectedTile.ball.gameObject.SetActive(false);
                    connectedTile.ball = null;
                }
            }
        }

        List<Tile> notConnectedTiles = tile.NotConnectedTiles();
        if (notConnectedTiles != null && notConnectedTiles.Count >= 1)
        {
            foreach (Tile notConnectedTile in notConnectedTiles)
            {
                notConnectedTile.ball._rb2d.isKinematic = false;
                notConnectedTile.ball._rb2d.gravityScale = 1;
                notConnectedTile.ball._collider2d.enabled = false;
                notConnectedTile.ball = null;
            }
        }

        GameManager.Instance.Win();
    }
}
