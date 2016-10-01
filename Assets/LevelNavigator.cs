
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
			return currentMapSegment.tileMap.CellToWorld (currentSegmentPosition);
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

	bool Move(int offsetX, int offsetY)
	{
		Vector3Int targetTilePos = currentSegmentPosition;
		targetTilePos.x += offsetX;
		targetTilePos.y += offsetY;

		// ignore this tile if it's not walkable
		Sprite targetTileSprite = currentMapSegment.tileMap.GetSprite(targetTilePos);
		if (targetTileSprite.name != "grass")
			return false;

		// move the current position there
		currentSegmentPosition = targetTilePos;

		return true;
	}

	#region ICloneable implementation
	public object Clone ()
	{
		return new LevelNavigator (currentMapSegment);
	}
	#endregion
}