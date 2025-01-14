﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpZone : MonoBehaviour {
	public Transform jumpTo;
	// PLEASE DON'T OVERLAP THIS DESTINATION WITH ANOTHER WARP'S TRIGGER! (lol)
	
	void OnTriggerEnter(Collider other) {
		// Debug.Log($"Calling 'WarpTo' on {other.gameObject.name}");
		bool prevStatus = other.enabled;
		Vector3 warpPosition = (other.transform.position - this.transform.position) + jumpTo.position;
		other.gameObject.SendMessage("WarpTo", warpPosition, SendMessageOptions.DontRequireReceiver);
	}
	
	void OnDrawGizmosSelected() {
		if (!this.jumpTo) return;
		
		Gizmos.color = Color.cyan;
		
		// Yes i am drawing debug beziers. Sorry.
		Vector3 middleControlPoint = Vector3.Lerp(this.transform.localPosition, this.jumpTo.position, 0.5f);
		middleControlPoint.y += Vector3.Distance(this.transform.localPosition, this.jumpTo.position) / 2;
		
		int points = 8;
		Vector3 last = this.transform.localPosition, next;
		for (int i = 1; i <= points; i++) {
			float progress = (float)i / points;
			next = Vector3.Lerp(
				Vector3.Lerp(this.transform.localPosition, middleControlPoint, progress),
				Vector3.Lerp(middleControlPoint, this.jumpTo.position, progress),
				progress
			);
			Gizmos.DrawLine(last, next);
			last = next;
		}
		Gizmos.DrawWireCube(this.jumpTo.position, this.transform.localScale);
	}
}
