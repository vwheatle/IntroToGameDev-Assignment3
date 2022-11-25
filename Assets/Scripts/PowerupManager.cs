using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour {
	public EnemyManager em;
	
	public void BecomeFeared(GameObject target) {
		em.BroadcastMessage("Scare");
		target.SendMessage("Strong");
	}
	
	public void HealTarget(GameObject target) {
		Health health = target.GetComponent<Health>();
		
		// Nice to you. (If you have really low health, it gives you a bit more.)
		health.Heal(Mathf.Clamp(health.percentHealth, 0.1f, 1f) * health.maxHealth);
	}
}
