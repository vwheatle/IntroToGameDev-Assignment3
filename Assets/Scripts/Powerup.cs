using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {
	Transform inner;
	
	float spawnTime;
	
	void Awake() {
		inner = transform.GetChild(0);
		spawnTime = Time.time;
	}
	
	void Update() {
		inner.localRotation = Quaternion.Euler(
			45f, (Time.time - spawnTime) * 90f, 0f
		);
	}
}
