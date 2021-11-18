using System;
using GameCreator.Variables;

namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;

	#if UNITY_EDITOR
	using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ConditionTotalWeight : ICondition
	{
		public enum Comparison
		{
			LessThan,
			Equal,
			GreaterThan
		}

		public Comparison comparison = Comparison.LessThan;
		public NumberProperty value = new NumberProperty(10);

		// EXECUTABLE: -------------------------------------------------------------------------------------------------

		public override bool Check(GameObject target)
		{
			float weight = InventoryManager.Instance.GetCurrentWeight();
			float comparisonValue = this.value.GetValue(target);
			
			switch (this.comparison)
			{
				case Comparison.LessThan:
					return weight < comparisonValue;
				
				case Comparison.Equal:
					return Math.Abs(weight - comparisonValue) < 0.01f;

				case Comparison.GreaterThan:
					return weight > comparisonValue;
			}

			return false;
		}

		// +-----------------------------------------------------------------------------------------------------------+
		// | EDITOR                                                                                                    |
		// +-----------------------------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Conditions/";

		public new static string NAME = "Inventory/Compare Inventory Weight";
		private const string NODE_TITLE = "Inventory weight {0} {1}";

		// INSPECTOR METHODS: ------------------------------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(
				NODE_TITLE, 
				this.comparison.ToString(),
				this.value
			);
		}

		#endif
	}
}