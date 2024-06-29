using UnityEngine;

public class Bullet : MonoBehaviour {
	
	[SerializeField] private float speed = 70f;
	[SerializeField] private int damage = 50;
	[SerializeField] private float explosionRadius = 0f;
	[SerializeField] private GameObject impactEffect;
	
	private Transform target;
	
	public void Seek(Transform target)
		=> this.target = target;
	
	private void Update() 
	{
		if (target == null)
		{
			Destroy(gameObject);
			return;
		}
		
		Vector3 dir = target.position - transform.position;
		var distanceThisFrame = speed * Time.deltaTime;
		
		if (dir.magnitude <= distanceThisFrame)
		{
			HitTarget();
			return;
		}
		
		transform.Translate(dir.normalized * distanceThisFrame, Space.World);
		transform.LookAt(target);
	}

	private void HitTarget()
	{
		GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
		Destroy(effectIns, 5f);
		
		if (explosionRadius > 0f) Explode();
		else Damage(target);

		Destroy(gameObject);
	}

	private void Explode()
	{
		var colliders = Physics.OverlapSphere(transform.position, explosionRadius);

		foreach (Collider col in colliders)
		{
			if (col.CompareTag($"Enemy")) Damage(col.transform);
		}
	}

	private void Damage (Transform enemy)
	{
		Enemy e = enemy.GetComponent<Enemy>();
		if (e != null) e.TakeDamage(damage);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}
}
