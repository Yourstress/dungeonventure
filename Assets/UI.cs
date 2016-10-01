using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

	public static UI Shared { get; private set; }

	public GameObject mainMenuRoot;
	public GameObject killedRoot;

	public GameObject levelGO;

	public Text killedLabel;

	private GameObject levelPrefab;

	// Use this for initialization
	void Awake()
	{
		Shared = this;
		levelPrefab = Instantiate(levelGO);
	}

	public void StartGame()
	{
		levelGO.SetActive(true);

		// disable UI
		killedRoot.SetActive(false);
		mainMenuRoot.SetActive(false);
	}

	public void ShowKilledScreen(string message)
	{
		killedLabel.text = message;

		// show killed screen
		killedRoot.SetActive(true);

		// kill the level
		Destroy(levelGO);

		// create new level from prefab and enable it
		levelGO = Instantiate(levelPrefab);
	}
}
