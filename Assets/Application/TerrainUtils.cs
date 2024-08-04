using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

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
			var treeInstances = new TreeInstance[treesHolder.childCount];

			for (int i = 0; i < treesHolder.childCount; ++i)
			{
				Transform tree = treesHolder.GetChild(i);
				GameObject treePrefab = PrefabUtility.GetCorrespondingObjectFromSource(tree).gameObject;
				int treePrototypeIndex = terrainData.GetTreePrototypeIndex(treePrefab);
				var position = new Vector3(tree.position.x / terrainData.size.x,
					tree.position.y / terrainData.size.y, tree.position.z / terrainData.size.z);

				var treeInstance = new TreeInstance
				{
					position = position,
					widthScale = 1,
					heightScale = 1,
					prototypeIndex = treePrototypeIndex
				};

				treeInstances[i] = treeInstance;
			}

			terrainData.treeInstances = treeInstances;

			Object.DestroyImmediate(treesHolder.gameObject);

			EditorUtility.SetDirty(terrain);
		}

		private static int GetTreePrototypeIndex(this TerrainData terrainData, GameObject prefab)
		{
			for (var i = 0; i < terrainData.treePrototypes.Length; ++i)
			{
				TreePrototype treePrototype = terrainData.treePrototypes[i];
				if (treePrototype.prefab == prefab)
				{
					return i;
				}
			}

			return -1;
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
