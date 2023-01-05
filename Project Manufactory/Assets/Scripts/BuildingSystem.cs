using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingSystem : MonoBehaviour
{
	private Grid<GridObject> grid;
	[SerializeField] private PlacableSO currentQueuedObject;
	[SerializeField] private PlacedObject debugQueuedObject;
	private bool isMouseOverUI;
	private PlacableSO.Dir dir;

	private void Start()
	{
		grid = GetComponent<GridSystem>().grid;
	}

	private void Update()
	{
		isMouseOverUI = EventSystem.current.IsPointerOverGameObject();
	}

	public void OnPlace(InputAction.CallbackContext context)
	{
		if (context.performed && !isMouseOverUI) PlaceObject();
	}

	public void OnBreak(InputAction.CallbackContext context)
	{
		if (context.performed && !isMouseOverUI) BreakObject();
	}

	public void OnRotate(InputAction.CallbackContext context)
	{
		if (context.performed) dir = PlacableSO.GetNextDir(dir);
	}

	public void SetCurrentQueuedObject(PlacableSO placable)
	{
		currentQueuedObject = placable;
	}

	public void ClearCurrentQueuedObject()
	{
		currentQueuedObject = null;
	}

	private void PlaceObject()
	{
		Vector2Int originGridPosition = grid.GetMouseGridPosition();
		List<Vector2Int> gridPositionList = currentQueuedObject.GetGridPositionList(originGridPosition, dir);
		
		bool canBuild = true;
		foreach (Vector2Int position in gridPositionList)
		{
			if (!grid.GetGridObject(position.x, position.y).CanBuild())
			{
				canBuild = false;
				break;
			}
		}

		if(canBuild) 
		{
			Vector2Int rotationOffset = currentQueuedObject.GetRotationOffset(dir);
			Vector3 offset = new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
			Vector3 initWorldPosition = grid.GridToWorldPosition(originGridPosition) + offset;
			PlacedObject placedObject = PlacedObject.Create(initWorldPosition, originGridPosition, dir, currentQueuedObject);

			foreach (Vector2Int gridPosition in gridPositionList)
			{
				grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
			}
		}
	}

	private void BreakObject()
	{
		Vector2Int mousePosition = grid.GetMouseGridPosition();
		PlacedObject placedObject = grid.GetGridObject(mousePosition).GetPlacedObject();
		if(placedObject != null)
		{
			List<Vector2Int> occupiedGridPositions = placedObject.GetGridPositionList();
			placedObject.DestroySelf();
			for(int i = 0; i < occupiedGridPositions.Count; i++)
			{
				grid.GetGridObject(occupiedGridPositions[i]).SetPlacedObject(null);
			}
		}
	}

}


