using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BallData", menuName = "BallInfo/BallData")]
public class BallInfo : ScriptableObject
{
    public enum BallColor
    {
        BLUE, BROWN, GREEN, RED, WHITE, YELLOW,
    };

    [SerializeField] private BallColor ballColor;
    [SerializeField] private int score;

    public BallColor Color { get { return ballColor; } }
    public int Score { get { return score; } }
}
