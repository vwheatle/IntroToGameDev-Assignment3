using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkDie : MonoBehaviour {
	bool _active = false;
	
	public bool active { get => _active; }
	
	float startTime = 0f;
	public float deathTime = 0.5f;
	
	Vector3 initialScale = Vector3.one;
	
	void Awake() {
		initialScale = transform.localScale;
	}
	
	public void StartShrink() {
		startTime = Time.unscaledTime;
		_active = true;
	}
	
	void LateUpdate() {
		if (!_active) return;
		
		float diffTime = Time.unscaledTime - startTime;
		float percentDead = diffTime / deathTime;
		
		float scalarScale = Mathf.Max(0f, 1f - percentDead);
		transform.localScale = Vector3.one * scalarScale;
		
		if (percentDead >= 1f) {
			_active = false;
			Destroy(this.gameObject);
		}
	}
}
