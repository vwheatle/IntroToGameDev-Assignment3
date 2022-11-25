using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayKills : MonoBehaviour {
	public PlayerMove target;
	
	TMP_Text text;
	
	void Awake() {
		text = GetComponent<TMP_Text>();
	}
	
	void LateUpdate() {
		text.text = $"{target.killCount} Kills";
		text.color = target.isStrong ? Color.yellow : Color.white;
		text.fontStyle = target.isStrong
			? (text.fontStyle | FontStyles.Bold)
			: (text.fontStyle & ~FontStyles.Bold);
	}
}
