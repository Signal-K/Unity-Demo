namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
    using GameCreator.Characters;

	#if UNITY_EDITOR
	using UnityEditor;
	#endif

	[AddComponentMenu("")]
    public class ActionUnequip : IAction
	{
        public enum Operation
        {
            Type,
            Item
        }

        public TargetGameObject character = new TargetGameObject(TargetGameObject.Target.Player);

        public Operation unequip = Operation.Type;
        [InventoryMultiItemType] public int types = 0;
        public ItemHolder item = new ItemHolder();

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            GameObject targetObject = this.character.GetGameObject(target);
            if (target == null)
            {
                Debug.LogError("No target found in Action: Unequip");
                return true;
            }

            switch (this.unequip)
            {
                case Operation.Item:
                    if (this.item.item == null)
                    {
                        Debug.LogError("Item not defined in Action: Unequip");
                        return true;
                    }

                    InventoryManager.Instance.Unequip(
                        targetObject,
                        this.item.item.uuid
                    );
                    break;

                case Operation.Type:
                    InventoryManager.Instance.UnequipTypes(
                        targetObject,
                        this.types
                    );
                    break;
            }

            return true;
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

        #if UNITY_EDITOR

        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Actions/";

        public static new string NAME = "Inventory/Unequip Item";
        private const string NODE_TITLE = "Unequip {0} from {1}";

		// PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty spCharacter;
        private SerializedProperty spUnequip;
        private SerializedProperty spItem;
        private SerializedProperty spTypes;

		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(
                NODE_TITLE, 
                this.unequip,
                this.character
            );
		}

		protected override void OnEnableEditorChild ()
		{
            this.spCharacter = this.serializedObject.FindProperty("character");
            this.spUnequip = this.serializedObject.FindProperty("unequip");
            this.spItem = this.serializedObject.FindProperty("item");
            this.spTypes = this.serializedObject.FindProperty("types");
		}

		protected override void OnDisableEditorChild ()
		{
            this.spCharacter = null;
            this.spUnequip = null;
            this.spItem = null;
            this.spTypes = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.spCharacter);
            EditorGUILayout.PropertyField(this.spUnequip);

            EditorGUI.indentLevel++;
            switch (this.spUnequip.intValue)
            {
                case (int)Operation.Item:
                    EditorGUILayout.PropertyField(this.spItem);
                    break;

                case (int)Operation.Type:
                    EditorGUILayout.PropertyField(this.spTypes);
                    break;
            }
            EditorGUI.indentLevel--;

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
