using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.name == "arrow" || col.name == "sprite"/*sword*/)
			Destroy(gameObject);
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
			// both randoms 0? choose a dimension and move along it randomly
			else if (moveX == 0 && moveY == 0)
			{
				int val = Random.Range(0, 2);
				if (val == 0)
					val = -1;

				if (Random.Range(0, 2) == 0)
					moveX = val;
				else
					moveY = val;
			}

			bool moveSuccess = nav.Move(moveX, moveY);

			if (moveSuccess)
			{
				// rotate towards move target
				Vector3 targetRot = Vector3.zero;

				if (moveX > 0)
					targetRot.z = -90;
				else if (moveX < 0)
					targetRot.z = 90;
				else if (moveY > 0)
					targetRot.z = 0;
				else if (moveY < 0)
					targetRot.z = 180;
				transform.DOLocalRotate(targetRot, 0.15f);

				// move there instantly
				//				transform.position = nav.WorldPosition;

				// tween transform there
				transform.DOMove(nav.WorldPosition, 0.4f).SetDelay(0.35f);

				yield return new WaitForSeconds(1 + Random.Range(0, .35f));
			}
			else
				yield return null;
		}
	}
}
