using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    [SerializeField] private string gameSceneName;

    public void LoadGameScene()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
