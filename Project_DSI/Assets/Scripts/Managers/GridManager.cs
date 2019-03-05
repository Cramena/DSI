using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
	public static GridManager instance;

	[Space]
	[Header("References:")]
	public GameObject cellPrefab;

	[Space]
	[Header("Parameters:")]
	public int gridWidth;
	public int gridHeight;
	public float cellSpacing;

	//NON SERIALIZED
	[System.NonSerialized] public Cell[,] grid;

	//PRIVATE PARAMETERS

	private void Awake()
	{
		#region Singleton
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
		#endregion
		grid = new Cell[gridWidth, gridHeight];
		GridCreation();
	}

	void Start()
    {
	}

	void GridCreation()
	{
		for (int x = 0; x < gridWidth; x++)
		{
			for (int y = 0; y < gridHeight; y++)
			{
				Cell newCell = Instantiate(cellPrefab, new Vector3((x - gridWidth / 2) * cellSpacing, (y - gridHeight / 2) * cellSpacing, 0), Quaternion.identity, this.transform).GetComponent<Cell>();
				newCell.InitializeCell(new Vector2Int(x, y));
				grid[x, y] = newCell;
			}
		}
	}
}
