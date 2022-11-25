using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverListener : MonoBehaviour {
	// bad design makes me sad but some times it cannot be avoided.
	
	Animator anim;
	
	public TMP_Text highScoreText;
	
	void Awake() {
		anim = GetComponent<Animator>();
	}
	
	// OH MY GOD JUST LET ME DESTRUCTURE THIS TUPLE INSIDE THE   DAMN FUNCTIO N SIGNATURE
	void GameOver((bool, int) scoreAndIfItsNewRecord) {
		(bool newRecord, int highScore) = scoreAndIfItsNewRecord;
		
		anim.SetBool("GameOver", true);
		
		highScoreText.text = newRecord ? $"New Record!\n{highScore} Kills" : "";
	}
}
