using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour {

	[SerializeField] private GameObject ui;

	[SerializeField] private Text upgradeCost;
	[SerializeField] private Text sellAmount;
	
	[SerializeField] private Button upgradeButton;

	private Node target;

	public void SetTarget(Node target)
	{
		this.target = target;
		transform.position = target.GetBuildPosition();

		upgradeCost.text = target.isUpgraded ? "DONE" : "$" + target.turretBlueprint.upgradeCost;
		upgradeButton.interactable = !target.isUpgraded;
		sellAmount.text = "$" + target.turretBlueprint.GetSellAmount();

		ui.SetActive(true);
	}

	public void Hide()
		=> ui.SetActive(false);

	public void Upgrade()
	{
		target.UpgradeTurret();
		BuildManager.instance.DeselectNode();
	}

	public void Sell()
	{
		target.SellTurret();
		BuildManager.instance.DeselectNode();
	}
}
