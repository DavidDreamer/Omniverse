using System;
using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
{

	public class PropertyObsolete
	{
		public PropertyDesc Desc { get; }

		public float RawAmount { get; private set; }

		public float Amount { get; private set; }

		public float Regeneration { get; private set; }

		public bool Vital { get; }

		public bool Invulnerable { get; private set; }

		public bool OutOf => Amount == 0;

		public List<PropertyModifier> Modifiers { get; }

		public PropertyObsolete(PropertyDesc desc)
		{
			Desc = desc;

			RawAmount = Desc.Default;
			Amount = Desc.Default;
			Regeneration = Desc.Regeneration;

			Modifiers = new List<PropertyModifier>();

			Vital = desc.Vital;
		}

		public void FixedTick(float deltaTime)
		{
			Amount = RawAmount;

			foreach (PropertyModifier modifier in Modifiers)
			{
				Modify(modifier);
			}

			Change(Regeneration * deltaTime);
		}

		public void AddModifier(PropertyModifier modifier)
		{
			Modifiers.Add(modifier);
		}

		public void RemoveModifier(PropertyModifier modifier)
		{
			Modifiers.Remove(modifier);
		}

		public void Restore()
		{
			Amount = Desc.Default;
		}

		public void Modify(PropertyModifier modifier)
		{
			Debug.Assert(!Invulnerable);

			float value = GetModificationValue();

			switch (modifier.Operation)
			{
				case Dreambox.Core.ArithmeticOperation.Addition:
					Change(value);
					break;
				case Dreambox.Core.ArithmeticOperation.Subtraction:
					Change(-value);
					break;
				case Dreambox.Core.ArithmeticOperation.Multiplication:
					break;
				case Dreambox.Core.ArithmeticOperation.Division:
					break;
				default: throw new ArgumentOutOfRangeException(modifier.Operation.ToString());
			}

			float GetModificationValue()
			{
				switch (modifier.Mode)
				{
					case PropertyModifierMode.Absolute:
						return modifier.Value;
					case PropertyModifierMode.Percentage:
						return modifier.Value * RawAmount;
					default: throw new ArgumentOutOfRangeException(modifier.Mode.ToString());
				}
			}
		}

		public void Change(float delta)
		{
			RawAmount = Mathf.Clamp(RawAmount + delta, Desc.Range.Min, Desc.Range.Max);
		}
	}
}
