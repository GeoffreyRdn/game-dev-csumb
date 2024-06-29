using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool gameEnded = false;
	
    private void Update() 
    {
        if (gameEnded) return;
        if (PlayerStats.lives <= 0) EndGame();
    }

    private void EndGame()
    {
        gameEnded = true;
        Debug.Log("Game Over!");
    }
}
