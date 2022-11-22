using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
	public List<Vector2> spawns;
	public List<GameObject> targets;
	
	public List<GameObject> spawnableEnemies;
	public float enemySpawnInterval = 5f;
	float enemySpawnTime = 0f;
	
	public float minTargetDistance = 12f;
	public float minAnythingElseDistance = 3.5f;
	
	void Setup() {
		enemySpawnTime = Time.time;
	}
	
	void Update() {
		float timePassed = Time.time - enemySpawnTime;
		if (timePassed >= enemySpawnInterval) {
			SpawnEnemy();
		}
	}
	
	Vector3? GetNewSpawnPosition() {
		int tries = spawns.Count;
		int firstTry = Random.Range(0, tries), tryOffset;
		
		// Try the randomly-selected item first, and if it fails,
		// continue down the list until you hit firstTry again.
		// If you haven't spawned anything by then, try again next frame.
		
		for (tryOffset = 0; tryOffset < tries; tryOffset++) {
			int trySpawn = (firstTry + tryOffset) % tries;
			
			// Unpack the position. (I really wish there was a Vector3.xz(Vector2) constructor..)
			Vector3 spawnPos = new Vector3(spawns[trySpawn].x, 0f, spawns[trySpawn].y);
			
			// Check for if the moving target (player) is too close to safely spawn.
			GameObject target = GetTargetNearestTo(spawnPos);
			if (Vector3.Distance(target.transform.position, spawnPos) < minTargetDistance)
				continue;
			
			// Check for if another enemy is too close to safely spawn.
			Vector3 bounds = new Vector3(minAnythingElseDistance, 1f, minAnythingElseDistance) / 2;
			if (Physics.CheckBox(spawnPos, bounds))
				continue;
			
			// If all succeeds, return the position!
			return spawnPos;
		}
		
		// Failed to spawn a thing!
		return null;
	}
	
	void SpawnEnemy() {
		Vector3? spawnPos = GetNewSpawnPosition();
		if (spawnPos.HasValue) {
			// If all succeeds, spawn a thing!
			Instantiate(
				spawnableEnemies[Random.Range(0, spawnableEnemies.Count)],
				spawnPos.Value, Quaternion.identity, this.transform
			);
			
			// Reset the timer.
			enemySpawnTime = Time.time;
		} else {
			// Failed to spawn a thing!
			Debug.Log("failed to spawn anything!");
		}
	}
	
	public GameObject GetTargetNearestTo(Vector3 position) {
		GameObject nearest = null;
		Vector3 nearestOffset = Vector3.positiveInfinity;
		
		foreach (GameObject target in targets) {
			Vector3 offset = target.transform.position - position;
			if (offset.magnitude < nearestOffset.magnitude) {
				nearest = target;
				nearestOffset = offset;
			}
		}
		
		return nearest;
	}
	
	void DrawX(Vector3 position, float size) {
		Vector3 v1 = new Vector3(+1f, 0f, 1f) * size;
		Vector3 v2 = new Vector3(-1f, 0f, 1f) * size;
		
		Gizmos.DrawLine(position - v1, position + v1);
		Gizmos.DrawLine(position - v2, position + v2);
	}
	
	void OnDrawGizmosSelected() {
		Gizmos.color = Color.green;
		foreach (Vector2 spawn in spawns) {
			Gizmos.DrawWireSphere(new Vector3(spawn.x, 0.5f, spawn.y), 1f);
		}
		
		Gizmos.color = Color.red;
		foreach (GameObject target in targets) {
			if (!target) continue;
			DrawX(target.transform.position, 1f);
		}
	}
}
