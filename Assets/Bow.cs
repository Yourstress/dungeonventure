using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bow : Weapon
{
	public GameObject arrow;
	public float arrowSpeed = 1;
	public float arrowDistance = 3;

	private bool isReady = true;

	Collider2D arrowCollider;
	void Awake()
	{
		arrowCollider = GetComponentInChildren<Collider2D>();
		arrowCollider.enabled = false;

	}

	public override void Shoot()
	{
		if (!isReady)
			return;
		
		isReady = false;
		arrow.transform.DOLocalMoveY(arrowDistance, arrowSpeed).OnComplete(Recycle).SetEase(Ease.InBack).SetSpeedBased();

		Invoke("EnableCollider", .1f);
	}

	void EnableCollider()
	{
		arrowCollider.enabled = false;
	}

	void Recycle()
	{
		isReady = true;

		arrow.transform.localPosition = Vector3.zero;
		arrow.transform.localScale = Vector3.zero;
		arrow.transform.DOScale(Vector3.one, .3f);

		arrowCollider.enabled = false;
	}
}
