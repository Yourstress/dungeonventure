using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Movement : MonoBehaviour {


	Level level;
	// keep a reference to this somewhere
	LevelNavigator navigator;

	// Use this for initialization
	void Start() {
		level = GameObject.FindObjectOfType<Level> ();
		navigator = new LevelNavigator (level.Entrance);
		transform.position = navigator.WorldPosition;
	}

	void Update() {	

		bool move = false;
		bool upWalkH = Input.GetKeyDown(KeyCode.UpArrow);
		bool rightWalkH = Input.GetKeyDown(KeyCode.RightArrow);
		bool downWalkH = Input.GetKeyDown(KeyCode.DownArrow);
		bool leftWalkH = Input.GetKeyDown(KeyCode.LeftArrow);

		if (upWalkH) {			
			if (!navigator.MoveUp ()) {
				Debug.Log ("Nope!");
			} else
				move = true;
		} 

		else if (rightWalkH) {			
			if (!navigator.MoveRight ()) {
				Debug.Log ("Nope!");
			} else
				move = true;		
		} 

		else if (downWalkH) {			
			if (!navigator.MoveDown ()) {
				Debug.Log ("Nope!");
			} else
				move = true;		
		} 

		else if (leftWalkH) {			
			if (!navigator.MoveLeft ()) {
				Debug.Log ("Nope!");
			} else
				move = true;		
		}

			

		if (move) {
			transform.DOMove (navigator.WorldPosition, 0.2f, false);
		}

	}
}


/*
Level level = GameObject.FindObjectOfType<Level> ();

            // keep a reference to this somewhere
            LevelNavigator navigator = new LevelNavigator (level.Entrance);

            // all move functions return a bool. if it returns null, it's an invalid tile (wall, tree, etc..)
            if (!navigator.MoveLeft ())
                Debug.Log ("Nope!");

            // move the character there.. we can lerp/tween it later right here
            character.transform.position = navigator.WorldPosition;
*/
