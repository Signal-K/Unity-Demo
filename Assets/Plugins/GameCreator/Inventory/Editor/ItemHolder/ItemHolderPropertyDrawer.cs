namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using GameCreator.Core;

	[CustomPropertyDrawer(typeof(ItemHolder), true)]
	public class ItemHolderPropertyDrawer : PropertyDrawer 
	{
        public const string PROP_ITEM = "item";

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
            SerializedProperty spItem = property.FindPropertyRelative(PROP_ITEM);

			Rect activatorRect = EditorGUI.PrefixLabel(position, label);

			string itemName = "(none)";
			if (spItem.objectReferenceValue != null) 
			{
				itemName = ((Item)spItem.objectReferenceValue).itemName.content;
				if (string.IsNullOrEmpty(itemName)) 
				{
					itemName = "No-name";
				}
			}

			GUIContent variableContent = new GUIContent(itemName);
			if (EditorGUI.DropdownButton(activatorRect, variableContent, FocusType.Keyboard))
			{
				PopupWindow.Show(
                    activatorRect, 
                    new ItemHolderPropertyDrawerWindow(activatorRect, property)
                );
			}
		}
	}
}