using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	ModularTileMap currentSegment;
	LevelNavigator nav;

	public bool isDead { get; private set; }
	// Use this for initialization
	void Start () {
		currentSegment = GetComponentInParent<ModularTileMap>();
		nav = new LevelNavigator(currentSegment);
		nav.MoveToWorld(transform.position);

		StartCoroutine(MoveEnemy());
	}

	IEnumerator MoveEnemy()
	{
		while (!isDead)
		{
			int moveX = Random.Range(-1, 2);
			int moveY = Random.Range(-1, 2);

			// only move in one direction
			if (moveX != 0 && moveY != 0)
			{
				if (Random.Range(0, 2) == 0)
					moveX = 0;
				else
					moveY = 0;
			}

			bool moveSuccess = nav.Move(moveX, moveY);

			if (moveSuccess)
			{
				transform.position = nav.WorldPosition;
				
				yield return new WaitForSeconds(1);
			}
			else
				yield return null;
		}
	}
}
