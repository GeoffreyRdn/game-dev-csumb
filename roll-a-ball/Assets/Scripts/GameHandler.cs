using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameHandler : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip pieceSFX;
    [SerializeField] AudioClip winSFX;
    
    [SerializeField] GameObject allCoins;
    
    [SerializeField] private GameObject scoreGO;
    [SerializeField] private GameObject winGO;
    [SerializeField] private TextMeshProUGUI scoreTMP;
    
    private int score = 0;
    private int nbCoins;
    
    private void Awake()
    {
        nbCoins = allCoins.transform.childCount;
        scoreTMP.text = score + " / " + nbCoins;
    }

    private void OnWin(GameObject player)
    {
        audioSource.PlayOneShot(winSFX);
        player.GetComponent<PlayerInput>().DeactivateInput();
        Rigidbody body = player.GetComponent<PlayerBehavior>().body;
        
        winGO.SetActive(true);
        scoreGO.SetActive(false);
        
        // stop the player
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;

        TimerHandler.run = false;
    }
    
    public void OnCoinCollected(GameObject player)
    {
        audioSource.PlayOneShot(pieceSFX);
        scoreTMP.text = ++score + " / " + nbCoins;
        if (score == nbCoins)
            OnWin(player);
    }
}
