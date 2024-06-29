using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    #region Variables
    
    [SerializeField] private GameObject gameFinish;
    [SerializeField] private TextMeshProUGUI scoreText;
 
    [SerializeField] private int scoreP1 = 0;
    [SerializeField] private int scoreP2 = 0;
    [SerializeField] private int pointsToWin = 11;
    
    [SerializeField] private AudioClip winSFX;
    [SerializeField] private AudioClip scoreSFX;
    
    [SerializeField] private HighScore highScore;
    [SerializeField] private TextMeshProUGUI textHighScore;
    
    private AudioSource audioSource;
    private PaddleModifier paddleModifier;

    #endregion

    #region Init

    private void Awake()
    {
        textHighScore.text = highScore.text;
        paddleModifier = GetComponent<PaddleModifier>();
        audioSource = GetComponent<AudioSource>();
    }

    #endregion
    
    #region Methods
    private void OnWin(Player winner)
    {
        int diff = Math.Abs(scoreP1 - scoreP2);
        if (diff > highScore.diff)
        {
            highScore.diff = diff;
            highScore.highScore = scoreP2 + " - " + scoreP1;
            highScore.text = "High score: " + highScore.highScore;
        }
        
        audioSource.clip = winSFX;
        audioSource.Play();
        
        gameFinish.GetComponentInChildren<TextMeshProUGUI>().text = 
            $"Game Over,\n {(winner == Player.Player2 ? "Left" : "Right")} Paddle Wins";
        
        gameFinish.SetActive(true);
    }

    IEnumerator TextEffect()
    {
        scoreText.fontStyle = FontStyles.Bold;
        scoreText.color = Color.red;
        bool decreasing = false;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                scoreText.fontSize = decreasing ? scoreText.fontSize - 1 : scoreText.fontSize + 1;
                yield return new WaitForSeconds(0.1f);
            }

            decreasing = true;
        }
        
        scoreText.fontStyle = FontStyles.Normal;
        scoreText.color = Color.white;  
    }
    
    public void UpdateScore()
    {
        (scoreP1, scoreP2) = (0,0);
        scoreText.text = scoreP2 + " - " + scoreP1;
        paddleModifier.ChangePaddleSize(scoreP1, scoreP2);
    }

    public void UpdateScore(Player player)
    {
        if (player == Player.Player1)
        {
            scoreText.text = ++scoreP2 + " - " + scoreP1;
            if (scoreP2 == pointsToWin)
            {
                BallBehavior.Play = false;
                OnWin(Player.Player2);
            }
            else
            {
                audioSource.clip = scoreSFX;
                audioSource.Play();
            }
        }
        else
        {
            scoreText.text = scoreP2 + " - " + ++scoreP1;
            if (scoreP1 == pointsToWin)
            {
                BallBehavior.Play = false;
                OnWin(Player.Player1);
            }
            else
            {
                audioSource.clip = scoreSFX;
                audioSource.Play();
            }
        }
        
        paddleModifier.ChangePaddleSize(scoreP1, scoreP2);
        StartCoroutine(TextEffect());
    }
    #endregion
}
