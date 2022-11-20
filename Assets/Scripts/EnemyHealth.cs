using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
	new Renderer renderer;
	
	public Material otherMaterial;
	
	bool fearful_ = false;
	public bool fearful {
		get => fearful_;
		set {
			(this.renderer.material, this.otherMaterial) =
				(this.otherMaterial, this.renderer.material);
			fearful_ = value;
		}
	}
	
	void Start() {
		renderer = GetComponent<Renderer>();
	}
	
	void Update() {
		// Stare at camera. Unblinking
		transform.rotation = Quaternion.LookRotation(
			transform.position - Camera.main.transform.position
		);
	}
	
	void Scare() { fearful = !fearful; }
}
