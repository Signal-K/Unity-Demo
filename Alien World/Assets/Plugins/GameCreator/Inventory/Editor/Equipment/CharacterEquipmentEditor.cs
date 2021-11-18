namespace GameCreator.Inventory
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using GameCreator.Core;

    [CustomEditor(typeof(CharacterEquipment))]
    public class CharacterEquipmentEditor : Editor
    {
        private static DatabaseInventory DATABASE_INVENTORY;
        private CharacterEquipment equipment;

        private SerializedProperty spSaveEquipment;

        // INITIALIZERS: --------------------------------------------------------------------------

        private void OnEnable()
        {
            if (target == null || serializedObject == null) return;
            if (DATABASE_INVENTORY == null) DATABASE_INVENTORY = DatabaseInventory.Load();
            this.equipment = (CharacterEquipment)this.target;

            this.spSaveEquipment = serializedObject.FindProperty("saveEquipment");
        }

        public override bool RequiresConstantRepaint()
        {
            return Application.isPlaying;
        }

        // PAINT METHOD: --------------------------------------------------------------------------

        public override void OnInspectorGUI()
        {
            if (target == null || serializedObject == null) return;
            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Equipment information", MessageType.Info);
            }
            else
            {
                string[] typeNames = DATABASE_INVENTORY.GetItemTypesNames();
                for (int i = 0; i < this.equipment.equipment.items.Length; ++i)
                {
                    if (this.equipment.equipment.items[i].isEquipped)
                    {
                        int uuid = this.equipment.equipment.items[i].itemID;
                        string item = InventoryManager.Instance.itemsCatalogue[uuid].itemName.content;
                        EditorGUILayout.LabelField(typeNames[i], item);
                    }
                }
            }

            this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.spSaveEquipment);

            this.serializedObject.ApplyModifiedProperties();

            if (this.PaintGlobalID())
            {
                EditorGUILayout.Space();
                GlobalEditorID.Paint(this.equipment);
            }
        }

        protected virtual bool PaintGlobalID()
        {
            return true;
        }
    }
}