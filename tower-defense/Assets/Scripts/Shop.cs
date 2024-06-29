using UnityEngine;

public class Shop : MonoBehaviour {

	[SerializeField] private TurretBlueprint standardTurret;
	[SerializeField] private TurretBlueprint missileLauncher;
	[SerializeField] private TurretBlueprint laserBeamer;

	private BuildManager buildManager;

	void Start()
		=> buildManager = BuildManager.instance;

	public void SelectStandardTurret()
	{
		Debug.Log("Standard Turret Selected");
		buildManager.SelectTurretToBuild(standardTurret);
	}

	public void SelectMissileLauncher()
	{
		Debug.Log("Missile Launcher Selected");
		buildManager.SelectTurretToBuild(missileLauncher);
	}

	public void SelectLaserBeamer()
	{
		Debug.Log("Laser Beamer Selected");
		buildManager.SelectTurretToBuild(laserBeamer);
	}
}
