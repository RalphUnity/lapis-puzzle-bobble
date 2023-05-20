using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private TextMeshProUGUI highestScore;

    private void Awake() => LoadScore();

    public void StartGame() => SceneManager.LoadScene(1);

    public void ExitGame() => Application.Quit();

    private void LoadScore() => highestScore.text = "Highest Score: " + PlayerPrefs.GetInt(GameManager.SCORE_PREFS).ToString();
}
