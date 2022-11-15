using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
	CharacterController cc;
	
	public float moveSpeed = 2f;
	
	void Start() {
		cc = GetComponent<CharacterController>();
	}
	
	void Update() {
		Vector2 wasd = new Vector2(
			Input.GetAxisRaw("Horizontal"),
			Input.GetAxisRaw("Vertical")
		).normalized * moveSpeed;
		
		Vector3 move = new Vector3(
			wasd.x, 0f, wasd.y
		);
		
		cc.Move(move * Time.deltaTime);
		
		Vector3 a = this.transform.position;
		a.y = 0f; this.transform.position = a;
	}
}
