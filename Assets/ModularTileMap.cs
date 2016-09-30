using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public enum ConnectorType
{
	None = -1,
	Top = 0,
	Bottom = 1,
	Left = 2,
	Right = 3,
}

[Serializable]
public class Connector
{
	public ConnectorType type;
	public Vector3Int position;
}

public class ModularTileMap : MonoBehaviour {

	public ITileMap map { get; private set; }
	public TileMap tileMap;

	public Connector entranceConnector;
	public Connector exitConnector;

	public List<TileObject> tileObjects = new List<TileObject> ();

	// Use this for initialization
	void Start () {
		map = tileMap;
	}
}
