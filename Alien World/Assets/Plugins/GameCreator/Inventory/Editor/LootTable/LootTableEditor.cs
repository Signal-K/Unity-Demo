using UnityEngineInternal;
namespace GameCreator.Inventory
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;
    using GameCreator.Core;
    using GameCreator.Variables;

    [CustomEditor(typeof(LootTable))]
    public class LootTableEditor : Editor
    {
        private const string SUMMARY = "Summary";

        private static readonly Color COLOR_BG = new Color(0, 0, 0, 0.5f);

        public static readonly HexColor[] COLOR_HEXS = new HexColor[]
        {
            new HexColor("#1098ad"), // Cyan
            new HexColor("#f59f00"), // Yellow
            new HexColor("#74b816"), // Lime
            new HexColor("#f03e3e"), // Red
            new HexColor("#37b24d"), // Green
            new HexColor("#d6336c"), // Pink
            new HexColor("#f76707"), // Orange
            new HexColor("#ae3ec9"), // Grape
            new HexColor("#7048e8"), // Violet
            new HexColor("#0ca678"), // Teal
            new HexColor("#4263eb"), // Indigo
            new HexColor("#1c7ed6"), // Blue
        };

        // PROPERTIES: ----------------------------------------------------------------------------

        private LootTable lootTable;
        private SerializedProperty spNoDropWeight;

        private SerializedProperty spLoot;
        private ReorderableList lootList;

        // METHODS: -------------------------------------------------------------------------------

        private void OnEnable()
        {
            this.lootTable = this.target as LootTable;

            this.spNoDropWeight = this.serializedObject.FindProperty("noDropWeight");
            this.spLoot = this.serializedObject.FindProperty("loot");

            this.lootList = new ReorderableList(
                this.serializedObject,
                this.spLoot,
                true, true, true, true
            );

            this.lootList.drawHeaderCallback = this.WaresList_Header;
            this.lootList.drawElementCallback = this.WaresList_Paint;
            this.lootList.elementHeightCallback = this.WaresList_Height;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(this.spNoDropWeight);
            this.spNoDropWeight.intValue = Mathf.Max(0, this.spNoDropWeight.intValue);

            EditorGUILayout.Space();
            this.lootList.DoLayoutList();
            EditorGUILayout.Space();

            this.PaintSummary();

            serializedObject.ApplyModifiedProperties();
        }

        // LOOT TABLE METHODS: --------------------------------------------------------------------

        private void WaresList_Header(Rect rect)
        {
            EditorGUI.LabelField(rect, "Loot");
        }

        private void WaresList_Paint(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty spProperty = this.spLoot.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, spProperty, true);
        }

        private float WaresList_Height(int index)
        {
            return (
                EditorGUI.GetPropertyHeight(this.spLoot.GetArrayElementAtIndex(index)) +
                EditorGUIUtility.standardVerticalSpacing
            );
        }

        private void PaintSummary()
        {
            EditorGUILayout.LabelField(SUMMARY, EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.Space();

            Rect summaryRectBg = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.miniButton);

            Rect summaryRect = new Rect(
                summaryRectBg.x + 1,
                summaryRectBg.y + 1,
                summaryRectBg.width - 2,
                summaryRectBg.height - 2
            );

            EditorGUI.DrawRect(summaryRectBg, COLOR_BG);
            EditorGUILayout.Space();

            float totalWeight = this.lootTable.noDropWeight;
            for (int i = 0; i < this.lootTable.loot.Length; ++i)
            {
                totalWeight += this.lootTable.loot[i].weight;
            }

            Rect rect = new Rect(summaryRect.x, summaryRect.y, 0, summaryRect.height);

            for (int i = 0; i < this.lootTable.loot.Length; ++i)
            {
                float weight = this.lootTable.loot[i].weight;
                float percent = weight / totalWeight;

                rect = new Rect(
                    rect.x + rect.width,
                    rect.y,
                    summaryRect.width * percent,
                    rect.height
                );

                int colorIndex = i % COLOR_HEXS.Length;
                Color color = COLOR_HEXS[colorIndex].GetColor();

                EditorGUI.DrawRect(rect, color);
                string itemName = this.lootTable.loot[i].item.ToString();
                if (this.lootTable.loot[i].amount > 1)
                {
                    itemName = string.Format(
                        "{0} ({1})", 
                        itemName, 
                        this.lootTable.loot[i].amount
                    );
                }

                this.PaintLegend(
                    itemName,
                    string.Format("{0:.00} %", 100f * percent),
                    color
                );
            }

            if (this.lootTable.noDropWeight > 0)
            {
                this.PaintLegend(
                    "No Drop",
                    string.Format("{0:.00} %", 100f * (this.lootTable.noDropWeight / totalWeight)),
                    COLOR_BG
                );
            }
        }

        private void PaintLegend(string label1, string label2, Color color)
        {
            Rect rectInfo = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.label);
            Rect rectColor = new Rect(
                rectInfo.x,
                rectInfo.y,
                rectInfo.height,
                rectInfo.height
            );
            Rect rectLabel = new Rect(
                rectInfo.x + rectColor.width + EditorGUIUtility.standardVerticalSpacing,
                rectInfo.y,
                rectInfo.width - (rectColor.width + EditorGUIUtility.standardVerticalSpacing),
                rectInfo.height
            );

            EditorGUI.DrawRect(
                new Rect(
                    rectColor.x + 3, rectColor.y + 3,
                    rectColor.width - 6, rectColor.height - 6
                ),
                color
            );
            EditorGUI.LabelField(
                rectLabel,
                label1,
                label2
            );
        }
    }
}