
using UnityEngine;

public class TileObject : MonoBehaviour
{
	public enum Type
	{
		Pickup,
		Enemy,
		Trap,
	}

	public Type tileType;
}