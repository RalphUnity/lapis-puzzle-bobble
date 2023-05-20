using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Ceiling ceiling;
    [SerializeField] private Player player;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameObject pauseObj;

    public ObjectPool ObjectPool { get { return objectPool; } }
    public GridManager GridManager { get { return gridManager; } }
    public Ceiling Ceiling { get { return ceiling; } }
    public Player Player { get { return player; } }
    public ScoreManager ScoreManager { get { return scoreManager; } }
    public AudioManager AudioManager { get { return audioManager; } }

    public const string SCORE_PREFS = "Score";

    public BallInfo.BallColor firedColor;

    private void Awake()
    {
        if(_instance == null)
            _instance = this;
    }

    private void OnApplicationQuit() => ScoreManager.SaveScore();

    public void Win()
    {
        bool hasWin = true;
        foreach (var tile in GridManager.Tiles)
        {
            if(tile.Value.ball != null)
            {
                hasWin = false;
                break;
            }
        }

        Player.isFiring = false;

        if (hasWin)
            StartCoroutine(LoadNextScene());
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Player.isFiring = false;
        AudioManager.StopBackgroundOST();
        pauseObj.SetActive(true);
    }

    public void Resume() 
    {
        Time.timeScale = 1;
        player.isFiring = true;
        AudioManager.PlayBackgroundOST();
        pauseObj.SetActive(false);
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2f);

        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(1);
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Player.isFiring = true;
        }
    }
}
