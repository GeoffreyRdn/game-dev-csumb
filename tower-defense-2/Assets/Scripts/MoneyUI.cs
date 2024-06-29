using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour {

	[SerializeField] private Text moneyText;
	
	private void Update()
		=> moneyText.text = "$" + PlayerStats.money;
}
