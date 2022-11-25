using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHealth : MonoBehaviour {
	public Health target;
	
	RectTransform rect;
	
	void Awake() {
		rect = GetComponent<RectTransform>();
		
		rect.anchorMin = Vector2.zero;
	}
	
	void LateUpdate() {
		rect.anchorMax = new Vector2(Mathf.Clamp01(target.percentHealth), 1f);
	}
}
