using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Omniverse
{
	public class Property
	{
		public PropertyDesc Desc { get; }

		public AsyncReactiveProperty<float> RawAmount { get; }

		public AsyncReactiveProperty<float> Amount { get; }

		public AsyncReactiveProperty<float> Regeneration { get; }

		public bool Vital { get; }

		public bool Invulnerable { get; private set; }

		public bool OutOf => Amount.Value == 0;

		public List<PropertyModifier> Modifiers { get; }

		public Property(PropertyDesc desc)
		{
			Desc = desc;

			RawAmount = new AsyncReactiveProperty<float>(desc.Default);
			Amount = new AsyncReactiveProperty<float>(desc.Default);
			Regeneration = new AsyncReactiveProperty<float>(desc.Regeneration);

			Modifiers = new List<PropertyModifier>();

			Vital = desc.Vital;
		}

		public void FixedTick(float deltaTime)
		{
			Amount.Value = RawAmount.Value;

			foreach (PropertyModifier propertyModifier in Modifiers)
			{
				Amount.Value += propertyModifier.Value;
			}

			Change(Regeneration.Value * deltaTime);
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
			Amount.Value = Desc.Default;
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
						return modifier.Value * Desc.Range.Max;
					default: throw new ArgumentOutOfRangeException(modifier.Mode.ToString());
				}
			}
		}

		public void Change(float delta)
		{
			RawAmount.Value = Mathf.Clamp(RawAmount.Value + delta, Desc.Range.Min, Desc.Range.Max);
		}
	}
}
