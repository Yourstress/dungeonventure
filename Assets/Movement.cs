using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Movement : MonoBehaviour {


	Level level;
	LevelNavigator navigator;

	public Transform rotateSprite;
	public Transform rotateSidekick;
	public Transform sidekickCharacter;

	public float waitBetweenMoves = 0.1f;
	private float nextAllowedMoveAt = 0;
	Vector3 lastHeroPosition;

	// Use this for initialization
	void Start() {
		level = GameObject.FindObjectOfType<Level> ();
		navigator = new LevelNavigator (level.Entrance);
		transform.position = navigator.WorldPosition;
	}

	void Update() {	

		//get the last position the hero occupied, used to move sidekick

		// constrict movement steps to specific intervals
		if (Time.time < nextAllowedMoveAt)
			return;

		nextAllowedMoveAt = Time.time + waitBetweenMoves;
		
		bool moveHero = false;
		bool upWalkH = Input.GetKeyDown(KeyCode.UpArrow);
		bool rightWalkH = Input.GetKeyDown(KeyCode.RightArrow);
		bool downWalkH = Input.GetKeyDown(KeyCode.DownArrow);
		bool leftWalkH = Input.GetKeyDown(KeyCode.LeftArrow);

		bool moveSidekick = false;
		bool upWalkS = Input.GetKeyDown(KeyCode.W);
		bool rightWalkS = Input.GetKeyDown(KeyCode.D);
		bool downWalkS = Input.GetKeyDown(KeyCode.S);
		bool leftWalkS = Input.GetKeyDown(KeyCode.A);

		// rotate towards move target
		Vector3 targetRot = Vector3.zero;
		Vector3 sidekickRot = Vector3.zero;

		Vector3 prevWorldPosition = navigator.WorldPosition;
		
		if (upWalkH) {			
			if (!navigator.MoveUp ()) {
//				Debug.Log ("Nope!");
			} else
			{
				moveHero = true;
				targetRot.z = 0;
			}
		} 

		else if (rightWalkH) {			
			if (!navigator.MoveRight ()) {
//				Debug.Log ("Nope!");
			} else
			{
				moveHero = true;		
				targetRot.z = -90;
			}
		} 

		else if (downWalkH) {			
			if (!navigator.MoveDown ()) {
//				Debug.Log ("Nope!");
			} else
			{
				moveHero = true;		
				targetRot.z = 180;
			}
		} 

		else if (leftWalkH) {			
			if (!navigator.MoveLeft ()) {
//				Debug.Log ("Nope!");
			} else
			{
				moveHero = true;		
				targetRot.z = 90;
			}
		}
			

//		if (upWalkS) {			
//			if (!navigator.MoveUp ()) {
////				Debug.Log ("Nope!");
//			} else
//			{
//				moveSidekick = true;
//				sidekickRot.z = 0;
//			}
//		} 
//
//		else if (rightWalkS) {			
//			if (!navigator.MoveRight ()) {
////				Debug.Log ("Nope!");
//			} else
//			{
//				moveSidekick = true;		
//				sidekickRot.z = -90;
//			}
//		} 
//
//		else if (downWalkS) {			
//			if (!navigator.MoveDown ()) {
////				Debug.Log ("Nope!");
//			} else
//			{
//				moveSidekick = true;		
//				sidekickRot.z = 180;
//			}
//		} 
//
//		else if (leftWalkS) {			
//			if (!navigator.MoveLeft ()) {
////				Debug.Log ("Nope!");
//			} else
//			{
//				moveSidekick = true;		
//				sidekickRot.z = 90;
//			}
//		}
//					

		if (moveHero) {
			// rotate towards target
			rotateSprite.DOLocalRotate(targetRot, 0.15f);
			lastHeroPosition = transform.position;
			transform.DOMove (navigator.WorldPosition, 0.2f, false);




			//measure the distance between the hero and the sidekick
			float distanceBetweenCharactersX = transform.position.x - sidekickCharacter.transform.position.x;
			float distanceBetweenCharactersY = transform.position.y - sidekickCharacter.transform.position.y;

			//if the sidekick is in a valid position, leave him be
			//otherwise, move him to the hero's last position
			if (distanceBetweenCharactersX >= 1.0f && distanceBetweenCharactersX <= 1.5f ||
				distanceBetweenCharactersX >= -1.0f && distanceBetweenCharactersX <= -1.5f ||
				distanceBetweenCharactersY >= 1.0f && distanceBetweenCharactersY <= 1.5f ||
				distanceBetweenCharactersY >= -1.0f && distanceBetweenCharactersY <= -1.5f) {

				//rotate to face same direction as player
				rotateSidekick.DOLocalRotate(targetRot, 0.15f);
				sidekickCharacter.transform.DOMove (lastHeroPosition, 0.2f, false);

			}

			//teleport if sidekick is too far away
			else if (distanceBetweenCharactersX >= 1.5f || distanceBetweenCharactersX <= -1.5f ||
				distanceBetweenCharactersY >= 1.5f || distanceBetweenCharactersY <= -1.5f) {

				rotateSidekick.DOLocalRotate(targetRot, 0.15f);
				sidekickCharacter.transform.DOMove (lastHeroPosition, 0.2f, false);
			}
			else
				sidekickCharacter.transform.DOMove (lastHeroPosition, 0.2f, false);

			Vector3 diff = lastHeroPosition - navigator.WorldPosition;
			float angle = 0;
			if (diff.x > 0.1f)
				angle = -90;
			else if (diff.x < -.1f)
				angle = 90;
			if (diff.y > 0.1f)
				angle = 0;
			else if (diff.y < -0.1f)
				angle = 180;
			rotateSidekick.DOLocalRotate(new Vector3(0,0,angle), 0.15f).SetDelay(.15f);
		}

//		if (moveSidekick) {
//			// rotate towards target
//			rotateSidekick.DOLocalRotate(sidekickRot, 0.15f);
//			sidekickCharacter.transform.DOMove (prevWorldPosition, 0.2f, false);
//
//		}
//		sidekickCharacter.transform.position = lastHeroPosition;
				 
//		return;

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
