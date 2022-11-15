using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	public GameObject thing;
	
	float moveSpeed = 0.25f;
	
	Vector3 offset;
	Vector3 currVelocity;
	
	void Start() {
		offset = this.transform.position - thing.transform.position;
		currVelocity = Vector3.zero;
	}
	
	void Update() {
		this.transform.position = Vector3.SmoothDamp(
			this.transform.position,
			thing.transform.position + offset,
			ref currVelocity,
			moveSpeed
		);
	}
}
