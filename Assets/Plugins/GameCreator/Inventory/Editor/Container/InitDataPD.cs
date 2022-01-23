namespace GameCreator.Inventory
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using GameCreator.Core;

    [CustomPropertyDrawer(typeof(Container.InitData))]
    public class InitDataPD : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty spItem = property.FindPropertyRelative("item");
            SerializedProperty spAmount = property.FindPropertyRelative("amount");

            Rect rectItem = Rect.zero;
            Rect rectAmount = Rect.zero;

            this.GetRects(
                position, spItem, spAmount, 
                ref rectItem, ref rectAmount
            );

            EditorGUI.PropertyField(rectItem, spItem, true);

            EditorGUI.PropertyField(rectAmount, spAmount, true);
            spAmount.intValue = Mathf.Max(0, spAmount.intValue);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty spItem = property.FindPropertyRelative("item");
            SerializedProperty spAmount = property.FindPropertyRelative("amount");

            Rect rectItem = Rect.zero;
            Rect rectAmount = Rect.zero;
            
            this.GetRects(
                Rect.zero, 
                spItem, spAmount, 
                ref rectItem, ref rectAmount
            );

            return (
                rectItem.height +
                rectAmount.height +
                (3 * EditorGUIUtility.standardVerticalSpacing)
            );
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void GetRects(Rect position, 
            SerializedProperty spItem, SerializedProperty spAmount, 
            ref Rect rectItem, ref Rect rectAmount)
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
        }
    }
}