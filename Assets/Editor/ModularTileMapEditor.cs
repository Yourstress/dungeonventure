
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ModularTileMap))]
public class ModularTileMapEditor : Editor
{
	ModularTileMap tileMap { get { return (ModularTileMap)target; } }

	public override void OnInspectorGUI ()
	{
		DrawConnectorGUI ("Entrance", tileMap.entranceConnector);
		DrawConnectorGUI ("Exit", tileMap.exitConnector);

		GUILayout.Label ("Objects", EditorStyles.boldLabel);
		for (int x = 0; x < tileMap.tileObjects.Count; x++)
		{
			if (tileMap.tileObjects [x] == null) {
				tileMap.tileObjects.RemoveAt (x--);
				continue;
			}
			tileMap.tileObjects [x] = (TileObject)EditorGUILayout.ObjectField (tileMap.tileObjects [x], typeof(TileObject), true);
		}
		TileObject newTileObj = (TileObject)EditorGUILayout.ObjectField ("New Tile Object", null, typeof(TileObject), true);
		if (newTileObj != null)
		{
			tileMap.tileObjects.Add (newTileObj);
		}
	}

	void OnSceneGUI()
	{
		if (tileMap.tileMap == null)
			tileMap.tileMap = tileMap.GetComponent<TileMap> ();
		
		DrawConnectorGizmo ("Entrance", tileMap.entranceConnector);
		DrawConnectorGizmo ("Exit", tileMap.exitConnector);

		Vector3 halfCellSize = tileMap.tileMap.cellSize / 2f;
		halfCellSize.z = 0;

		for (int x = 0; x < tileMap.tileObjects.Count; x++)
		{
			TileObject tobj = tileMap.tileObjects [x];
			Vector3Int objCellPos = tileMap.tileMap.WorldToCell (tobj.transform.position);
			tobj.transform.position = tileMap.tileMap.CellToWorld (objCellPos) + halfCellSize;

			// draw move handle
			Vector3 newWorld = Handles.FreeMoveHandle (tobj.transform.position, Quaternion.identity, halfCellSize.x, Vector3.zero, Handles.CircleCap);

			// convert dragged world pos to cell pos
			objCellPos = tileMap.tileMap.WorldToCell(newWorld);

			// then back to world position
			newWorld = tileMap.tileMap.CellToWorld(objCellPos);

			tobj.transform.position = newWorld + halfCellSize;
		}
	}

	void DrawConnectorGizmo(string name, Connector connector)
	{
		Vector3 halfCellSize = tileMap.tileMap.cellSize/2f;

		if (connector.type != ConnectorType.None)
		{
			Vector3 world = tileMap.tileMap.CellToWorld (connector.position) + halfCellSize;
			Vector3 newWorld = Handles.FreeMoveHandle (world, Quaternion.identity, halfCellSize.x, Vector3.zero, Handles.CircleCap);

			// draw label
			world.y += halfCellSize.y;
			Handles.Label (world, name);

			connector.position = tileMap.tileMap.WorldToCell (newWorld);
		}
	}

	void DrawConnectorGUI(string connectorName, Connector connector)
	{
		GUILayout.Label (connectorName);
		connector.type = (ConnectorType)EditorGUILayout.EnumPopup (connector.type);
		if (connector.type != ConnectorType.None)
		{
			connector.position = EditorGUILayout.Vector3IntField ("Position", connector.position);
		}
	}
}