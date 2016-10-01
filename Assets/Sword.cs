using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sword : Weapon {

	bool isShooting = false;

	Collider2D hitCollider;
	void Awake()
	{
		hitCollider = equippedParent.GetComponentInChildren<Collider2D>();
		hitCollider.enabled = false;
	}

	public override void Shoot ()
	{
		if (isShooting)
			return;
		
		isShooting = true;

		Vector3 rot = new Vector3(0,0,90);
		equippedParent.transform.DOLocalRotate(rot, .2f).OnComplete(() =>
			{
				hitCollider.enabled = true;
				equippedParent.transform.DOLocalRotate(-rot, .1f).SetEase(Ease.OutExpo).OnComplete(() =>
					{
						rot.z = 0;
						equippedParent.transform.DOLocalRotate(rot, .2f).OnComplete(() =>
							{
								hitCollider.enabled = false;
								isShooting = false;
							});
					});
			});
	}
}
