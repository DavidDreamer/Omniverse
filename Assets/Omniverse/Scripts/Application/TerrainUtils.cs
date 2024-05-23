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

			Transform treesHolder = FindTreesHolder(terrain);

			foreach (TreeInstance treeInstance in terrain.terrainData.treeInstances)
			{
				TreePrototype treePrototype = terrain.terrainData.treePrototypes[treeInstance.prototypeIndex];

				Vector3 position = new Vector3(treeInstance.position.x * terrainData.size.x,
					                   treeInstance.position.y * terrainData.size.y,
					                   treeInstance.position.z * terrainData.size.z) +
				                   terrain.transform.position;

				Debug.Log(treeInstance.rotation);
				
				Quaternion rotation = Quaternion.Euler(0, 0, 0);

				var prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(treePrototype.prefab, treesHolder);
				prefabInstance.transform.position = position;
				prefabInstance.transform.rotation = rotation;
			}

			terrain.terrainData.treeInstances = Array.Empty<TreeInstance>();

			EditorUtility.SetDirty(terrain);
		}
		
		[MenuItem("Dreambox/Terrain/Import Trees")]
		public static void ImportTrees()
		{
			var terrain = Selection.activeGameObject.GetComponent<Terrain>();

			if (terrain == null)
			{
				return;
			}

			TerrainData terrainData = terrain.terrainData;
			
			Transform treesHolder = FindTreesHolder(terrain);

			var treeInstances = new TreeInstance[treesHolder.transform.childCount];

			terrain.terrainData.treeInstances = treeInstances;

			EditorUtility.SetDirty(terrain);
		}

		private static Transform FindTreesHolder(Terrain terrain)
		{
			const string name = "Trees";
			
			Transform holder = terrain.transform.Find(name);
			if (holder == null)
			{
				holder = new GameObject(name).transform;
				holder.SetParent(terrain.transform, false);
			}

			return holder;
		}
	}
}
