using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour {
	public GameObject thing;
	
	NavMeshAgent agent;
	
	void Start() {
		agent = GetComponent<NavMeshAgent>();
		
		// Begin by looking at thing to follow.
		transform.rotation = Quaternion.LookRotation(
			thing.transform.position - transform.position,
			Vector3.up
		);
	}
	
	void Update() {
		// Approach thing.
		agent.SetDestination(thing.transform.position);
	}
}
