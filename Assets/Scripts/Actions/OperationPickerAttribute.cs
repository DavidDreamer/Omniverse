using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
{
	public interface IAction<T>
	{
		void Perform(T target);
	}

	public interface IActionTargetProvider<T>
	{
		IEnumerable<T> GetTargets();
	}
	public class OperationPickerAttribute : PropertyAttribute
	{
	}
}
