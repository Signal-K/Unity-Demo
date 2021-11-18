namespace GameCreator.Inventory
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(InventoryMultiItemTypeAttribute))]
    public class InventoryMultiItemTypeAttributePD : PropertyDrawer
    {
        private static DatabaseInventory INVENTORY;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (INVENTORY == null)
            {
                INVENTORY = DatabaseInventory.Load();
            }

            property.intValue = EditorGUI.MaskField(
                position,
                label,
                property.intValue,
                INVENTORY.GetItemTypesIDs()
            );
        }
    }
}