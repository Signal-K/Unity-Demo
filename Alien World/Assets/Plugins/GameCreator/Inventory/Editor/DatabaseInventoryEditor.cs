namespace GameCreator.Inventory
{
	using System;
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using UnityEditor.AnimatedValues;
	using System.Linq;
	using System.Reflection;
	using GameCreator.Core;

	[CustomEditor(typeof(DatabaseInventory))]
	public class DatabaseInventoryEditor : IDatabaseEditor
	{
		private const string PROP_INVENTORY_CATALOGUE = "inventoryCatalogue";
		private const string PROP_INVENTORY_CATALOGUE_ITEMS = "items";
		private const string PROP_INVENTORY_CATALOGUE_RECIPE = "recipes";
        private const string PROP_INVENTORY_CATALOGUE_TYPES = "itemTypes";

		private const string PROP_INVENTORY_SETTINGS = "inventorySettings";
        private const string PROP_CONTAINER_UI_PREFAB = "containerUIPrefab";
        private const string PROP_MERCHANT_UI_PREFAB = "merchantUIPrefab";
        private const string PROP_INVENTORY_UI_PREFAB = "inventoryUIPrefab";
		private const string PROP_INVENTORY_ONDRAG_GRABITEM = "onDragGrabItem";
		private const string PROP_ITEM_CURSOR_DRAG = "cursorDrag";
		private const string PROP_ITEM_CURSOR_DRAG_HS = "cursorDragHotspot";

		private const string PROP_ITEM_DRAG_TO_COMBINE = "dragItemsToCombine";
        private const string PROP_STOPTIME_ONOPEN = "pauseTimeOnUI";
		private const string PROP_DROP_ITEM_OUTSIDE = "canDropItems";
        private const string PROP_SAVE_INVENTORY = "saveInventory";
        private const string PROP_DROP_MAX_DISTANCE = "dropItemMaxDistance";

        private const string PROP_LIMIT_WEIGHT = "limitInventoryWeight";
        private const string PROP_MAX_WEIGHT = "maxInventoryWeight";

		private const string MSG_EMPTY_CATALOGUE = "There are no items. Add one clicking the 'Create Item' button";
		private const string MSG_EMPTY_RECIPES = "There are no recipes. Add one clicking the 'Create Recipe' button";

		private const string SEARCHBOX_NAME = "searchbox";

        private static readonly GUIContent GC_MERCHANT = new GUIContent("Merchant UI Prefab (opt)");
        private static readonly GUIContent GC_CONTAINER = new GUIContent("Container UI Prefab (opt)");
        private static readonly GUIContent GC_INVENTORY = new GUIContent("Inventory UI Prefab (opt)");

        private class ItemsData
		{
			public ItemEditor cachedEditor;
			public SerializedProperty spItem;

			public ItemsData(SerializedProperty item)
			{
				this.spItem = item;

				Editor cache = this.cachedEditor;
				Editor.CreateCachedEditor(item.objectReferenceValue, typeof(ItemEditor), ref cache);
				this.cachedEditor = (ItemEditor)cache;
			}
		}

		private class RecipeData
		{
			public RecipeEditor cachedEditor;
			public SerializedProperty spRecipe;

            public RecipeData(SerializedProperty recipe)
			{
                this.spRecipe = recipe;

				Editor cache = this.cachedEditor;
				Editor.CreateCachedEditor(recipe.objectReferenceValue, typeof(RecipeEditor), ref cache);
				this.cachedEditor = (RecipeEditor)cache;
			}
		}

        private static readonly GUIContent[] TAB_NAMES = new GUIContent[]
		{
			new GUIContent("Catalogue"),
            new GUIContent("Types"),
			new GUIContent("Recipes"),
			new GUIContent("Settings")
		};

		// PROPERTIES: -------------------------------------------------------------------------------------------------

		private int tabIndex = 0;

		public SerializedProperty spItems;
        public SerializedProperty spRecipes;
        public SerializedProperty spItemTypes;

        private SerializedProperty spContainerUIPrefab;
        private SerializedProperty spMerchantUIPrefab;
		private SerializedProperty spInventoryUIPrefab;
		private SerializedProperty spItemOnDragGrabItem;
		private SerializedProperty spItemCursorDrag;
        private SerializedProperty spSaveInventory;

        private SerializedProperty spItemCursorDragHotspot;

		private SerializedProperty spItemDragToCombine;
        private SerializedProperty spInventoryStopTime;
        private SerializedProperty spCanDropItems;
        private SerializedProperty spDropMaxDistance;

        private SerializedProperty spLimitWeight;
        private SerializedProperty spMaxWeight;

		private List<ItemsData> itemsData;
		private List<RecipeData> recipesData;

		private GUIStyle searchFieldStyle;
		private GUIStyle searchCloseOnStyle;
		private GUIStyle searchCloseOffStyle;

		public string searchText = "";
		public bool searchFocus = true;

        public EditorSortableList editorSortableListItems;
        public EditorSortableList editorSortableListRecipes;

        public Dictionary<int, Rect> itemsHandleRect = new Dictionary<int, Rect>();
        public Dictionary<int, Rect> recipesHandleRect = new Dictionary<int, Rect>();

        public Dictionary<int, Rect> itemsHandleRectRow = new Dictionary<int, Rect>();
        public Dictionary<int, Rect> recipesHandleRectRow = new Dictionary<int, Rect>();

		// INITIALIZE: -------------------------------------------------------------------------------------------------

		private void OnEnable()
		{
            if (target == null || serializedObject == null) return;

			SerializedProperty spInventoryCatalogue = serializedObject.FindProperty(PROP_INVENTORY_CATALOGUE);
			this.spItems = spInventoryCatalogue.FindPropertyRelative(PROP_INVENTORY_CATALOGUE_ITEMS);
			this.spRecipes = spInventoryCatalogue.FindPropertyRelative(PROP_INVENTORY_CATALOGUE_RECIPE);
            this.spItemTypes = spInventoryCatalogue.FindPropertyRelative(PROP_INVENTORY_CATALOGUE_TYPES);

			SerializedProperty spInventorySettings = serializedObject.FindProperty(PROP_INVENTORY_SETTINGS);
            this.spMerchantUIPrefab = spInventorySettings.FindPropertyRelative(PROP_MERCHANT_UI_PREFAB);
            this.spContainerUIPrefab = spInventorySettings.FindPropertyRelative(PROP_CONTAINER_UI_PREFAB);
            this.spInventoryUIPrefab = spInventorySettings.FindPropertyRelative(PROP_INVENTORY_UI_PREFAB);
			this.spItemOnDragGrabItem = spInventorySettings.FindPropertyRelative(PROP_INVENTORY_ONDRAG_GRABITEM);
			this.spItemCursorDrag = spInventorySettings.FindPropertyRelative(PROP_ITEM_CURSOR_DRAG);
            this.spSaveInventory = spInventorySettings.FindPropertyRelative(PROP_SAVE_INVENTORY);

            this.spItemCursorDragHotspot = spInventorySettings.FindPropertyRelative(PROP_ITEM_CURSOR_DRAG_HS);

			this.spItemDragToCombine = spInventorySettings.FindPropertyRelative(PROP_ITEM_DRAG_TO_COMBINE);
			this.spInventoryStopTime = spInventorySettings.FindPropertyRelative(PROP_STOPTIME_ONOPEN);
			this.spCanDropItems = spInventorySettings.FindPropertyRelative(PROP_DROP_ITEM_OUTSIDE);
            this.spDropMaxDistance = spInventorySettings.FindPropertyRelative(PROP_DROP_MAX_DISTANCE);

            this.spLimitWeight = spInventorySettings.FindPropertyRelative(PROP_LIMIT_WEIGHT);
            this.spMaxWeight = spInventorySettings.FindPropertyRelative(PROP_MAX_WEIGHT);

			int itemsSize = this.spItems.arraySize;
			this.itemsData = new List<ItemsData>();
			for (int i = 0; i < itemsSize; ++i)
			{
				this.itemsData.Add(new ItemsData(this.spItems.GetArrayElementAtIndex(i)));
			}

			int recipesSize = this.spRecipes.arraySize;
			this.recipesData = new List<RecipeData>();
			for (int i = 0; i < recipesSize; ++i)
			{
				this.recipesData.Add(new RecipeData(this.spRecipes.GetArrayElementAtIndex(i)));
			}

            this.editorSortableListItems = new EditorSortableList();
            this.editorSortableListRecipes = new EditorSortableList();
		}

		// OVERRIDE METHODS: -------------------------------------------------------------------------------------------

		public override string GetName ()
		{
			return "Inventory";
		}

        public override bool CanBeDecoupled()
        {
            return true;
        }

        // GUI METHODS: ------------------------------------------------------------------------------------------------

        public override void OnInspectorGUI ()
		{
			this.OnPreferencesWindowGUI();
		}

		public override void OnPreferencesWindowGUI()
		{
			this.serializedObject.Update();

			int prevTabIndex = this.tabIndex;
			this.tabIndex = GUILayout.Toolbar(this.tabIndex, TAB_NAMES);
			if (prevTabIndex != this.tabIndex) this.ResetSearch();

			EditorGUILayout.Space();

			switch (this.tabIndex)
			{
			case 0 : this.PaintCatalogue(); break;
            case 1 : this.PaintTypes(); break;
			case 2 : this.PaintRecipes(); break;
			case 3 : this.PaintSettings(); break;
			}

			this.serializedObject.ApplyModifiedPropertiesWithoutUndo();
		}

		private void PaintCatalogue()
		{
            ItemEditor.ItemReturnOperation returnOp = new ItemEditor.ItemReturnOperation();
            int removeIndex = -1;
            int duplicateIndex = -1;

            this.PaintSearch();

			int itemsCatalogueSize = this.spItems.arraySize;
			if (itemsCatalogueSize == 0)
			{
				EditorGUILayout.HelpBox(MSG_EMPTY_CATALOGUE, MessageType.Info);
			}

			for (int i = 0; i < itemsCatalogueSize; ++i)
			{
				if (this.itemsData[i].cachedEditor == null) continue;
                returnOp = this.itemsData[i].cachedEditor.OnPreferencesWindowGUI(this, i);
                if (returnOp.removeIndex) removeIndex = i;
                if (returnOp.duplicateIndex) duplicateIndex = i;
            }

			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Create Item", GUILayout.MaxWidth(200)))
			{
				this.ResetSearch();

				int insertIndex = itemsCatalogueSize;
				this.spItems.InsertArrayElementAtIndex(insertIndex);

				Item item = Item.CreateItemInstance();
				this.spItems.GetArrayElementAtIndex(insertIndex).objectReferenceValue = item;
				this.itemsData.Insert(insertIndex, new ItemsData(this.spItems.GetArrayElementAtIndex(insertIndex)));
			}

			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			if (removeIndex != -1)
			{
				this.itemsData[removeIndex].cachedEditor.OnDestroyItem();
				UnityEngine.Object deleteItem = this.itemsData[removeIndex].cachedEditor.target;
				this.spItems.RemoveFromObjectArrayAt(removeIndex);
				this.itemsData.RemoveAt(removeIndex);

				string path = AssetDatabase.GetAssetPath(deleteItem);
				DestroyImmediate(deleteItem, true);
				AssetDatabase.ImportAsset(path);
			}
            else if (duplicateIndex != -1)
            {
                this.ResetSearch();

                int srcIndex = duplicateIndex;
                int insertIndex = duplicateIndex + 1;

                this.spItems.InsertArrayElementAtIndex(insertIndex);

                Item item = Item.CreateItemInstance();
                EditorUtility.CopySerialized(
                    this.spItems.GetArrayElementAtIndex(srcIndex).objectReferenceValue,
                    item
                );

                SerializedProperty newItem = this.spItems.GetArrayElementAtIndex(insertIndex);
                newItem.objectReferenceValue = item;

                newItem.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                newItem.serializedObject.Update();

                ItemsData newItemData = new ItemsData(newItem);

                newItemData.cachedEditor.serializedObject
                    .FindProperty("actionsOnClick")
                    .objectReferenceValue = this.MakeCopyOf(item.actionsOnClick);

                newItemData.cachedEditor.serializedObject
                    .FindProperty("actionsOnEquip")
                    .objectReferenceValue = this.MakeCopyOf(item.actionsOnEquip);

                newItemData.cachedEditor.serializedObject
                    .FindProperty("actionsOnUnequip")
                    .objectReferenceValue = this.MakeCopyOf(item.actionsOnUnequip);

                newItemData.cachedEditor.serializedObject
                    .FindProperty("conditionsEquip")
                    .objectReferenceValue = this.MakeCopyOf(item.conditionsEquip);

                int uuid = Mathf.Abs(Guid.NewGuid().GetHashCode());
                newItemData.cachedEditor.spUUID.intValue = uuid;

                newItemData.cachedEditor.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                newItemData.cachedEditor.serializedObject.Update();
                newItemData.cachedEditor.OnEnable();

                this.itemsData.Insert(insertIndex, newItemData);
            }

            EditorSortableList.SwapIndexes swapIndexes = this.editorSortableListItems.GetSortIndexes();
            if (swapIndexes != null)
            {
                this.spItems.MoveArrayElement(swapIndexes.src, swapIndexes.dst);

                ItemsData tempItem = this.itemsData[swapIndexes.src];
                this.itemsData[swapIndexes.src] = this.itemsData[swapIndexes.dst];
                this.itemsData[swapIndexes.dst] = tempItem;
            }
		}

        private GameObject MakeCopyOf(UnityEngine.Object original)
        {
            string originalPath = AssetDatabase.GetAssetPath(original);
            string targetPath = AssetDatabase.GenerateUniqueAssetPath(originalPath);

            AssetDatabase.CopyAsset(originalPath, targetPath);
            AssetDatabase.Refresh();

            return AssetDatabase.LoadAssetAtPath<GameObject>(targetPath);
        }

        private void PaintTypes()
        {
            this.spItemTypes.arraySize = ItemType.MAX;
            for (int i = 0; i < ItemType.MAX; ++i)
            {
                SerializedProperty spItemType = this.spItemTypes.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.PropertyField(
                    spItemType.FindPropertyRelative("id"),
                    new GUIContent(string.Format("ID: {0}", i + 1))
                );
                EditorGUILayout.PropertyField(spItemType.FindPropertyRelative("name"), GUIContent.none);

                EditorGUILayout.EndHorizontal();
            }
        }

		private void PaintRecipes()
		{
			int removeIndex = -1;

			int recipeCatalogueSize = this.spRecipes.arraySize;
			if (recipeCatalogueSize == 0)
			{
				EditorGUILayout.HelpBox(MSG_EMPTY_RECIPES, MessageType.Info);
			}

			for (int i = 0; i < recipeCatalogueSize; ++i)
			{
				bool removeRecipe = this.recipesData[i].cachedEditor.OnPreferencesWindowGUI(this, i);
				if (removeRecipe) removeIndex = i;
			}

			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Create Recipe", GUILayout.MaxWidth(200)))
			{
				this.ResetSearch();

				int insertIndex = recipeCatalogueSize;
				this.spRecipes.InsertArrayElementAtIndex(insertIndex);

				Recipe recipe = Recipe.CreateRecipeInstance();
				this.spRecipes.GetArrayElementAtIndex(insertIndex).objectReferenceValue = recipe;
				this.recipesData.Insert(insertIndex, new RecipeData(this.spRecipes.GetArrayElementAtIndex(insertIndex)));
			}

			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			if (removeIndex != -1)
			{
				this.recipesData[removeIndex].cachedEditor.OnDestroyRecipe();
				UnityEngine.Object deleteRecipe = this.recipesData[removeIndex].cachedEditor.target;

				this.spRecipes.RemoveFromObjectArrayAt(removeIndex);
				this.recipesData.RemoveAt(removeIndex);

				string path = AssetDatabase.GetAssetPath(deleteRecipe);
				DestroyImmediate(deleteRecipe, true);
				AssetDatabase.ImportAsset(path);
			}

            EditorSortableList.SwapIndexes swapIndexes = this.editorSortableListRecipes.GetSortIndexes();
            if (swapIndexes != null)
            {
                this.spRecipes.MoveArrayElement(swapIndexes.src, swapIndexes.dst);

                RecipeData tempRecipt = this.recipesData[swapIndexes.src];
                this.recipesData[swapIndexes.src] = this.recipesData[swapIndexes.dst];
                this.recipesData[swapIndexes.dst] = tempRecipt;
            }
		}

		private void PaintSettings()
		{
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);

			EditorGUILayout.LabelField("User Interface", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(this.spMerchantUIPrefab, GC_MERCHANT);
            EditorGUILayout.PropertyField(this.spContainerUIPrefab, GC_CONTAINER);
            EditorGUILayout.PropertyField(this.spInventoryUIPrefab, GC_INVENTORY);
			EditorGUILayout.PropertyField(this.spItemOnDragGrabItem);

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spItemCursorDrag);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(this.spItemCursorDragHotspot.displayName);
			EditorGUILayout.PropertyField(this.spItemCursorDragHotspot, GUIContent.none);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			EditorGUILayout.LabelField("Behavior Configuration", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(this.spItemDragToCombine);
			EditorGUILayout.PropertyField(this.spInventoryStopTime);
            EditorGUILayout.PropertyField(this.spSaveInventory);

            EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spCanDropItems);
            EditorGUI.BeginDisabledGroup(!this.spCanDropItems.boolValue);
            EditorGUILayout.PropertyField(this.spDropMaxDistance);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spLimitWeight);
            EditorGUI.indentLevel++;
            EditorGUI.BeginDisabledGroup(!this.spLimitWeight.boolValue);
            EditorGUILayout.PropertyField(this.spMaxWeight);
            EditorGUI.EndDisabledGroup();
            EditorGUI.indentLevel++;

			EditorGUILayout.EndVertical();
		}

		// PRIVATE METHODS: --------------------------------------------------------------------------------------------

		private void PaintSearch()
		{
			if (this.searchFieldStyle == null) this.searchFieldStyle = new GUIStyle(GUI.skin.FindStyle("SearchTextField"));
			if (this.searchCloseOnStyle == null) this.searchCloseOnStyle = new GUIStyle(GUI.skin.FindStyle("SearchCancelButton"));
			if (this.searchCloseOffStyle == null) this.searchCloseOffStyle = new GUIStyle(GUI.skin.FindStyle("SearchCancelButtonEmpty"));

			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(5f);

			GUI.SetNextControlName(SEARCHBOX_NAME);
			this.searchText = EditorGUILayout.TextField(this.searchText, this.searchFieldStyle);

			if (this.searchFocus)
			{
				EditorGUI.FocusTextInControl(SEARCHBOX_NAME);
				this.searchFocus = false;
			}

			GUIStyle style = (string.IsNullOrEmpty(this.searchText) 
				? this.searchCloseOffStyle 
				: this.searchCloseOnStyle
			);

			if (GUILayout.Button("", style)) 
			{
				this.ResetSearch();
			}

			GUILayout.Space(5f);
			EditorGUILayout.EndHorizontal();
		}

		private void ResetSearch()
		{
			this.searchText = "";
			GUIUtility.keyboardControl = 0;
			EditorGUIUtility.keyboardControl = 0;
			this.searchFocus = true;
		}
    }
}