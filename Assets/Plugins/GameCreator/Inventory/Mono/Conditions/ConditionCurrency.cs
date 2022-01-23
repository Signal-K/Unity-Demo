namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
	using GameCreator.Variables;

	#if UNITY_EDITOR
	using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ConditionCurrency : ICondition
	{
		public NumberProperty currencyAmount = new NumberProperty(100f);

		// EXECUTABLE: -------------------------------------------------------------------------------------------------

		public override bool Check(GameObject target)
		{
			int currentCurrency = InventoryManager.Instance.GetCurrency();
			return this.currencyAmount.GetInt(target) <= currentCurrency;
		}

		// +-----------------------------------------------------------------------------------------------------------+
		// | EDITOR                                                                                                    |
		// +-----------------------------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Conditions/";

		public static new string NAME = "Inventory/Enough Currency";
		private const string NODE_TITLE = "Player has at least {0} of currency";

		// PROPERTIES: -------------------------------------------------------------------------------------------------

		private SerializedProperty spCurrencyAmount;

		// INSPECTOR METHODS: ------------------------------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE, this.currencyAmount);
		}

		protected override void OnEnableEditorChild ()
		{
			this.spCurrencyAmount = this.serializedObject.FindProperty("currencyAmount");
		}

		protected override void OnDisableEditorChild ()
		{
			this.spCurrencyAmount = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.spCurrencyAmount);

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
