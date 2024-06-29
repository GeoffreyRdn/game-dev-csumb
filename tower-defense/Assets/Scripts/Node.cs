using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {

	[SerializeField] private Color hoverColor;
	[SerializeField] private Color notEnoughMoneyColor;
	[SerializeField] private Vector3 positionOffset;

	[Header("Optional")]
	public GameObject turret;

	private Renderer rend;
	private Color startColor;

	private BuildManager buildManager;

	private void Start()
	{
		rend = GetComponent<Renderer>();
		startColor = rend.material.color;

		buildManager = BuildManager.instance;
    }

	public Vector3 GetBuildPosition()
		=> transform.position + positionOffset;

	private void OnMouseDown ()
	{
		if (EventSystem.current.IsPointerOverGameObject() || !buildManager.CanBuild) return;

		if (turret != null)
		{
			Debug.Log("Impossible to build there!");
			return;
		}

		buildManager.BuildTurretOn(this);
	}

	private void OnMouseEnter()
	{
		if (EventSystem.current.IsPointerOverGameObject() || !buildManager.CanBuild) return;
		rend.material.color = buildManager.HasMoney ? hoverColor : notEnoughMoneyColor;
	}

	void OnMouseExit ()
		=> rend.material.color = startColor;
}
