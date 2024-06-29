using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {

	[SerializeField] private Transform enemyPrefab;
	[SerializeField] private Transform spawnPoint;
	[SerializeField] private float timeBetweenWaves = 5f;
	[SerializeField] private Text waveCountdownText;
	
	private float countdown = 2f;
	private int currentWave = 0;

	private void Update()
	{
		if (countdown <= 0f)
		{
			StartCoroutine(SpawnWave());
			countdown = timeBetweenWaves;
		}

		countdown -= Time.deltaTime;
		countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
		
		waveCountdownText.text = $"{countdown:00.00}";
	}

	private IEnumerator SpawnWave()
	{
		currentWave++;
		
		for (var i = 0; i < currentWave; i++)
		{
			SpawnEnemy();
			yield return new WaitForSeconds(0.5f);
		}
	}

	private void SpawnEnemy()
		=> Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
}
