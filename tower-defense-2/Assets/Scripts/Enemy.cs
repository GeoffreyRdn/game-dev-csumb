using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

	public float startSpeed = 10f;
	public float startHealth = 100;
	
	private float health;
	public int worth = 50;
	
	[HideInInspector] public float speed;
	
	public GameObject deathEffect;

	[Header("Unity Stuff")]
	public Image healthBar;
	private bool isDead = false;

	private void Start()
	{
		speed = startSpeed;
		health = startHealth;
	}

	public void TakeDamage(float amount)
	{
		health -= amount;
		healthBar.fillAmount = health / startHealth;

		if (health <= 0 && !isDead) Die();
	}

	public void Slow(float pct)
		=> speed = startSpeed * (1f - pct);

	private void Die()
	{
		isDead = true;
		
		PlayerStats.money += worth;

		GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(effect, 5f);
		Destroy(gameObject);

		WaveSpawner.enemiesAlive--;
	}
}
