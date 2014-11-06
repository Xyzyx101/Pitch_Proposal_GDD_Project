using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	public int minWaveSize;
	public int maxWaveSize;
	public float minTimeBetweenWaves;
	public float maxTimeBetweenWaves;
	public Transform enemyPrefab;
	float nextWaveTime;
	int nextWaveSize;
	int enemiesToSpawn;
	float timeBetweenEnemies = 0.75f;
	float timeSinceLastEnemy;
	bool spawning;

	// Use this for initialization
	void Start () {
		NextWave ();
	}
	
	// Update is called once per frame
	void Update () {
		nextWaveTime -= Time.deltaTime;
		if (nextWaveTime < 0 && !spawning) {
			spawning = true;
			timeSinceLastEnemy = 0;
		}
		timeSinceLastEnemy -= Time.deltaTime;
		if (spawning && timeSinceLastEnemy <= 0) {
			Instantiate(enemyPrefab, transform.position, Quaternion.identity);
			timeSinceLastEnemy = timeBetweenEnemies;
			nextWaveSize -= 1;
		}
		if (nextWaveSize == 0) {
			NextWave();
		}
	}

	void NextWave() {
		spawning = false;
		nextWaveTime = Random.Range (minTimeBetweenWaves, maxTimeBetweenWaves);
		nextWaveSize = Random.Range (minWaveSize, maxWaveSize);
		print (nextWaveSize);
	}
}
