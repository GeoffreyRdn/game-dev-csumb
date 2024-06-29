using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {

	public static int enemiesAlive = 0;

	[SerializeField] private Wave[] waves;
	[SerializeField] private Transform spawnPoint;
	[SerializeField] private float timeBetweenWaves = 5f;
	[SerializeField] private Text waveCountdownText;
	
	[SerializeField] private GameManager gameManager;

	private float countdown = 2f;
	private int currentWave = 0;

	private void Update()
	{
		if (enemiesAlive > 0) return;

		if (currentWave == waves.Length)
		{
			gameManager.WinLevel();
			enabled = false;
		}

		if (countdown <= 0f)
		{
			StartCoroutine(SpawnWave());
			countdown = timeBetweenWaves;
			return;
		}

		countdown -= Time.deltaTime;
		countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

		waveCountdownText.text = $"{countdown:00.00}";
	}

	private IEnumerator SpawnWave()
	{
		PlayerStats.rounds++;
		Wave wave = waves[currentWave];

		enemiesAlive = wave.count;

		for (int i = 0; i < wave.count; i++)
		{
			SpawnEnemy(wave.enemy);
			yield return new WaitForSeconds(1f / wave.rate);
		}

		currentWave++;
	}

	private void SpawnEnemy(GameObject enemy)
		=> Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
}
