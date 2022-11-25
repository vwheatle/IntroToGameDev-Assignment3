﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour {
	EnemyManager em;
	
	bool alive = true;
	
	NavMeshAgent agent;
	Animator animator;
	
	public float hurtPower = 1f;
	public float hurtRange = 0.75f;
	// this is really naive, it checks the distance between this enemy
	// and the target's origin.. this means that large targets with
	// large colliders won't ever get hurt because they'll never be
	// "within the hurtRange".. How could this be fixed??
	// (i simply.. i guess maybe colliders have an IsInBoundingBox thing maybe?
	// physics systems tend to have that... but the parallel selves we're
	// tracking in the game don't have colliders!!)
	// This doesn't matter. I can just hard-code things.
	
	void Awake() {
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		
		em = transform.parent.GetComponent<EnemyManager>();
	}
	
	void Start() {
		// Begin by looking at thing to follow.
		GameObject nearestTarget = em.GetTargetNearestTo(transform.position);
		transform.rotation = Quaternion.LookRotation(
			nearestTarget.transform.position - transform.position,
			Vector3.up
		);
	}
	
	void LateUpdate() {
		if (alive) {
			GameObject nearestTarget = em.GetTargetNearestTo(transform.position);
			if (em.scared) {
				// Run away from thing, and don't even try to hurt them.
				// (Thought there was gonna be some built-in way, but
				//  this works fine too.)
				Vector3 away = transform.position - nearestTarget.transform.position;
				away = away.normalized * 3f;
				agent.destination = away;
			} else {
				// Approach thing.
				agent.destination = nearestTarget.transform.position;
				
				// If close, attack at an interval.
				if (WithinAttackDistance(nearestTarget)) {
					Attack(nearestTarget);
				}
			}
		}
	}
	
	bool WithinAttackDistance(GameObject target) {
		Vector3 targetXZ = new Vector3(
			target.transform.position.x,
			this.transform.position.y,
			target.transform.position.z
		);
		
		// Ensure close enough to target to attack.
		if (Vector3.Distance(this.transform.position, targetXZ) > hurtRange)
			return false;
		
		// Ensure no other enemies between this and target.
		RaycastHit hit;
		if (Physics.Linecast(
			this.transform.position, targetXZ,
			out hit,
			1 << LayerMask.NameToLayer("Shoot"),
			QueryTriggerInteraction.Ignore
		)) {
			// If we did get a hit, confirm it's the target.
			return hit.collider.gameObject == target;
		} else {
			// Sometimes, targets don't have colliders! Hurt anyway.
			return true;
		}
	}
	
	void Attack(GameObject target) {
		float thisPower = hurtPower * Random.Range(0.85f, 1.1f);
		target.SendMessage("Hurt", thisPower);
	}
	
	// === MESSAGES ===
	
	void WarpTo(Vector3 warpPosition) {
		agent.enabled = false;
		transform.position = warpPosition;
		agent.enabled = true;
	}
	
	void Scare() {
		agent.ResetPath();
	}
	
	void Die() {
		alive = false;
		animator.Play("Death");
		agent.ResetPath();
	}
	
	// Built-in animation event when dying...
	void StartSinking() {
		// Now this looks like a job for   ShrinkDie.cs
		ShrinkDie die = this.gameObject.AddComponent<ShrinkDie>();
		die.StartShrink();
		Destroy(this);
	}
	
	// Gizmo funny
	void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(
			this.transform.position,
			this.transform.position + this.transform.forward * hurtRange
		);
	}
}
