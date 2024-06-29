using UnityEngine;

public class BuildManager : MonoBehaviour {

	public static BuildManager instance;

	[SerializeField] private GameObject buildEffect;
	private TurretBlueprint turretToBuild;
	
	public bool CanBuild => turretToBuild != null;
	public bool HasMoney => PlayerStats.money >= turretToBuild.cost;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("ERROR : More than one BuildManager in scene!");
			return;
		}
		
		instance = this;
	}
	
	public void BuildTurretOn(Node node)
	{
		if (PlayerStats.money < turretToBuild.cost)
		{
			Debug.Log("Not enough money to build this defense!");
			return;
		}
		
		PlayerStats.money -= turretToBuild.cost;

		GameObject turret = Instantiate(turretToBuild.prefab, node.GetBuildPosition(), Quaternion.identity);
		node.turret = turret;

		GameObject effect = Instantiate(buildEffect, node.GetBuildPosition(), Quaternion.identity);
		Destroy(effect, 5f);

		Debug.Log("Turret build! Money left: " + PlayerStats.money);
	}

	public void SelectTurretToBuild (TurretBlueprint turret)
		=> turretToBuild = turret;
}
