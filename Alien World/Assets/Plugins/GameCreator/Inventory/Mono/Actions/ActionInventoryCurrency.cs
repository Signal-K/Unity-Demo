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
	public class ActionInventoryCurrency : IAction 
	{
		public enum CURRENCY_OPERATION
		{
			Add,
			Substract
		}

		public CURRENCY_OPERATION operation;
		public int amount = 0;

        // EXECUTABLE: -------------------------------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            switch (this.operation)
            {
                case CURRENCY_OPERATION.Add: InventoryManager.Instance.AddCurrency(this.amount); break;
                case CURRENCY_OPERATION.Substract: InventoryManager.Instance.SubstractCurrency(this.amount); break;
            }

            return true;
        }

		// +-----------------------------------------------------------------------------------------------------------+
		// | EDITOR                                                                                                    |
		// +-----------------------------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Actions/";

		public static new string NAME = "Inventory/Currency";
		private const string NODE_TITLE = "{0} {1} of currency";

		// PROPERTIES: -------------------------------------------------------------------------------------------------

		private SerializedProperty spOperation;
		private SerializedProperty spAmount;

		// INSPECTOR METHODS: ------------------------------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE, this.operation.ToString(), this.amount.ToString());
		}

		protected override void OnEnableEditorChild ()
		{
			this.spOperation = this.serializedObject.FindProperty("operation");
			this.spAmount = this.serializedObject.FindProperty("amount");
		}

		protected override void OnDisableEditorChild ()
		{
			this.spOperation = null;
			this.spAmount = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.spOperation);
			EditorGUILayout.PropertyField(this.spAmount);
			this.spAmount.intValue = Mathf.Max(0, this.spAmount.intValue);

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}