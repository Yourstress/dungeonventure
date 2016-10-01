using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Movement : MonoBehaviour {


	Level level;
	// keep a reference to this somewhere
	LevelNavigator navigator;

	public Transform rotateSprite;

	public float waitBetweenMoves = 0.1f;
	private float nextAllowedMoveAt = 0;

	// Use this for initialization
	void Start() {
		level = GameObject.FindObjectOfType<Level> ();
		navigator = new LevelNavigator (level.Entrance);
		transform.position = navigator.WorldPosition;
	}

	void Update() {	

		// constrict movement steps to specific intervals
		if (Time.time < nextAllowedMoveAt)
			return;

		nextAllowedMoveAt = Time.time + waitBetweenMoves;
		
		bool move = false;
		bool upWalkH = Input.GetKey(KeyCode.UpArrow);
		bool rightWalkH = Input.GetKey(KeyCode.RightArrow);
		bool downWalkH = Input.GetKey(KeyCode.DownArrow);
		bool leftWalkH = Input.GetKey(KeyCode.LeftArrow);

		// rotate towards move target
		Vector3 targetRot = Vector3.zero;
		
		if (upWalkH) {			
			if (!navigator.MoveUp ()) {
				Debug.Log ("Nope!");
			} else
			{
				move = true;
				targetRot.z = 0;
			}
		} 

		else if (rightWalkH) {			
			if (!navigator.MoveRight ()) {
				Debug.Log ("Nope!");
			} else
			{
				move = true;		
				targetRot.z = -90;
			}
		} 

		else if (downWalkH) {			
			if (!navigator.MoveDown ()) {
				Debug.Log ("Nope!");
			} else
			{
				move = true;		
				targetRot.z = 180;
			}
		} 

		else if (leftWalkH) {			
			if (!navigator.MoveLeft ()) {
				Debug.Log ("Nope!");
			} else
			{
				move = true;		
				targetRot.z = 90;
			}
		}


			

		if (move) {
			// rotate towards target
			rotateSprite.DOLocalRotate(targetRot, 0.15f);

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
