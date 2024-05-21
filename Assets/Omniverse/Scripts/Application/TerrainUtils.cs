using UnityEditor;
using UnityEngine;

namespace Omniverse
{
	public static class TerrainUtils
	{
		[MenuItem("Dreambox/Terrain/Export Trees")]
		public static void ExportTrees()
		{
			var terrain = Selection.activeObject as Terrain;

			if (terrain == null)
			{
				return;
			}

			Transform treesHolder = terrain.transform.Find("Trees");
			if (treesHolder == null)
			{
				treesHolder = new GameObject("Trees").transform;
				treesHolder.SetParent(terrain.transform, false);
			}

			foreach (var VARIABLE in terrain.terrainData.treeInstances)
			{
				
			}
		}
	}
}
