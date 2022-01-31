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
	public class ActionLootTable : IAction
	{
        public enum Target
        {
            PlayerInventory,
            Container
        }

        public LootTable lootTable;

        public Target target = Target.PlayerInventory;
        public TargetGameObject container = new TargetGameObject(TargetGameObject.Target.Invoker);

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            if (this.lootTable == null) return true;
            LootTable.LootResult loot = this.lootTable.Get();

            if (loot.item != null && loot.amount > 0)
            {
                switch (this.target)
                {
                    case Target.PlayerInventory:
                        InventoryManager.Instance.AddItemToInventory(
                            loot.item.uuid,
                            loot.amount
                        );
                        break;

                    case Target.Container:
                        Container containerInstance = this.container.GetComponent<Container>(target);
                        if (containerInstance != null)
                        {
                            containerInstance.AddItem(
                                loot.item.uuid,
                                loot.amount
                            );
                        }
                        break;
                }
            }

            return true;
        }

        #if UNITY_EDITOR

        public static new string NAME = "Inventory/Use Loot Table";
        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Actions/";

        private const string NODE_TITLE = "Use {0}";

        private SerializedProperty spLootTable;
        private SerializedProperty spTarget;
        private SerializedProperty spContainer;

        public override string GetNodeTitle()
        {
            return string.Format(
                NODE_TITLE, 
                (this.lootTable == null ? "(none)" : this.lootTable.name)
            );
        }

        protected override void OnEnableEditorChild()
        {
            this.spLootTable = serializedObject.FindProperty("lootTable");
            this.spTarget = serializedObject.FindProperty("target");
            this.spContainer = serializedObject.FindProperty("container");
        }

        protected override void OnDisableEditorChild()
        {
            this.spLootTable = null;
            this.spTarget = null;
            this.spContainer = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.spLootTable);
            EditorGUILayout.PropertyField(this.spTarget);

            if (this.spTarget.enumValueIndex == (int)Target.Container)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(this.spContainer);
                EditorGUI.indentLevel--;
            }

            this.serializedObject.ApplyModifiedProperties();
        }

        #endif
    }
}
