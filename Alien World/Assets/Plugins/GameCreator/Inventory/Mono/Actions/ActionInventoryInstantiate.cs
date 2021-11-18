namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
	using GameCreator.Core.Hooks;

	#if UNITY_EDITOR
	using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ActionInventoryInstantiate : IAction 
	{
		public ItemHolder item;
        public TargetPosition target = new TargetPosition();

        // EXECUTABLE: -------------------------------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            if (this.item != null && this.item.item.prefab != null)
            {
                Vector3 position = this.target.GetPosition(target);
                Quaternion rotation = Quaternion.identity;
                Instantiate(this.item.item.prefab, position, rotation);
            }

            return true;
        }

		// +-----------------------------------------------------------------------------------------------------------+
		// | EDITOR                                                                                                    |
		// +-----------------------------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Actions/";

		public static new string NAME = "Inventory/Instantiate Item";
		private const string NODE_TITLE = "Instantiate item {0}";

		// PROPERTIES: -------------------------------------------------------------------------------------------------

		private SerializedProperty spItem;
		private SerializedProperty spTarget;

		// INSPECTOR METHODS: ------------------------------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(
				NODE_TITLE, 
				(this.item == null || this.item.item == null ? "(none)" : this.item.item.itemName.content)
			);
		}

		protected override void OnEnableEditorChild ()
		{
			this.spItem = serializedObject.FindProperty("item");
            this.spTarget = serializedObject.FindProperty("target");
		}

		protected override void OnDisableEditorChild ()
		{
			this.spItem = null;
            this.spTarget = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.spItem);
            EditorGUILayout.PropertyField(this.spTarget);

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}