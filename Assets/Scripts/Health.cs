using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
	public float maxHealth = 10f;
	float health = -1f;
	
	public float currentHealth { get => health; }
	public float percentHealth { get => Mathf.Clamp01(health / maxHealth); }
	
	void Awake() {
		health = maxHealth;
	}
	
	public void Heal(float amount) {
		health = Mathf.Clamp(health + amount, 0f, maxHealth);
	}
	
	// code smell.. :(
	
	void Hurt((GameObject, float) culpritAndAmount) {
		(GameObject culprit, float amount) = culpritAndAmount;
		health -= amount;
		if (health <= 0f) {
			this.gameObject.BroadcastMessage(
				"Die", culprit, SendMessageOptions.DontRequireReceiver
			);
			culprit?.SendMessage("Killer", SendMessageOptions.DontRequireReceiver);
			Destroy(this);
		}
	}
}
