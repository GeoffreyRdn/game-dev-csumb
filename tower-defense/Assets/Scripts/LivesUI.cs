using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour {

	[SerializeField] private Text livesText;
	
	void Update() 
		=> livesText.text = PlayerStats.lives + " LIVES";
}
