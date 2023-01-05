using UnityEngine;

public class Machine : PlacableSO
{
	public int machineID;
}

[CreateAssetMenu(fileName = "New Input-Only", menuName = "Create New Placable/Machines/Input-Only Machine")]
public class InputOnlyMachine : Machine
{
	public Entity inputSlot;
}

[CreateAssetMenu(fileName = "New Output-Only", menuName = "Create New Placable/Machines/Output-Only Machine")]
public class OutputOnlyMachine : Machine
{
	public Entity outputSlot;
}

[CreateAssetMenu(fileName = "New Input-Output", menuName = "Create New Placable/Machines/Input-Output Machine")]
public class InputOutputMachine : Machine
{
	public Entity inputSlot;
	public Entity outputSlot;
}

