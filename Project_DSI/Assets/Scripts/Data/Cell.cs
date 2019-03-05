using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
	public Vector2Int position;
	[System.NonSerialized] public Ball containedObject;

	public bool obstacle;

	public void InitializeCell(Vector2Int _position)
	{
		position = _position;
	}

}
