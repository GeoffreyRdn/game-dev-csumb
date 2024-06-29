using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private string mainScene;
    
    void Start()
    {
        StartCoroutine(GoBackToMainScene(5));
    }

    private IEnumerator GoBackToMainScene(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(BackgroundMusic.instance);
        SceneManager.LoadScene(mainScene);
    }
}
