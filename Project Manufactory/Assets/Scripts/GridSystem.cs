using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    private int width = 100;
    private int height = 100;
    private float cellSize = 10.0f;
	public Grid<GridObject> grid { get; private set; }

	private void Awake()
	{
		grid = new Grid<GridObject>(width, height, cellSize, new Vector3(-500, 0, -500), (Grid<GridObject> g, int x, int z) => new GridObject(g, x, z));
	}
	
}

public class GridObject
{
	private Grid<GridObject> grid;
	private int x;
	private int z;
	private PlacedObject placedObject;

	public GridObject(Grid<GridObject> grid, int x, int z)
	{
		this.grid = grid;
		this.x = x;
		this.z = z;
	}

	public void SetPlacedObject(PlacedObject placedObject)
	{
		this.placedObject = placedObject;
	}

	public PlacedObject GetPlacedObject()
	{
		return placedObject;
	}

	public bool CanBuild()
	{
		return placedObject == null;
	}

	public override string ToString()
	{
		return x + ", " + z + ", " + (placedObject == null ? "\nN/A" : "\n" + placedObject.transform.name);
	}
}



