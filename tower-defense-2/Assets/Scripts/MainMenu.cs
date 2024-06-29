using UnityEngine;

public class MainMenu : MonoBehaviour {

	[SerializeField] private string levelToLoad = "LevelSelect";
	[SerializeField] private SceneFader sceneFader;

	private void Play()
		=> sceneFader.FadeTo(levelToLoad);

	public void Quit()
	{
		if (Application.isEditor) UnityEditor.EditorApplication.isPlaying = false;
		else Application.Quit();
	}
}
