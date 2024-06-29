using UnityEngine;

public class BuildManager : MonoBehaviour {

	public static BuildManager instance;
	
	public GameObject buildEffect;
	public GameObject sellEffect;

	private TurretBlueprint turretToBuild;
	private Node selectedNode;

	public NodeUI nodeUI;

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
	
	public void SelectNode(Node node)
	{
		if (selectedNode == node)
		{
			DeselectNode();
			return;
		}

		selectedNode = node;
		turretToBuild = null;

		nodeUI.SetTarget(node);
	}

	public void DeselectNode()
	{
		selectedNode = null;
		nodeUI.Hide();
	}

	public void SelectTurretToBuild (TurretBlueprint turret)
	{
		turretToBuild = turret;
		DeselectNode();
	}

	public TurretBlueprint GetTurretToBuild()
		=> turretToBuild;
}
