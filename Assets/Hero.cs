using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
	Weapon currentWeapon = null;
	public Transform weaponParent;

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (ProcessTrap(coll))
			return;

		if (ProcessEnemy(coll))
			return;

		if (ProcessWeaponPickup(coll))
			return;
	}

	bool ProcessTrap(Collider2D coll)
	{
		Trap trap = coll.GetComponent<Trap>();
		if (trap != null && !trap.isTrapActivated)
		{
			trap.ActivateTrap();

			Die();

			return true;
		}

		return false;
	}

	bool ProcessEnemy(Collider2D coll)
	{
		Enemy enemy = coll.GetComponent<Enemy>();

		if (enemy != null)
		{
			Destroy(enemy.gameObject);
			return true;
		}

		return false;
	}

	bool ProcessWeaponPickup(Collider2D coll)
	{
		Sword sword = coll.GetComponentInParent<Sword>();
		if (sword != null)
		{
			Debug.Log("Picked up");
			sword.SetPickedUp(true);
			return true;
		}

		Bow bow = coll.GetComponentInParent<Bow>();
		if (bow != null && !bow.isEquipped)
		{
			bow.SetPickedUp(true);

			if (currentWeapon != null)
			{
				currentWeapon.equippedParent.transform.SetParent(currentWeapon.transform);
				currentWeapon.SetPickedUp(false);
			}

			currentWeapon = bow;
			currentWeapon.equippedParent.transform.SetParent(weaponParent);
			currentWeapon.equippedParent.transform.localPosition = new Vector3(0, .33f, 0);
			currentWeapon.equippedParent.transform.localEulerAngles = Vector3.zero;

			return true;
		}

		return false;
	}

	void Die()
	{
		Destroy(gameObject);
	}
}