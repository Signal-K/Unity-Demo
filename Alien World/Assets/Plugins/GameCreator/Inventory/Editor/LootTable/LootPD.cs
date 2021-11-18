namespace GameCreator.Inventory
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using GameCreator.Core;

    [CustomPropertyDrawer(typeof(LootTable.Loot))]
    public class LootPD : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty spItem = property.FindPropertyRelative("item");
            SerializedProperty spAmount = property.FindPropertyRelative("amount");
            SerializedProperty spWeight = property.FindPropertyRelative("weight");

            Rect rectItem = Rect.zero;
            Rect rectAmount = Rect.zero;
            Rect rectWeight = Rect.zero;
            this.GetRects(
                position, spItem, spAmount, spWeight, 
                ref rectItem, ref rectAmount, ref rectWeight
            );

            EditorGUI.PropertyField(rectItem, spItem, true);

            EditorGUI.PropertyField(rectAmount, spAmount, true);
            spAmount.intValue = Mathf.Max(0, spAmount.intValue);

            EditorGUI.PropertyField(rectWeight, spWeight, true);
            spWeight.intValue = Mathf.Max(0, spWeight.intValue);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty spItem = property.FindPropertyRelative("item");
            SerializedProperty spAmount = property.FindPropertyRelative("amount");
            SerializedProperty spWeight = property.FindPropertyRelative("weight");

            Rect rectItem = Rect.zero;
            Rect rectAmount = Rect.zero;
            Rect rectWeight = Rect.zero;
            this.GetRects(
                Rect.zero, 
                spItem, spAmount, spWeight, 
                ref rectItem, ref rectAmount, ref rectWeight
            );

            return (
                rectItem.height +
                rectAmount.height +
                rectWeight.height +
                (4 * EditorGUIUtility.standardVerticalSpacing)
            );
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void GetRects(Rect position, 
            SerializedProperty spItem, SerializedProperty spAmount, SerializedProperty spLimit, 
            ref Rect rectItem, ref Rect rectAmount, ref Rect rectWeight)
        {
            rectItem = new Rect(
                position.x,
                position.y + EditorGUIUtility.standardVerticalSpacing,
                position.width,
                EditorGUI.GetPropertyHeight(spItem, true)
            );

            rectAmount = new Rect(
                rectItem.x,
                rectItem.y + rectItem.height + EditorGUIUtility.standardVerticalSpacing,
                position.width,
                EditorGUI.GetPropertyHeight(spAmount, true)
            );

            rectWeight = new Rect(
                position.x,
                rectAmount.y + rectAmount.height + EditorGUIUtility.standardVerticalSpacing,
                position.width,
                EditorGUI.GetPropertyHeight(spLimit)
            );
        }
    }
}