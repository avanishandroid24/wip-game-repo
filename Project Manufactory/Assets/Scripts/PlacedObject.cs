using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
	private Vector2Int origin;
	private PlacableSO placableSO;
	private PlacableSO.Dir dir;

	public static PlacedObject Create(Vector3 worldPosition, Vector2Int origin, PlacableSO.Dir dir, PlacableSO placableSO)
	{
		Transform placedObjectTransform = Instantiate(placableSO.prefab, worldPosition, Quaternion.Euler(0, placableSO.GetRotationAngle(dir), 0));

		PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();

		placedObject.origin = origin;
		placedObject.dir = dir;
		placedObject.placableSO = placableSO;

		return placedObject;
	}

	public List<Vector2Int> GetGridPositionList()
	{
		return placableSO.GetGridPositionList(origin, dir);
	}

	public void DestroySelf()
	{
		Destroy(gameObject);
	}
}

