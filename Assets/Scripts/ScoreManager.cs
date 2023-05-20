using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;

    private void Awake()
    {
        LoadScore();
    }

    public void SetScore(int score)
    {
        this.score += score;
        SaveScore();
        scoreText.text = "Score: " + this.score.ToString();
    }

    public void SaveScore() => PlayerPrefs.SetInt(GameManager.SCORE_PREFS, score);

    private void LoadScore() => scoreText.text = "Score: " + PlayerPrefs.GetInt(GameManager.SCORE_PREFS).ToString();
}
