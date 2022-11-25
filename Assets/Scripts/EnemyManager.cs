using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
	public List<GameObject> targets;
	
	public float scareTimeLength = 7f;
	
	bool scared = false;
	float scareStartTime = 0f;
	
	public bool isScared { get => scared; }
	
	void LateUpdate() {
		if (scared && (Time.time - scareStartTime) >= scareTimeLength) {
			scared = false;
			BroadcastMessage("Unscare", SendMessageOptions.DontRequireReceiver);
		}
	}
	
	public void Scare() {
		scared = true;
		scareStartTime = Time.time;
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
	
	void GizmosDrawX(Vector3 position, float size) {
		Vector3 v1 = new Vector3(+1f, 0f, 1f) * size;
		Vector3 v2 = new Vector3(-1f, 0f, 1f) * size;
		
		Gizmos.DrawLine(position - v1, position + v1);
		Gizmos.DrawLine(position - v2, position + v2);
	}
	
	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		foreach (GameObject target in targets) {
			if (!target) continue;
			GizmosDrawX(target.transform.position, 1f);
		}
	}
}
