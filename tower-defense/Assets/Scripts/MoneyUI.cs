using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoneyUI : MonoBehaviour {

	[SerializeField] private Text moneyText;
	
	void Update()
		=> moneyText.text = "$" + PlayerStats.money;
}
