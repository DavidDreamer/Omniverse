using System;

namespace Omniverse.Abilities
{
	public interface ITargetDesc
	{
	}

	public interface ITarget
	{
	}

	public static class ITargetDescUtils
	{
		public static ITarget Build(this ITargetDesc desc)
		{
			switch (desc)
			{
				case null:
					return null;
				case EntityTargetDesc entityTargetDesc:
					return new EntityTarget(entityTargetDesc);
				case PointTargetDesc pointTargetDesc:
					return new PointTarget(pointTargetDesc);
				default:
					throw new ArgumentOutOfRangeException(nameof(desc));
			}
		}
	}
}
