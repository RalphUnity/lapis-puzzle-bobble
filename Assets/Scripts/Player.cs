using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform ballSpawnPoint;
    [SerializeField] private Transform parent;

    [HideInInspector] public bool isFiring = true;

    private bool isPausing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFiring)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Fire();
                GameManager.Instance.AudioManager.PlaySFX(GameManager.Instance.AudioManager.fireClip);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPausing = !isPausing;

            if (isPausing)
                GameManager.Instance.Pause();
            else
                GameManager.Instance.Resume();
        }

        if(!isPausing)
            RotateToMousePosition();
    }

    private void RotateToMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);

        mousePos.x -= objectPos.x;
        mousePos.y -= objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 270));
    }

    private void Fire()
    {
        isFiring = false;
        Ball ball = GameManager.Instance.ObjectPool.GetPooledObject().GetComponent<Ball>();

        if(ball != null )
        {
            ball.transform.position = ballSpawnPoint.transform.position;
            ball.transform.rotation = ballSpawnPoint.transform.rotation;
            ball.gameObject.SetActive(true);
            ball.transform.SetParent(parent);
            ball.newBullet = true;
            Rigidbody2D rb2d = ball.GetComponent<Rigidbody2D>();
            ball.GetComponent<Collider2D>().enabled = true;
            rb2d.isKinematic = false;
            rb2d.AddRelativeForce(Vector2.right * 15f, ForceMode2D.Impulse);
            GameManager.Instance.firedColor = ball.BallInfo.Color;
        }
    }
}
