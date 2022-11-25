using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHealth3D : MonoBehaviour {
	public Health target;
	
	LineRenderer line;
	
	public Color scaredColor = Color.blue;
	public Color unscaredColor = Color.red;
	
	void Awake() {
		line = transform.GetChild(0).GetComponent<LineRenderer>();
		
	}
	
	void Start() {
		// hack
		if ((target?.transform.parent.GetComponent<EnemyManager>()?.isScared).GetValueOrDefault())
			Scare();
	}
	
	void LateUpdate() {
		// Update health bar.
		line.SetPosition(1, Vector3.right * target.percentHealth);
		
		// Stare at camera. Unblinking
		transform.rotation = Quaternion.LookRotation(
			transform.position - Camera.main.transform.position
		);
	}
	
	void Die() { Destroy(this.gameObject); }
	
	void Scare() { line.startColor = line.endColor = scaredColor; }
	void Unscare() { line.startColor = line.endColor = unscaredColor; }
}
