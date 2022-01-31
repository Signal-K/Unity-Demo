namespace GameCreator.Inventory
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using GameCreator.Core;

    [CustomPropertyDrawer(typeof(Merchant.Ware))]
    public class WarePD : PropertyDrawer
    {
        private const float PADDING = 2.0f;
        private static GUIContent GC_LIMIT = new GUIContent("Limit Amount");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty spItem = property.FindPropertyRelative("item");
            SerializedProperty spLimitAmount = property.FindPropertyRelative("limitAmount");
            SerializedProperty spAmount = property.FindPropertyRelative("maxAmount");

            Rect rectItem = Rect.zero;
            Rect rectLimit = Rect.zero;
            this.GetRects(position, spItem, spAmount, ref rectItem, ref rectLimit);

            EditorGUI.PropertyField(rectItem, spItem, true);

            Rect rectLimitLabel = new Rect(
                rectLimit.x,
                rectLimit.y,
                EditorGUIUtility.labelWidth,
                EditorGUIUtility.singleLineHeight
            );

            Rect rectLimitAmount = new Rect(
                rectLimitLabel.x + rectLimitLabel.width,
                rectLimit.y,
                EditorGUIUtility.singleLineHeight + PADDING,
                EditorGUIUtility.singleLineHeight
            );

            Rect rectAmount = new Rect(
                rectLimitAmount.x + rectLimitAmount.width,
                rectLimit.y,
                rectLimit.width - (rectLimitLabel.width + rectLimitAmount.width),
                EditorGUIUtility.singleLineHeight
            );

            EditorGUI.LabelField(rectLimitLabel, GC_LIMIT);
            EditorGUI.PropertyField(rectLimitAmount, spLimitAmount, GUIContent.none);

            EditorGUI.BeginDisabledGroup(!spLimitAmount.boolValue);
            EditorGUI.PropertyField(rectAmount, spAmount, GUIContent.none);
            EditorGUI.EndDisabledGroup();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty spItem = property.FindPropertyRelative("item");
            SerializedProperty spAmount = property.FindPropertyRelative("maxAmount");

            Rect rectItem = Rect.zero;
            Rect rectLimit = Rect.zero;
            this.GetRects(Rect.zero, spItem, spAmount, ref rectItem, ref rectLimit);

            return rectLimit.y + rectLimit.height;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void GetRects(Rect position, SerializedProperty spItem, SerializedProperty spLimit, ref Rect rectItem, ref Rect rectLimit)
        {
            rectItem = new Rect(
                position.x,
                position.y + PADDING,
                position.width,
                EditorGUI.GetPropertyHeight(spItem, true)
            );

            rectLimit = new Rect(
                position.x,
                rectItem.y + rectItem.height + PADDING,
                position.width,
                EditorGUI.GetPropertyHeight(spLimit) + PADDING
            );
        }
    }
}