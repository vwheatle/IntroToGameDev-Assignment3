using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {
	PowerupManager pm;
	Transform inner;
	new AudioSource audio;
	
	public string collectEvent;
	
	float spawnTime;
	
	void Awake() {
		pm = transform.parent.GetComponent<PowerupManager>();
		inner = transform.GetChild(0);
		audio = GetComponent<AudioSource>();
		spawnTime = Time.time;
	}
	
	void LateUpdate() {
		inner.localRotation = Quaternion.Euler(
			45f, Mathf.Sin(Mathf.PI * (Time.time - spawnTime)) * 30f, 0f
		);
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			pm.SendMessage(collectEvent, other.gameObject);
			
			audio.pitch = Random.Range(0.9f, 1.1f);
			audio.Play();
			
			// shoutouts to shrinkdie for real.
			ShrinkDie die = this.gameObject.AddComponent<ShrinkDie>();
			die.StartShrink();
			Destroy(this);
		}
	}
}
