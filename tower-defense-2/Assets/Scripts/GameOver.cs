using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	[SerializeField] private string menuSceneName = "MainMenu";
	[SerializeField] private SceneFader sceneFader;

	public void Retry()
		=> sceneFader.FadeTo(SceneManager.GetActiveScene().name);

	public void Menu()
		=> sceneFader.FadeTo(menuSceneName);
}
