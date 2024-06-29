using System;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    #region Events
    
    private void OnEnable()
    {
        Enemy.OnEnemyDestroyed += UpdateScore;
        Player.OnPlayerLose += DisplayGameOver;
        Player.UpdateLife += UpdateLives;
        GameHandler.Restart += Restart;
    }
    
    private void OnDisable()
    {
        Enemy.OnEnemyDestroyed -= UpdateScore;
        Player.OnPlayerLose -= DisplayGameOver;
        Player.UpdateLife -= UpdateLives;
        GameHandler.Restart -= Restart;
    }

    #endregion
    
    #region Variables
    
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highscoreText;
    [SerializeField] private Text livesText;
    
    [SerializeField] private GameObject lifeHolder;
    [SerializeField] private GameObject life;
    //[SerializeField] private GameObject gameOverPrefab;
    //[SerializeField] private GameObject startPrefab;
    //[SerializeField] private GameObject enemiesPrefab;

    [SerializeField] private Highscore highscore;

    private int score;
    
    #endregion

    #region Init

    private void Start()
    {
        //GameHandler.hasLoose = true;
        //enemiesPrefab.SetActive(false);
        highscoreText.text = highscore.text;
    }

    /*private void Update()
    {
        //if (startPrefab.activeSelf && Input.GetKeyDown(KeyCode.R))
        if (Input.GetKeyDown(KeyCode.R))
        {
            //startPrefab.SetActive(false);
            enemiesPrefab.SetActive(true);
            GameHandler.hasLoose = false;
        }
    }*/

    #endregion
    
    
    
    #region Methods
    private string Zeros()
    {
        int nbZeros = 4;
        int scoreBackup = score;
        
        while (scoreBackup >= 1 && nbZeros > 0)
        {
            scoreBackup /= 10;
            nbZeros--;
        }
        
        return new String('0', nbZeros);
    }

    private void SaveHighscore()
    {
        if (score > highscore.score)
        {
            highscore.score = score;
            highscore.text = "HIGH" + scoreText.text;
        }
    }
    
    private void UpdateScore(int points)
    {
        score += points;
        scoreText.text = "SCORE : " + Zeros() + score;
        SaveHighscore();
    }
    
    private void UpdateLives(int lives)
    {
        if (lives < 0) return;

        livesText.text = lives.ToString();
        if (lifeHolder.transform.childCount > lives)
        {
            // remove a life from the ui
            Destroy(lifeHolder.transform.GetChild(0).gameObject);
        }

        else
        {
            // add a life to the ui
            int diff = Math.Abs(lives - lifeHolder.transform.childCount);
            for (int i = 0; i < diff; i++)
                Instantiate(life, lifeHolder.transform);
        }
    }


    private void DisplayGameOver()
    {
        scoreText.text = "";
        highscoreText.text = "";
        
        //gameOverPrefab.SetActive(true);
    }

    private void Restart()
    {
        score = 0;
        //gameOverPrefab.SetActive(false);
        scoreText.text = "SCORE : 0000";
        highscoreText.text = highscore.text;
    }
    
    #endregion
}
