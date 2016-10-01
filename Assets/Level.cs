using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
	[ContextMenuItem("Generate Maps", "SpawnLevel")]
	public ModularTileMap[] tileMapSegmentPrefabs;

	List<KeyValuePair<ModularTileMap, Connector>> freeConnectors = new List<KeyValuePair<ModularTileMap,Connector>>();

	private ModularTileMap lastSegment = null;
	private Connector lastExitConnector = new Connector() { type = ConnectorType.Top };

	public int spawnSegmentsOnStart = 20;

	public ModularTileMap Entrance { get; private set; }

	void Start()
	{
		for (int x = 0; x < spawnSegmentsOnStart; x++)
		{
			SpawnLevel ();
		}
	}

	ModularTileMap InstantiateCompatibleSegment(ConnectorType connector)
	{
		int rnd = Random.Range (0, tileMapSegmentPrefabs.Length); 
		connector = ReverseConnectorType (connector);
		for (int x = 0; x < tileMapSegmentPrefabs.Length; x++)
		{
			int prefabIndex = (rnd + x) % tileMapSegmentPrefabs.Length;
			if (tileMapSegmentPrefabs [prefabIndex].entranceConnector.type == connector) {
				
				ModularTileMap newSegment = Instantiate (tileMapSegmentPrefabs [prefabIndex]);
				newSegment.transform.SetParent (transform, false);
				return newSegment;
			}
		}

		throw new UnityException ("No compatible segment " + connector + " found");
	}

	void SpawnLevel()
	{
		ModularTileMap newSegment = InstantiateCompatibleSegment (lastExitConnector.type);

		// first one? put it at 0,0
		if (freeConnectors.Count == 0) {
			newSegment.transform.position = Vector3.zero;
			Entrance = newSegment;
		}
		else
		{
			// find an exit for this entrance
			KeyValuePair<ModularTileMap,Connector> exitSegment = FindExitSegment(lastExitConnector.type, true);

			// figure out the intersection point (the exit segment's connector point)
			Vector3 exitWorldPos = exitSegment.Key.tileMap.CellToWorld(exitSegment.Value.position);

			// figure out position of the new segment's entrance in local position to apply the offset
			Vector3 entranceLoclPos = newSegment.tileMap.CellToLocal(newSegment.entranceConnector.position);

			// move new segment to the exit segment offseted by this segment's entrance position
			newSegment.transform.position = exitWorldPos - entranceLoclPos;

			// connect the last segment's exit to this new segment's entrance
			lastExitConnector.connectedSegment = newSegment;

			// also connect this tile map's entrance back to the last segment's exit
			newSegment.entranceConnector.connectedSegment = lastSegment;
		}

		lastExitConnector = newSegment.exitConnector;
		lastSegment = newSegment;

		// add its connectors to the free connectors array
		freeConnectors.Add(new KeyValuePair<ModularTileMap, Connector>(newSegment, newSegment.entranceConnector));
		freeConnectors.Add(new KeyValuePair<ModularTileMap, Connector>(newSegment, newSegment.exitConnector));
	}

	private ConnectorType ReverseConnectorType(ConnectorType type)
	{
		switch (type)
		{
		case ConnectorType.Bottom:		return ConnectorType.Top;
		case ConnectorType.Top:			return ConnectorType.Bottom;
		case ConnectorType.Left:		return ConnectorType.Right;
		case ConnectorType.Right:		return ConnectorType.Left;
		default: throw new UnityException ("Incorrect connector type");
		}
	}

	KeyValuePair<ModularTileMap,Connector> FindExitSegment(ConnectorType entranceType, bool popFromList)
	{
		for (int x = 0; x < freeConnectors.Count; x++)
		{
			if (freeConnectors [x].Value.type == entranceType) {
				var conn = freeConnectors [x];

				if (popFromList)
					freeConnectors.RemoveAt (x);

				return conn;
			}
		}

		throw new UnityException ("No exit segment found.");
	}
}
