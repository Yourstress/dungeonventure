using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public GameObject floorParent;
	public GameObject equippedParent;

	public bool isEquipped { get; private set; }

	public void SetPickedUp(bool isPickedUp)
	{
		if (isEquipped == isPickedUp)
			return;
		
		isEquipped = isPickedUp;

		floorParent.SetActive(!isEquipped);
		equippedParent.SetActive(isEquipped);
	}

	public virtual void Shoot()
	{
	}
}
