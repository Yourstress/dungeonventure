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
		Trap trap = coll.GetComponentInParent<Trap>();
		if (trap != null && !trap.isTrapActivated)
		{
			trap.ActivateTrap();

			// destroy Movement script
			Destroy(GetComponent<Movement>());
			Invoke("DieByTrap", 1);

			return true;
		}

		return false;
	}

	void DieByTrap()
	{
		Die("You've been sliced dead by a trap!");
	}

	bool ProcessEnemy(Collider2D coll)
	{
		Enemy enemy = coll.GetComponent<Enemy>();

		if (enemy != null)
		{
			Die("You've been slain by an enemy!");
			return true;
		}

		return false;
	}

	bool ProcessWeaponPickup(Collider2D coll)
	{
		Sword sword = coll.GetComponentInParent<Sword>();
		if (sword != null)
		{
			PickupWeapon(sword);

			return true;
		}

		Bow bow = coll.GetComponentInParent<Bow>();
		if (bow != null && !bow.isEquipped)
		{
			PickupWeapon(bow);

			return true;
		}

		return false;
	}

	void PickupWeapon(Weapon weapon)
	{
		weapon.SetPickedUp(true);

		if (currentWeapon != null)
		{
			Debug.Log("Dropping " + currentWeapon.name, currentWeapon.gameObject);
			currentWeapon.equippedParent.transform.SetParent(currentWeapon.transform);
			currentWeapon.SetPickedUp(false);
		}

		currentWeapon = weapon;
		Debug.Log("picking up " + currentWeapon.name, currentWeapon.gameObject);
		currentWeapon.equippedParent.transform.SetParent(weaponParent);
		currentWeapon.equippedParent.transform.localPosition = new Vector3(0, .33f, 0);
		currentWeapon.equippedParent.transform.localEulerAngles = Vector3.zero;
	}

	void Die(string message)
	{
		UI.Shared.ShowKilledScreen(message);

		Destroy(gameObject);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && currentWeapon != null)
			currentWeapon.Shoot();
	}
}