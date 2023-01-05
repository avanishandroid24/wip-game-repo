using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Entity : ScriptableObject
{
	public int entityID;
	public enum EntityType
	{
		None,
		Item,
		Unit
	}

	public virtual EntityType IsOfType()
	{
		return EntityType.None;
	}
}
[CreateAssetMenu(fileName = "New Item", menuName = "Create New Entity/Item")]
public class Item : Entity
{
	public enum ItemType
	{
		None,
		Wood,
		Stone,
		Metal
	}

	public ItemType type;
	public GameObject prefab;

	public override EntityType IsOfType()
	{
		return EntityType.Item;
	}
}

[CreateAssetMenu(fileName = "New Unit", menuName = "Create New Entity/Unit")]
public class Unit : Entity
{
	public enum UnitType
	{
		Electricity,
		Gas,
		Fuel
	}

	public UnitType type;

	public override EntityType IsOfType()
	{
		return EntityType.Unit;
	}
}


public class Slot
{
	public Entity.EntityType Type { get; }
	public Entity Entity { get; set; }

	public Slot(Entity.EntityType type)
	{
		Type = type;
	}

	public void SetEntity(Entity entity)
	{
		if (entity.IsOfType() == Type)
		{
			Entity = entity;
			OnEntitySet?.Invoke(entity);
		}
	}

	public void ClearEntity()
	{
		Entity = null;
	}

	public delegate void EntitySetEvent(Entity entity);
	public event EntitySetEvent OnEntitySet;
}


