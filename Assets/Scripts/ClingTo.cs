﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClingTo : MonoBehaviour {
	public Transform thing;
	
	void LateUpdate() {
		this.transform.position = thing.transform.position;
	}
	
	// Repeat Hurt message
	void Hurt((GameObject, float) packet) {
		thing.SendMessage("Hurt", packet, SendMessageOptions.DontRequireReceiver);
	}
}
