using UnityEngine;

public class CompleteLevel : MonoBehaviour {

	[SerializeField] private string menuSceneName = "MainMenu";
	[SerializeField] private string nextLevel = "Level02";
	[SerializeField] private int levelToUnlock = 2;

	[SerializeField] private SceneFader sceneFader;

	public void Continue()
	{
		PlayerPrefs.SetInt("levelReached", levelToUnlock);
		sceneFader.FadeTo(nextLevel);
	}

	public void Menu()
		=> sceneFader.FadeTo(menuSceneName);
}
