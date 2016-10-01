using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

	public AudioSource trapAudioSource;

	public GameObject openTrap, closedTrap;

	public bool isTrapActivated { get; private set; }

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space))
			ActivateTrap ();
	}

	public void ActivateTrap()
	{
		// toggle trap activation
		isTrapActivated = !isTrapActivated;

		if (trapAudioSource)
			trapAudioSource.Play ();

		openTrap.gameObject.SetActive (!isTrapActivated);
		closedTrap.gameObject.SetActive (isTrapActivated);
	}
}
