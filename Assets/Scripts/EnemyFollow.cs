using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour {
	List<GameObject> things;
	
	EnemyManager em;
	NavMeshAgent agent;
	
	void Start() {
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
	
	void Update() {
		// Approach thing.
		GameObject nearestTarget = em.GetTargetNearestTo(transform.position);
		agent.SetDestination(nearestTarget.transform.position);
	}
}
