using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
	private Transform target;
	private NavMeshAgent navMeshAgent;

	private void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		navMeshAgent.destination = new Vector3(71, navMeshAgent.transform.position.y, -70);
	}

	private void Update()
	{
		var distance = navMeshAgent.remainingDistance;
		if (navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && distance == 0)
			EndPath();
	}

	private void EndPath()
	{
		PlayerStats.lives--;
		WaveSpawner.enemiesAlive--;
		Destroy(gameObject);
	}
}
