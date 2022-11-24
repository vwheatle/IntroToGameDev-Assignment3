using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	public GameObject thing;
	
	float moveSpeed = 0.25f;
	float snapDistance = 24f;
	
	Vector3 offset;
	Vector3 lastPosition;
	Vector3 currVelocity;
	
	void Awake() {
		// Risky but eh, it's supposed to follow the player's initial
		// position, not any position after they're moving.
		offset = this.transform.position - thing.transform.position;
		lastPosition = this.transform.position;
		currVelocity = Vector3.zero;
	}
	
	void LateUpdate() {
		Vector3 target = thing.transform.position + offset;
		if ((target - this.transform.position).magnitude > snapDistance) {
			this.transform.position += thing.transform.position - lastPosition;
			currVelocity = Vector3.zero;
		}
		
		this.transform.position = Vector3.SmoothDamp(
			this.transform.position,
			thing.transform.position + offset,
			ref currVelocity,
			moveSpeed
		);
		lastPosition = thing.transform.position;
	}
}
