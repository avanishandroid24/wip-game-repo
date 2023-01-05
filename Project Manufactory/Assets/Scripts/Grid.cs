using UnityEngine;
using CodeMonkey.Utils;
using System;
using UnityEngine.InputSystem;

public class Grid<TGridObject>
{
	private int width;
	private int height;
	private float cellSize;
	private TGridObject[,] data;
	private Vector3 origin;

	public Grid(int x, int z, float cellSize, Vector3 origin, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
	{
		width = x;
		height = z;
		this.cellSize = cellSize;
		this.origin = origin;

		data = new TGridObject[width, height];

		for (int i = 0; i < x; i++)
		{
			for (int j = 0; j < z; j++)
			{				
				data[i, j] = createGridObject(this, i, j);
			}
		}
			
		for (int i = 0; i <= width; i++)
		{
			Vector3 start = GridToWorldPosition(i, 0);
			Vector3 end = GridToWorldPosition(i, height);
			Debug.DrawLine(start, end, Color.white, 1000f);
		}
		for (int j = 0; j <= height; j++)
		{
			Vector3 start = GridToWorldPosition(0, j);
			Vector3 end = GridToWorldPosition(width, j);
			Debug.DrawLine(start, end, Color.white, 1000f);
		}
		if (Application.isEditor)
		{
			for (int i = 0; i < x; i++)
			{
				for (int j = 0; j < z; j++)
				{
					Vector3 centerPosition = GridToWorldPosition(i + 0.5f, j + 0.5f);
					UtilsClass.CreateWorldText(data[i,j]?.ToString(), null, centerPosition, 20, Color.white, TextAnchor.MiddleCenter);
				}
			}
		}
	}

	public float GetCellSize()
	{
		return cellSize;
	}

	public Vector3 GridToWorldPosition(float x, float z)
	{
		return origin + new Vector3(x * cellSize, 0, z * cellSize);
	}

	public Vector3 GridToWorldPosition(Vector2Int gridPosition)
	{
		return origin + new Vector3(gridPosition.x * cellSize, 0, gridPosition.y * cellSize);
	}

	public Vector2Int WorldPositionToGrid(Vector3 worldPosition)
	{		
		Vector3 localPosition = worldPosition - origin;
		int x = Mathf.FloorToInt(localPosition.x / cellSize);
		int z = Mathf.FloorToInt(localPosition.z / cellSize);
		return new Vector2Int(x, z);
	}

	public void SetGridObject(int x, int z, TGridObject value)
	{
		if (x < 0 || z < 0 || x >= width || z >= height) return;
		data[x, z] = value;
	}

	public void SetGridObject(Vector3 worldPosition, TGridObject value)
	{		
		Vector2 gridCoordinates = WorldPositionToGrid(worldPosition);
		SetGridObject((int)gridCoordinates.x, (int)gridCoordinates.y, value);
	}

	public TGridObject GetGridObject(int x, int z)
	{
		if (x < 0 || z < 0 || x >= width || z >= height) return default(TGridObject);
		return data[x, z];
	}

	public TGridObject GetGridObject(Vector3 worldPosition)
	{
		Vector2 gridCoordinates = WorldPositionToGrid(worldPosition);
		int x = (int)gridCoordinates.x;
		int z = (int)gridCoordinates.y;
		if (x < 0 || z < 0 || x >= width || z >= height) return default(TGridObject);
		return data[x, z];
	}

	public TGridObject GetGridObject(Vector2Int gridPosition)
	{
		if (gridPosition.x < 0 || gridPosition.y < 0 || gridPosition.x >= width || gridPosition.y >= height) return default(TGridObject);
		return data[gridPosition.x, gridPosition.y];
	}

	public Vector2Int GetMouseGridPosition()
	{
		return WorldPositionToGrid(GetMouseWorldPosition());
	}

	private Vector3 GetMouseWorldPosition()
	{
		Vector2 mousePosition = Mouse.current.position.ReadValue();
		Ray ray = Camera.main.ScreenPointToRay(mousePosition);
		if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
		{
			if (hit.collider != null) return hit.point;
		}
		return Vector3.zero;
	}
}
