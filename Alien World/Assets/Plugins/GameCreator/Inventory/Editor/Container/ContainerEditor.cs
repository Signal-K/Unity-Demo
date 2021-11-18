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

    [CustomEditor(typeof(Container))]
    public class ContainerEditor : Editor
    {
        private const string GCTOOLBAR_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Toolbar/Container.png";
        private static readonly GUIContent GC_CONT_UI = new GUIContent("Container UI (optional)");

        // PROPERTIES: ----------------------------------------------------------------------------

        private Container container;

        private SerializedProperty spInitItems;
        private ReorderableList initItemsList;

        private SerializedProperty spSaveContainer;
        private SerializedProperty spContainerUI;

        // INIT METHODS: --------------------------------------------------------------------------

        private void OnEnable()
        {
            this.container = this.target as Container;

            this.spInitItems = this.serializedObject.FindProperty("initItems");
            this.spSaveContainer = this.serializedObject.FindProperty("saveContainer");
            this.spContainerUI = this.serializedObject.FindProperty("containerUI");

            this.initItemsList = new ReorderableList(
                this.serializedObject,
                this.spInitItems,
                true, true, true, true
            );

            this.initItemsList.drawHeaderCallback = this.InitContainer_Header;
            this.initItemsList.drawElementCallback = this.InitContainer_Paint;
            this.initItemsList.elementHeightCallback = this.InitContainer_Height;
        }

        [InitializeOnLoadMethod]
        private static void RegisterContainerToolbar()
        {
            GameCreatorToolbar.REGISTER_ITEMS.Push(new GameCreatorToolbar.Item(
                string.Format(GCTOOLBAR_ICON_PATH),
                "Create a Container",
                ContainerEditor.CreateContainer,
                100
            ));
        }

        // PAINT METHODS: -------------------------------------------------------------------------

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.Space();

            if (Application.isPlaying) this.PaintRuntime();
            else this.initItemsList.DoLayoutList();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spContainerUI, GC_CONT_UI);
            EditorGUILayout.PropertyField(this.spSaveContainer);

            GlobalEditorID.Paint(this.container);

            serializedObject.ApplyModifiedProperties();
        }

        private void PaintRuntime()
        {
            foreach (KeyValuePair<int, Container.ItemData> element in this.container.data.items)
            {
                int uuid = element.Value.uuid;
                if (!InventoryManager.Instance.itemsCatalogue.ContainsKey(uuid)) continue;

                Item item = InventoryManager.Instance.itemsCatalogue[uuid];
                string itemName = item.itemName.content;

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField(
                    itemName,
                    element.Value.amount.ToString(),
                    EditorStyles.boldLabel
                );
                EditorGUILayout.EndVertical();
            }
        }

        // INIT ITEMS METHODS: --------------------------------------------------------------------

        private void InitContainer_Header(Rect rect)
        {
            EditorGUI.LabelField(rect, "Items");
        }

        private void InitContainer_Paint(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty spProperty = this.spInitItems.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, spProperty, true);
        }

        private float InitContainer_Height(int index)
        {
            return (
                EditorGUI.GetPropertyHeight(this.spInitItems.GetArrayElementAtIndex(index)) +
                EditorGUIUtility.standardVerticalSpacing
            );
        }

        // HIERARCHY CONTEXT MENU: ----------------------------------------------------------------

        [MenuItem("GameObject/Game Creator/Inventory/Container", false, 0)]
        public static void CreateContainer()
        {
            GameObject container = CreateSceneObject.Create("Container");
            container.AddComponent<Container>();
        }
    }
}