using System;
using UnityEditor;
using UnityEngine;

namespace Omniverse
{
	public static class TerrainUtils
	{
		[MenuItem("Dreambox/Terrain/Export Trees")]
		public static void ExportTrees()
		{
			var terrain = Selection.activeGameObject.GetComponent<Terrain>();

			if (terrain == null)
			{
				return;
			}

			TerrainData terrainData = terrain.terrainData;
			
			Transform treesHolder = terrain.transform.Find("Trees");
			if (treesHolder == null)
			{
				treesHolder = new GameObject("Trees").transform;
				treesHolder.SetParent(terrain.transform, false);
			}

			foreach (TreeInstance treeInstance in terrain.terrainData.treeInstances)
			{
				TreePrototype treePrototype = terrain.terrainData.treePrototypes[treeInstance.prototypeIndex];

				Vector3 position = new Vector3(treeInstance.position.x * terrainData.size.x,
					                   treeInstance.position.y * terrainData.size.y,
					                   treeInstance.position.z * terrainData.size.z) +
				                   terrain.transform.position;

				Quaternion rotation = Quaternion.Euler(0, treeInstance.rotation * Mathf.Rad2Deg, 0);

				var prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(treePrototype.prefab, treesHolder);
				prefabInstance.transform.position = position;
				prefabInstance.transform.rotation = rotation;
			}

			terrain.terrainData.treeInstances = Array.Empty<TreeInstance>();
		}
	}
}
