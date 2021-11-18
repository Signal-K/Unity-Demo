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
	public class ActionEquip : IAction
	{
        public TargetGameObject character = new TargetGameObject(TargetGameObject.Target.Player);
        public ItemHolder item = new ItemHolder();

        [InventorySingleItemType]
        public int type = 0;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            GameObject targetObject = this.character.GetGameObject(target);
            if (target == null)
            {
                Debug.LogError("No target found in Action: Equip");
                return true;
            }

            InventoryManager.Instance.Equip(
                targetObject, 
                this.item.item.uuid, 
                this.type
            );

            return true;
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

        #if UNITY_EDITOR

        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Actions/";

        public static new string NAME = "Inventory/Equip Item";
        private const string NODE_TITLE = "Equip {0} on {1}";

		// PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty spCharacter;
        private SerializedProperty spItem;
        private SerializedProperty spType;

		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(
                NODE_TITLE, 
                (this.item.item == null ? "nothing" : this.item.item.itemName.content),
                this.character
            );
		}

		protected override void OnEnableEditorChild ()
		{
            this.spCharacter = this.serializedObject.FindProperty("character");
            this.spItem = this.serializedObject.FindProperty("item");
            this.spType = this.serializedObject.FindProperty("type");
		}

		protected override void OnDisableEditorChild ()
		{
            this.spCharacter = null;
            this.spItem = null;
            this.spType = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.spCharacter);
            EditorGUILayout.PropertyField(this.spItem);
            EditorGUILayout.PropertyField(this.spType);

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
