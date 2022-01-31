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
    public class ConditionHasEquipped : ICondition
	{
        public enum Operation
        {
            Type,
            Item
        }

        public TargetGameObject character = new TargetGameObject(TargetGameObject.Target.Player);
        public Operation hasEquipped = Operation.Type;
		public ItemHolder item;

        [InventoryMultiItemType]
        public int types = ~0;

		// EXECUTABLE: -------------------------------------------------------------------------------------------------

        public override bool Check(GameObject target)
		{
            GameObject charTarget = this.character.GetGameObject(target);
            if (charTarget != null)
            {
                switch (this.hasEquipped)
                {
                    case Operation.Type:
                        return InventoryManager.Instance.HasEquipedTypes(
                            charTarget, 
                            this.types
                        );

                    case Operation.Item:
                        return InventoryManager.Instance.HasEquiped(
                            charTarget,
                            this.item.item.uuid
                        ) > 0;
                }
            }

			return false;
		}

		// +-----------------------------------------------------------------------------------------------------------+
		// | EDITOR                                                                                                    |
		// +-----------------------------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Conditions/";

		public static new string NAME = "Inventory/Has Equipped";
        private const string NODE_TITLE = "Has {0} equipped {1}";

		// PROPERTIES: -------------------------------------------------------------------------------------------------

		private SerializedProperty spCharacter;
		private SerializedProperty spHasEquipped;
        private SerializedProperty spItem;
        private SerializedProperty spTypes;

		// INSPECTOR METHODS: ------------------------------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(
				NODE_TITLE, 
                this.character,
                this.hasEquipped
			);
		}

		protected override void OnEnableEditorChild ()
		{
            this.spCharacter = this.serializedObject.FindProperty("character");
            this.spHasEquipped = this.serializedObject.FindProperty("hasEquipped");
            this.spItem = this.serializedObject.FindProperty("item");
            this.spTypes = this.serializedObject.FindProperty("types");
		}

		protected override void OnDisableEditorChild ()
		{
            this.spCharacter = null;
            this.spHasEquipped = null;
			this.spItem = null;
            this.spTypes = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.spCharacter);
            EditorGUILayout.PropertyField(this.spHasEquipped);

            switch (this.spHasEquipped.intValue)
            {
                case (int)Operation.Type: EditorGUILayout.PropertyField(this.spTypes); break;
                case (int)Operation.Item: EditorGUILayout.PropertyField(this.spItem);  break;
            }

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}