using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour {
	List<GameObject> things;
	
	EnemyHealth health;
	EnemyManager em;
	NavMeshAgent agent;
	
	void Start() {
		health = GetComponentInChildren<EnemyHealth>();
		
		agent = GetComponent<NavMeshAgent>();
		
		em = transform.parent.GetComponent<EnemyManager>();
		things = em.targets;
		
		// Begin by looking at thing to follow.
		GameObject nearestTarget = em.GetTargetNearestTo(transform.position);
		transform.rotation = Quaternion.LookRotation(
			nearestTarget.transform.position - transform.position,
			Vector3.up
		);
	}
	
	void LateUpdate() {
		GameObject nearestTarget = em.GetTargetNearestTo(transform.position);
		if (health.fearful) {
			// Run away from thing.
			// (Thought there was gonna be some built-in way, but
			//  this works fine too.)
			Vector3 away = transform.position - nearestTarget.transform.position;
			away = away.normalized * 10f;
			agent.destination = away;
		} else {
			// Approach thing.
			agent.destination = nearestTarget.transform.position;
		}
	}
	
	void WarpTo(Vector3 warpPosition) {
		agent.enabled = false;
		transform.position = warpPosition;
		agent.enabled = true;
	}
	
	void Scare() { agent.ResetPath(); }
}
