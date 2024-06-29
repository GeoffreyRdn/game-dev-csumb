using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameOverGO;

    [Space(10)]
    [Range(0,350)]
    [SerializeField] private float timer;

    public static bool GameOver = false;
    public static Timer Instance;

    private void Awake()
        => Instance = this;

    void Update()
    {
        if (!GameOver)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                timerText.text = "TIME\n" + (int) timer;
            }
            
            else
            {
                GameOver = true;
                gameOverGO.SetActive(true);
            }
        }
    }

    public void OnWin()
    {
        GameOver = true;
        gameOverGO.SetActive(true);
    }
}
