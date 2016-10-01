
using System;
using UnityEngine;

public class LevelNavigator : ICloneable
{
	public ModularTileMap currentMapSegment { get; private set; }
	public Vector3Int currentSegmentPosition;

	public Vector3 WorldPosition
	{
		get
		{
			// convert the cell position on the current map segment to world position
			Vector3 cellSize = (currentMapSegment.tileMap.cellSize*.5f);
			cellSize.z = 0;
			return currentMapSegment.tileMap.CellToWorld (currentSegmentPosition) + cellSize;
		}
	}

	public LevelNavigator(ModularTileMap mtm)
	{
		// initialize navigator at the segment's entrance
		currentMapSegment = mtm;
		currentSegmentPosition = mtm.entranceConnector.position;
	}

	public bool MoveLeft()
	{
		return Move (-1, 0);
	}

	public bool MoveRight()
	{
		return Move (1, 0);
	}

	public bool MoveUp()
	{
		return Move (0, 1);
	}

	public bool MoveDown()
	{
		return Move (0, -1);
	}

	public void MoveToWorld(Vector3 worldPos)
	{
		currentSegmentPosition = currentMapSegment.tileMap.WorldToCell(worldPos);
	}

	public bool Move(int offsetX, int offsetY)
	{
		Vector3Int targetTilePos = currentSegmentPosition;
		targetTilePos.x += offsetX;
		targetTilePos.y += offsetY;

		// ignore this tile if it's not walkable
		Sprite targetTileSprite = currentMapSegment.tileMap.GetSprite(targetTilePos);
		if (targetTileSprite == null || targetTileSprite.name != "grass")
			return false;

		// move the current position there
		currentSegmentPosition = targetTilePos;

		// if player crosses the segment's exit connector, change the current segment ref to its connected segment
		if (currentSegmentPosition == currentMapSegment.exitConnector.position)
			TeleportToSegment(currentMapSegment.exitConnector.connectedSegment);
		else if (currentSegmentPosition == currentMapSegment.entranceConnector.position && currentMapSegment.entranceConnector.connectedSegment != null)
			TeleportToSegment(currentMapSegment.entranceConnector.connectedSegment);

		return true;
	}

	private void TeleportToSegment(ModularTileMap segment)
	{
		Vector3 prevWorldPosition = WorldPosition;

		currentMapSegment = segment;
		currentSegmentPosition = currentMapSegment.tileMap.WorldToCell(prevWorldPosition);
	}

	#region ICloneable implementation
	public object Clone ()
	{
		return new LevelNavigator (currentMapSegment);
	}
	#endregion
}