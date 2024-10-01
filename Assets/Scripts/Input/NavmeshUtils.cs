using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Omniverse.Input
{
	public static class NavmeshUtils
	{
		public static Vector3 GetRandomPosition(Vector3 source, float radius, int areaMask)
		{
			Vector3 direction = Random.insideUnitSphere * radius;
			Vector3 randomPosition = source + direction;

			if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, float.MaxValue, areaMask))
			{
				return hit.position;
			}

			throw new InvalidOperationException("Position not found");
		}

		public static bool GetNavMeshPositionFromCursor(Ray ray, out Vector3 position)
		{
			position = Vector3.zero;

			if (!Physics.Raycast(ray, out RaycastHit rayCastHit, float.MaxValue))
			{
				return false;
			}

			if (!NavMesh.SamplePosition(rayCastHit.point, out NavMeshHit navMeshHit, float.MaxValue, 1))
			{
				return false;
			}

			position = navMeshHit.position;

			return true;
		}
	}
}
