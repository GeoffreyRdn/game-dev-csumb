using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	private Transform target;
	private Enemy targetEnemy;

	[Header("General")]

	[SerializeField] private float range = 15f;

	[Header("Use Bullets (default)")]
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private float fireRate = 1f;
	private float fireCountdown = 0f;

	[Header("Use Laser")]
	[SerializeField] private bool useLaser = false;

	[SerializeField] private int damageOverTime = 30;
	[SerializeField] private float slowAmount = .5f;

	[SerializeField] private LineRenderer lineRenderer;
	[SerializeField] private ParticleSystem impactEffect;
	[SerializeField] private Light impactLight;

	[Header("Unity Setup Fields")]

	[SerializeField] private string enemyTag = "Enemy";

	[SerializeField] private Transform partToRotate;
	[SerializeField] private float turnSpeed = 10f;

	[SerializeField] private Transform firePoint;


	private void Start()
		=> InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
	
	private void Update() 
	{
		if (target == null)
		{
			if (useLaser && lineRenderer.enabled)
			{
				lineRenderer.enabled = false;
				impactEffect.Stop();
				impactLight.enabled = false;
			}
			
			return;
		}

		LockOnTarget();

		if (useLaser) Laser();
		
		else
		{
			if (fireCountdown <= 0f)
			{
				Shoot();
				fireCountdown = 1f / fireRate;
			}

			fireCountdown -= Time.deltaTime;
		}
	}

	private void UpdateTarget()
	{
		var enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		GameObject nearestEnemy = null;
		
		var shortestDistance = Mathf.Infinity;
		
		foreach (GameObject enemy in enemies)
		{
			var distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
			if (distanceToEnemy < shortestDistance)
			{
				shortestDistance = distanceToEnemy;
				nearestEnemy = enemy;
			}
		}

		if (nearestEnemy != null && shortestDistance <= range)
		{
			target = nearestEnemy.transform;
			targetEnemy = nearestEnemy.GetComponent<Enemy>();
		} 
		
		else
			target = null;
	}
	private void LockOnTarget()
	{
		Vector3 dir = target.position - transform.position;
		Quaternion lookRotation = Quaternion.LookRotation(dir);
		Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
		partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
	}

	private void Laser()
	{
		targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
		targetEnemy.Slow(slowAmount);

		if (!lineRenderer.enabled)
		{
			lineRenderer.enabled = true;
			impactEffect.Play();
			impactLight.enabled = true;
		}

		lineRenderer.SetPosition(0, firePoint.position);
		lineRenderer.SetPosition(1, target.position);

		Vector3 dir = firePoint.position - target.position;

		impactEffect.transform.position = target.position + dir.normalized;
		impactEffect.transform.rotation = Quaternion.LookRotation(dir);
	}

	private void Shoot()
	{
		GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		Bullet bullet = bulletGO.GetComponent<Bullet>();

		if (bullet != null) bullet.Seek(target);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}
