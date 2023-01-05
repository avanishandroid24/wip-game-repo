using UnityEditor;
using UnityEngine;

public static class PrefabManager
{
	public static GameObject GetPrefab(string prefabName)
	{
		// Load the prefab from the assets folder
		GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/" + prefabName + ".prefab");

		// Return the prefab
		return prefab;
	}
}