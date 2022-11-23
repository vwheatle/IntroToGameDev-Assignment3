using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour {
	EnemyManager em;
	
	bool alive = true;
	
	NavMeshAgent agent;
	Animator animator;
	
	public float maxHealth = 3f;
	float health = -1f;
	
	void Start() {
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		
		em = transform.parent.GetComponent<EnemyManager>();
		
		// Begin by looking at thing to follow.
		GameObject nearestTarget = em.GetTargetNearestTo(transform.position);
		transform.rotation = Quaternion.LookRotation(
			nearestTarget.transform.position - transform.position,
			Vector3.up
		);
		
		health = maxHealth;
	}
	
	void LateUpdate() {
		if (alive) {
			GameObject nearestTarget = em.GetTargetNearestTo(transform.position);
			if (em.scared) {
				// Run away from thing.
				// (Thought there was gonna be some built-in way, but
				//  this works fine too.)
				Vector3 away = transform.position - nearestTarget.transform.position;
				away = away.normalized * 3f;
				agent.destination = away;
			} else {
				// Approach thing.
				agent.destination = nearestTarget.transform.position;
			}
		}
	}
	
	void WarpTo(Vector3 warpPosition) {
		agent.enabled = false;
		transform.position = warpPosition;
		agent.enabled = true;
	}
	
	void Scare() {
		agent.ResetPath();
	}
	
	void Hurt(float amount) {
		health -= amount;
		if (health < 0.01f) {
			alive = false;
			animator.Play("Death");
			agent.ResetPath();
		}
	}
	
	// Built-in animation event...
	void StartSinking() {
		// Now this looks like a job for   ShrinkDie.cs
		ShrinkDie die = this.gameObject.AddComponent<ShrinkDie>();
		die.StartShrink();
		Destroy(this);
	}
}
