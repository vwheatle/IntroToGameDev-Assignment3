using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
	public List<GameObject> targets;
	
	void Start() {
		
	}
	
	void Update() {
		
	}
	
	public GameObject GetTargetNearestTo(Vector3 position) {
		GameObject nearest = null;
		Vector3 nearestOffset = Vector3.positiveInfinity;
		
		foreach (GameObject target in targets) {
			Vector3 offset = target.transform.position - position;
			if (offset.magnitude < nearestOffset.magnitude) {
				nearest = target;
				nearestOffset = offset;
			}
		}
		
		return nearest;
	}
}
