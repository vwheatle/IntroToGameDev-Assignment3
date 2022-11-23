using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
	public List<GameObject> targets;
	
	bool scared_ = false;
	public bool scared {
		get => scared_;
	}
	
	void Scare() {
		scared_ = !scared_;
		foreach (Transform t in this.transform) {
			t.SendMessage("Scare", SendMessageOptions.DontRequireReceiver);
		}
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
	
	void DrawX(Vector3 position, float size) {
		Vector3 v1 = new Vector3(+1f, 0f, 1f) * size;
		Vector3 v2 = new Vector3(-1f, 0f, 1f) * size;
		
		Gizmos.DrawLine(position - v1, position + v1);
		Gizmos.DrawLine(position - v2, position + v2);
	}
	
	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		foreach (GameObject target in targets) {
			if (!target) continue;
			DrawX(target.transform.position, 1f);
		}
	}
}
