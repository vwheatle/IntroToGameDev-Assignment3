using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayBestKills : MonoBehaviour {
	public PlayerMove target;
	
	TMP_Text text;
	
	void Awake() {
		text = GetComponent<TMP_Text>();
	}
	
	void Start() {
		text.text = $"Beat {PlayerPrefs.GetInt("BestKills", 10)}!";
	}
}
