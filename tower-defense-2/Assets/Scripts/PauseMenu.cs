using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	[SerializeField] private GameObject ui;
	[SerializeField] private string menuSceneName = "MainMenu";
	[SerializeField] private SceneFader sceneFader;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) Toggle();
	}

	public void Toggle()
	{
		ui.SetActive(!ui.activeSelf);
		Time.timeScale = ui.activeSelf ? 0f : 1f;
	}

	public void Retry()
	{
		Toggle();
		sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

	public void Menu()
	{
		Toggle();
		sceneFader.FadeTo(menuSceneName);
	}
}
