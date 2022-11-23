using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
	public List<Vector2> spawns;
	public List<GameObject> spawnables;
	
	public float spawnInterval = 5f;
	float spawnTime = 0f;
	
	public List<GameObject> avoidTargets;
	
	public float minTargetDistance = 12f;
	public float minAnythingElseDistance = 3.5f;
	
	public bool giveUpIfSpawnFails = false;
	
	// === UNITY CALLBACK ZONE ===
	
	void Start() {
		spawnTime = Time.time;
	}
	
	void Update() {
		float timePassed = Time.time - spawnTime;
		if (timePassed >= spawnInterval) {
			if (SpawnSpawnable() || giveUpIfSpawnFails) {
				// Reset the timer.
				spawnTime = Time.time;
			} else {
				Debug.Log("failed to spawn anything!");
			}
		}
	}
	
	// === SPAWN LOGIC ZONE ===
	
	bool SpawnSpawnable() {
		Vector3? spawnPos = GetNewSpawnPosition();
		if (spawnPos.HasValue) {
			// If all succeeds, spawn a thing!
			Instantiate(
				spawnables[Random.Range(0, spawnables.Count)],
				spawnPos.Value, Quaternion.identity, this.transform
			);
		}
		return spawnPos.HasValue;
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
	
	GameObject GetTargetNearestTo(Vector3 position) {
		GameObject nearest = null;
		Vector3 nearestOffset = Vector3.positiveInfinity;
		
		foreach (GameObject target in avoidTargets) {
			Vector3 offset = target.transform.position - position;
			if (offset.magnitude < nearestOffset.magnitude) {
				nearest = target;
				nearestOffset = offset;
			}
		}
		
		return nearest;
	}
	
	// === GIZMO ZONE ===
	
	void DrawX(Vector3 position, float size) {
		Vector3 v1 = new Vector3(+1f, 0f, 1f) * size;
		Vector3 v2 = new Vector3(-1f, 0f, 1f) * size;
		
		Gizmos.DrawLine(position - v1, position + v1);
		Gizmos.DrawLine(position - v2, position + v2);
	}
	
	void OnDrawGizmosSelected() {
		Gizmos.color = Color.green;
		foreach (Vector2 spawn in spawns)
			Gizmos.DrawWireSphere(new Vector3(spawn.x, 0.5f, spawn.y), 1f);
		
		Gizmos.color = Color.red;
		foreach (GameObject target in avoidTargets)
			if (target) DrawX(target.transform.position, 1f);
	}
}
