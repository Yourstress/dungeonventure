
using UnityEngine;

public class TileObject : MonoBehaviour
{
	public enum Type
	{
		Weapon,
		Pickup,
		Enemy,
		Trap,
		Environment,
	}

	public Type tileType;
}