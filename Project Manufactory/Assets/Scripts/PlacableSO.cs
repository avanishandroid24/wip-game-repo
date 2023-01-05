using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Placable", menuName = "Create New Placable")]
public class PlacableSO : ScriptableObject
{
	public enum Dir
	{
		Down,
		Right,
		Up,
		Left
	}

	public static Dir GetNextDir(Dir dir)
	{
		switch (dir)
		{
			case Dir.Down:
				return Dir.Right;
			case Dir.Right:
				return Dir.Up;
			case Dir.Up:
				return Dir.Left;
			case Dir.Left:
				return Dir.Down;
			default:
				return Dir.Down;
		}
	}

	public string nameString;
	public Transform prefab;
	public Transform visual;
	public int width;
	public int height;

	public float GetRotationAngle(Dir dir)
	{
		return (float)dir * 90f;
	}

	public Vector2Int GetRotationOffset(Dir dir)
	{
		switch (dir)
		{
			case Dir.Down:
				return new Vector2Int(0, 0);
			case Dir.Right:
				return new Vector2Int(0, width);
			case Dir.Up:
				return new Vector2Int(width, height);
			case Dir.Left:
				return new Vector2Int(height, 0);
			default:
				return new Vector2Int(0, 0);
		}
	}

	public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir)
	{
		List<Vector2Int> gridPositionList = new List<Vector2Int>();
		switch (dir)
		{
			default:
			case Dir.Down:
			case Dir.Up:
				for (int x = 0; x < width; x++)
				{
					for (int y = 0; y < height; y++)
					{
						gridPositionList.Add(offset + new Vector2Int(x, y));
					}
				}
				break;

			case Dir.Left:
			case Dir.Right:
				for (int x = 0; x < height; x++)
				{
					for (int y = 0; y < width; y++)
					{
						gridPositionList.Add(offset + new Vector2Int(x, y));
					}
				}
				break;
		}

		return gridPositionList;
	}
}