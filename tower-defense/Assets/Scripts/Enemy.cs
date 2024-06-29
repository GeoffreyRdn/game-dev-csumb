using UnityEngine;

public class Enemy : MonoBehaviour {

	public float startSpeed = 10f;
	public float health = 100;
	public int worth = 50;
	
	[HideInInspector] public float speed;

	public GameObject deathEffect;

	private void Start()
		=> speed = startSpeed;

	public void TakeDamage(float amount)
	{
		health -= amount;
		if (health <= 0) Die();
	}

	public void Slow(float pct)
		=> speed = startSpeed * (1f - pct);

	private void Die()
	{
		PlayerStats.money += worth;
		GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(effect, 5f);
		Destroy(gameObject);
	}
}
