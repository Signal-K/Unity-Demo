namespace GameCreator.Inventory
{
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using UnityEditor.AnimatedValues;
	using GameCreator.Core;

	[CustomEditor(typeof(Item))]
	public class ItemEditor : Editor 
	{
        private const float ANIM_BOOL_SPEED = 3.0f;
        private const float BTN_HEIGHT = 24f;

		private const string PATH_PREFAB_CONSUME = "Assets/Plugins/GameCreatorData/Inventory/Prefabs/Consumables/";
		private const string NAME_PREFAB_CONSUME = "consume.prefab";

        private const string PATH_PREFAB_EQUIP_COND = "Assets/Plugins/GameCreatorData/Inventory/Prefabs/Conditions/";
        private const string NAME_PREFAB_EQUIP_COND = "equip.prefab";

        private const string PATH_PREFAB_EQUP = "Assets/Plugins/GameCreatorData/Inventory/Prefabs/Equip/";
        private const string NAME_PREFAB_EQUIP = "equip.prefab";
        private const string NAME_PREFAB_UNEQUIP = "unequip.prefab";

        private static readonly GUIContent[] EQUIPABLE_OPTIONS = new GUIContent[]
        {
            new GUIContent("Conditions"),
            new GUIContent("On Equip"),
            new GUIContent("On Unequip")
        };

        private static readonly string[] EQUIP_DESC = new string[]
        {
            "Requirements that must be met in order to equip this item",
            "Actions executed when equipping this item",
            "Actions executed when un-equipping this item",
        };

		private const string PROP_UUID = "uuid";
		private const string PROP_NAME = "itemName";
		private const string PROP_DESCRIPTION = "itemDescription";
        private const string PROP_COLOR = "itemColor";
		private const string PROP_SPRITE = "sprite";
		private const string PROP_PREFAB = "prefab";
		private const string PROP_PRICE = "price";
        private const string PROP_CANBESOLD = "canBeSold";
        private const string PROP_MAXSTACK = "maxStack";
        private const string PROP_WEIGHT = "weight";
        private const string PROP_ITEMTYPES = "itemTypes";
		
        private const string PROP_ONCLICK = "onClick";
        private const string PROP_CONSUMEITEM = "consumeItem";
        private const string PROP_ACTIONONCLICK = "actionsOnClick";

        private const string PROP_EQUIPABLE = "equipable";
        private const string PROP_FILLALLTYPES = "fillAllTypes";
        private const string PROP_CONDITIONSEQUIP = "conditionsEquip";
        private const string PROP_ACTIONSEQUIP = "actionsOnEquip";
        private const string PROP_ACTIONSUNEQUIP = "actionsOnUnequip";

        public class ItemReturnOperation
        {
            public bool removeIndex = false;
            public bool duplicateIndex = false;
        }

        // PROPERTIES: ----------------------------------------------------------------------------

        public SerializedProperty spUUID;
		private SerializedProperty spName;
		private SerializedProperty spDescription;
        private SerializedProperty spColor;
		private SerializedProperty spSprite;
		private SerializedProperty spPrefab;
        private SerializedProperty spCanBeSold;
        private SerializedProperty spPrice;
		private SerializedProperty spMaxStack;
        private SerializedProperty spWeight;
        private SerializedProperty spItemTypes;

        private SerializedProperty spOnClick;
        private SerializedProperty spConsumeItem;
        private SerializedProperty spActionsOnClick;
		
        private IActionsListEditor actionsOnClick;

        private int equipToolbarIndex = 0;
        private SerializedProperty spEquipable;
        private SerializedProperty spFillAllTypes;

        private SerializedProperty spConditionsEquip;
        private SerializedProperty spActionsOnEquip;
        private SerializedProperty spActionsOnUnequip;

        private IConditionsListEditor conditionsEquipEditor;
        private IActionsListEditor actionsOnEquipEditor;
        private IActionsListEditor actionsOnUnequipEditor;

		private AnimBool animUnfold;

		// METHODS: ----------------------------------------------------------------------------------------------------

		public void OnEnable()
		{
            if (target == null || serializedObject == null) return;

			this.spUUID = serializedObject.FindProperty(PROP_UUID);
			this.spName = serializedObject.FindProperty(PROP_NAME);
			this.spDescription = serializedObject.FindProperty(PROP_DESCRIPTION);
            this.spColor = serializedObject.FindProperty(PROP_COLOR);
			this.spSprite = serializedObject.FindProperty(PROP_SPRITE);
			this.spPrefab = serializedObject.FindProperty(PROP_PREFAB);
            this.spCanBeSold = serializedObject.FindProperty(PROP_CANBESOLD);
            this.spPrice = serializedObject.FindProperty(PROP_PRICE);
			this.spMaxStack = serializedObject.FindProperty(PROP_MAXSTACK);
            this.spWeight = serializedObject.FindProperty(PROP_WEIGHT);
            this.spItemTypes = serializedObject.FindProperty(PROP_ITEMTYPES);

			this.spOnClick = serializedObject.FindProperty(PROP_ONCLICK);
            this.spConsumeItem = serializedObject.FindProperty(PROP_CONSUMEITEM);
			this.spActionsOnClick = serializedObject.FindProperty(PROP_ACTIONONCLICK);

            this.SetupActionsList(
                ref this.spActionsOnClick,
                ref this.actionsOnClick,
                PATH_PREFAB_CONSUME,
                NAME_PREFAB_CONSUME
            );

            this.spEquipable = serializedObject.FindProperty(PROP_EQUIPABLE);
            this.spFillAllTypes = serializedObject.FindProperty(PROP_FILLALLTYPES);

            this.spConditionsEquip = serializedObject.FindProperty(PROP_CONDITIONSEQUIP);
            this.spActionsOnEquip = serializedObject.FindProperty(PROP_ACTIONSEQUIP);
            this.spActionsOnUnequip = serializedObject.FindProperty(PROP_ACTIONSUNEQUIP);

            this.SetupConditionsList(
                ref this.spConditionsEquip,
                ref this.conditionsEquipEditor,
                PATH_PREFAB_EQUIP_COND,
                NAME_PREFAB_EQUIP_COND
            );

            this.SetupActionsList(
                ref this.spActionsOnEquip,
                ref this.actionsOnEquipEditor,
                PATH_PREFAB_EQUP,
                NAME_PREFAB_EQUIP
            );

            this.SetupActionsList(
                ref this.spActionsOnUnequip,
                ref this.actionsOnUnequipEditor,
                PATH_PREFAB_EQUP,
                NAME_PREFAB_UNEQUIP
            );

			this.animUnfold = new AnimBool(false);
			this.animUnfold.speed = ANIM_BOOL_SPEED;
			this.animUnfold.valueChanged.AddListener(this.Repaint);
		}

        private void SetupConditionsList(ref SerializedProperty sp, ref IConditionsListEditor editor,
                                      string prefabPath, string prefabName)
        {
            if (sp.objectReferenceValue == null)
            {
                GameCreatorUtilities.CreateFolderStructure(prefabPath);
                string conditionsPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(
                    prefabPath, prefabName
                ));

                GameObject sceneInstance = new GameObject("Conditions");
                sceneInstance.AddComponent<IConditionsList>();

                GameObject prefabInstance = PrefabUtility.SaveAsPrefabAsset(sceneInstance, conditionsPath);
                DestroyImmediate(sceneInstance);

                sp.objectReferenceValue = prefabInstance.GetComponent<IConditionsList>();
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
                serializedObject.Update();
            }

            editor = Editor.CreateEditor(
                sp.objectReferenceValue, 
                typeof(IConditionsListEditor)
            ) as IConditionsListEditor;
        }

        private void SetupActionsList(ref SerializedProperty sp, ref IActionsListEditor editor, 
                                      string prefabPath, string prefabName)
        {
            if (sp.objectReferenceValue == null)
            {
                GameCreatorUtilities.CreateFolderStructure(prefabPath);
                string actionsPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(
                    prefabPath, prefabName
                ));

                GameObject sceneInstance = new GameObject("Actions");
                sceneInstance.AddComponent<Actions>();

                GameObject prefabInstance = PrefabUtility.SaveAsPrefabAsset(sceneInstance, actionsPath);
                DestroyImmediate(sceneInstance);

                Actions prefabActions = prefabInstance.GetComponent<Actions>();
                prefabActions.destroyAfterFinishing = true;
                sp.objectReferenceValue = prefabActions.actionsList;
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
                serializedObject.Update();
            }

            editor = Editor.CreateEditor(
                sp.objectReferenceValue, 
                typeof(IActionsListEditor)
            ) as IActionsListEditor;
        }

        public void OnDestroyItem()
		{
			if (this.spActionsOnClick.objectReferenceValue != null)
			{
                IActionsList list1 = (IActionsList)this.spActionsOnClick.objectReferenceValue;
                IActionsList list2 = (IActionsList)this.spActionsOnEquip.objectReferenceValue;
                IActionsList list3 = (IActionsList)this.spActionsOnUnequip.objectReferenceValue;
                IConditionsList cond1 = (IConditionsList)this.spConditionsEquip.objectReferenceValue;

				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(list1.gameObject));
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(list2.gameObject));
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(list3.gameObject));
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(cond1.gameObject));
				AssetDatabase.SaveAssets();
			}
		}

		public override void OnInspectorGUI ()
		{
            if (target == null || serializedObject == null) return;
			EditorGUILayout.HelpBox(
				"This Item can only be edited in the Inventory section of the Preferences window", 
				MessageType.Info
			);

			if (GUILayout.Button("Open Preferences"))
			{
				PreferencesWindow.OpenWindow();
			}
		}

        public ItemReturnOperation OnPreferencesWindowGUI(DatabaseInventoryEditor inventoryEditor, int index)
		{
			serializedObject.Update();
            inventoryEditor.searchText = inventoryEditor.searchText.ToLower();
			string spNameString = this.spName.FindPropertyRelative("content").stringValue;
			string spDescString = this.spDescription.FindPropertyRelative("content").stringValue;

            if (!string.IsNullOrEmpty(inventoryEditor.searchText) && 
                !spNameString.ToLower().Contains(inventoryEditor.searchText) && 
                !spDescString.ToLower().Contains(inventoryEditor.searchText))
			{
				return new ItemReturnOperation();
			}

            ItemReturnOperation result = this.PaintHeader(inventoryEditor, index);
			using (var group = new EditorGUILayout.FadeGroupScope (this.animUnfold.faded))
			{
				if (group.visible)
				{
					EditorGUILayout.BeginVertical(CoreGUIStyles.GetBoxExpanded());
					this.PaintContent();
					EditorGUILayout.EndVertical();
				}
			}

			serializedObject.ApplyModifiedPropertiesWithoutUndo();
			return result;
		}

        private ItemReturnOperation PaintHeader(DatabaseInventoryEditor inventoryEditor, int index)
		{
			bool removeItem = false;
            bool duplicateIndex = false;

            EditorGUILayout.BeginHorizontal();

            bool forceSortRepaint = false;
            if (inventoryEditor.itemsHandleRect.ContainsKey(index))
            {
                EditorGUIUtility.AddCursorRect(inventoryEditor.itemsHandleRect[index], MouseCursor.Pan);
                forceSortRepaint = inventoryEditor.editorSortableListItems.CaptureSortEvents(
                    inventoryEditor.itemsHandleRect[index], index
                );
            }

            if (forceSortRepaint) inventoryEditor.Repaint();

            GUILayout.Label("=", CoreGUIStyles.GetButtonLeft(), GUILayout.Width(25f), GUILayout.Height(BTN_HEIGHT));
            if (UnityEngine.Event.current.type == EventType.Repaint)
            {
                Rect dragRect = GUILayoutUtility.GetLastRect();
                if (inventoryEditor.itemsHandleRect.ContainsKey(index))
                {
                    inventoryEditor.itemsHandleRect[index] = dragRect;
                }
                else
                {
                    inventoryEditor.itemsHandleRect.Add(index, dragRect);
                }
            }

            if (inventoryEditor.itemsHandleRectRow.ContainsKey(index))
            {
                inventoryEditor.editorSortableListItems.PaintDropPoints(
                    inventoryEditor.itemsHandleRectRow[index],
                    index,
                    inventoryEditor.spItems.arraySize
                );
            }

			string name = (this.animUnfold.target ? "▾ " : "▸ ");
			string spNameString = this.spName.FindPropertyRelative("content").stringValue;
			name += (string.IsNullOrEmpty(spNameString) ? "No-name" :  spNameString);

			GUIStyle style = (this.animUnfold.target
				? CoreGUIStyles.GetToggleButtonMidOn() 
				: CoreGUIStyles.GetToggleButtonMidOff()
			);

			if (GUILayout.Button(name, style, GUILayout.Height(BTN_HEIGHT)))
			{
				this.animUnfold.target = !this.animUnfold.value;
			}

            GUIContent gcDuplicate = ClausesUtilities.Get(ClausesUtilities.Icon.Duplicate);
            if (GUILayout.Button(gcDuplicate, CoreGUIStyles.GetButtonMid(), GUILayout.Width(25), GUILayout.Height(BTN_HEIGHT)))
            {
                duplicateIndex = true;
            }

            GUIContent gcDelete = ClausesUtilities.Get(ClausesUtilities.Icon.Delete);
            if (GUILayout.Button(gcDelete, CoreGUIStyles.GetButtonRight(), GUILayout.Width(25), GUILayout.Height(BTN_HEIGHT)))
			{
				removeItem = true;
			}

			EditorGUILayout.EndHorizontal();
            if (UnityEngine.Event.current.type == EventType.Repaint)
            {
                Rect rect = GUILayoutUtility.GetLastRect();
                if (inventoryEditor.itemsHandleRectRow.ContainsKey(index))
                {
                    inventoryEditor.itemsHandleRectRow[index] = rect;
                }
                else
                {
                    inventoryEditor.itemsHandleRectRow.Add(index, rect);
                }
            }

            ItemReturnOperation result = new ItemReturnOperation();
            if (removeItem) result.removeIndex = true;
            if (duplicateIndex) result.duplicateIndex = true;

            return result;
		}

		private void PaintContent()
		{
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.PropertyField(this.spUUID);
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.PropertyField(this.spName);
			EditorGUILayout.PropertyField(this.spDescription);
            EditorGUILayout.PropertyField(this.spColor);

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(this.spSprite);
			EditorGUILayout.PropertyField(this.spPrefab);

			EditorGUILayout.Space();

            EditorGUILayout.PropertyField(this.spCanBeSold);
            EditorGUI.BeginDisabledGroup(!this.spCanBeSold.boolValue);
			EditorGUILayout.PropertyField(this.spPrice);
            EditorGUI.EndDisabledGroup();

			EditorGUILayout.PropertyField(this.spMaxStack);
            EditorGUILayout.PropertyField(this.spWeight);
            EditorGUILayout.PropertyField(this.spItemTypes);

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spOnClick);
			EditorGUI.BeginDisabledGroup(!this.spOnClick.boolValue);
            EditorGUILayout.PropertyField(this.spConsumeItem);

			if (this.actionsOnClick != null)
			{
				this.actionsOnClick.OnInspectorGUI();
			}
			EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spEquipable);
            EditorGUI.BeginDisabledGroup(!this.spEquipable.boolValue);

            EditorGUILayout.PropertyField(this.spFillAllTypes);
            this.equipToolbarIndex = GUILayout.Toolbar(this.equipToolbarIndex, EQUIPABLE_OPTIONS);
            EditorGUILayout.HelpBox(EQUIP_DESC[this.equipToolbarIndex], MessageType.Info);
            switch (this.equipToolbarIndex)
            {
                case 0 : if (this.conditionsEquipEditor != null) this.conditionsEquipEditor.OnInspectorGUI(); break;
                case 1 : if (this.actionsOnEquipEditor != null) this.actionsOnEquipEditor.OnInspectorGUI(); break;
                case 2 : if (this.actionsOnUnequipEditor != null) this.actionsOnUnequipEditor.OnInspectorGUI(); break;
            }

            EditorGUI.EndDisabledGroup();
		}
	}
}