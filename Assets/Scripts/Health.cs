using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
	public float maxHealth = 10f;
	float health = -1f;
	
	public float percentHealth {
		get => health / maxHealth;
	}
	
	void Awake() {
		health = maxHealth;
	}
	
	void Hurt(float amount) {
		health -= amount;
		if (health <= 0f) {
			this.gameObject.BroadcastMessage(
				"Die", SendMessageOptions.DontRequireReceiver
			);
			Destroy(this);
		}
	}
}
