namespace GameCreator.Inventory
{
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;
    using UnityEditor.AnimatedValues;
    using GameCreator.Core;

    [CustomEditor(typeof(Merchant))]
    public class MerchantEditor : Editor
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spUuid;
        private SerializedProperty spTitle;
        private SerializedProperty spDescription;
        private SerializedProperty spMerchantUI;

        private SerializedProperty spWares;
        private ReorderableList waresList;

        private SerializedProperty spPurchasePercent;
        private SerializedProperty spSellPercent;

        // METHODS: -------------------------------------------------------------------------------

        private void OnEnable()
        {
            this.spUuid = serializedObject.FindProperty("uuid");
            if (string.IsNullOrEmpty(this.spUuid.stringValue))
            {
                this.spUuid.stringValue = System.Guid.NewGuid().ToString("N");
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
                serializedObject.Update();
            }

            this.spTitle = serializedObject.FindProperty("title");
            this.spDescription = serializedObject.FindProperty("description");
            this.spMerchantUI = serializedObject.FindProperty("merchantUI");

            SerializedProperty spWarehouse = serializedObject.FindProperty("warehouse");
            this.spWares = spWarehouse.FindPropertyRelative("wares");

            this.waresList = new ReorderableList(
                this.spWares.serializedObject,
                this.spWares,
                true, true, true, true
            );

            this.waresList.drawHeaderCallback = this.WaresList_Header;
            this.waresList.drawElementCallback = this.WaresList_Paint;
            this.waresList.elementHeightCallback = this.WaresList_Height;

            this.spPurchasePercent = serializedObject.FindProperty("purchasePercent");
            this.spSellPercent = serializedObject.FindProperty("sellPercent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(this.spTitle);
            EditorGUILayout.PropertyField(this.spDescription);

            EditorGUILayout.Space();
            this.waresList.DoLayoutList();
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(this.spMerchantUI);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(this.spUuid);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spPurchasePercent);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spSellPercent);

            serializedObject.ApplyModifiedProperties();
        }

        // WARE LIST METHODS: ---------------------------------------------------------------------

        private void WaresList_Header(Rect rect)
        {
            EditorGUI.LabelField(rect, "Wares");
        }

        private void WaresList_Paint(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty spProperty = this.spWares.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, spProperty);
        }

        private float WaresList_Height(int index)
        {
            return (
                EditorGUI.GetPropertyHeight(this.spWares.GetArrayElementAtIndex(index)) +
                EditorGUIUtility.standardVerticalSpacing
            );
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        public string GetHeaderName()
        {
            return this.spTitle.stringValue;
        }
    }
}