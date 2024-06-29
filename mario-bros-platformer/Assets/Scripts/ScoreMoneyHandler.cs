using System;
using TMPro;
using UnityEngine;

public class ScoreMoneyHandler : MonoBehaviour
{
    public static ScoreMoneyHandler Instance;
    
    [SerializeField] private Camera camera;
    
    [SerializeField] private TextMeshProUGUI coins;
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;
    private int nbCoins = 0;
    private RaycastHit hit;


    private void Awake()
        => Instance = this;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay (Input.mousePosition);
            if (Physics.Raycast (ray, out hit, 100)) {
                var objectHit = hit.transform.gameObject;
                
                if (objectHit.CompareTag("Brick"))
                    OnBrickHitted(objectHit);
                
                else if (objectHit.CompareTag("Mystery"))
                    OnMysteryBlockHitted(objectHit);
            }
        }
    }

    private string ZeroCount()
    {
        string zero = "000000";
        int score2 = score;
        while (score2 >= 1)
        {
            score2 /= 10;
            zero = zero.Substring(1);
        }

        return zero;
    }
    
    public void OnBrickHitted(GameObject brick)
    {
        score += 100;
        scoreText.text = "MARIO\n" + ZeroCount() + score;
        Destroy(brick);
    }

    public void OnMysteryBlockHitted(GameObject mystery)
    {
        int currentCoins = nbCoins;
        nbCoins += mystery.GetComponent<MysteryBlock>().OnHit(mystery.transform.position);
        coins.text = "x" + (nbCoins < 10 ? "0" + nbCoins : nbCoins);

        if (currentCoins == nbCoins) return;
        
        score += 100;
        scoreText.text = "MARIO\n" + ZeroCount() + score;
    }
}
