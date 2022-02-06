namespace GameCreator.Inventory
{
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using UnityEditor.AnimatedValues;
	using GameCreator.Core;

	[CustomEditor(typeof(Recipe))]
	public class RecipeEditor : Editor 
	{
		private const float ITEM_SIZE = 50f;
		private const float ANIM_BOOL_SPEED = 3.0f;
		private const string NAME_FORMAT = "{0} x {1} + {2} x {3}";

		private const string PROP_ITEM_1 = "itemToCombineA";
		private const string PROP_AMOU_1 = "amountA";
		private const string PROP_ITEM_2 = "itemToCombineB";
		private const string PROP_AMOU_2 = "amountB";
		private const string PROP_ONCRAFT = "removeItemsOnCraft";
		private const string PROP_ACTION = "actionsList";

		private const string PATH_PREFAB_RECIPES = "Assets/Plugins/GameCreatorData/Inventory/Prefabs/Recipes/";
		private const string NAME_PREFAB_RECIPES = "recipe.prefab";

		// PROPERTIES: -------------------------------------------------------------------------------------------------

		private SerializedProperty spItemHolderToCombineA;
		private SerializedProperty spItemToCombineA;
		private SerializedProperty spAmountA;
		private SerializedProperty spItemHolderToCombineB;
		private SerializedProperty spItemToCombineB;
		private SerializedProperty spAmountB;

		private SerializedProperty spRemoveItemsOnCraft;
		private SerializedProperty spActionsList;
		private IActionsListEditor actionsListEditor;

		private AnimBool animUnfold;
		private GUIContent guiContentAmount = new GUIContent("Amount");

		// METHODS: ----------------------------------------------------------------------------------------------------

		private void OnEnable()
		{
			this.spItemHolderToCombineA = serializedObject.FindProperty(PROP_ITEM_1);
			this.spItemToCombineA = this.spItemHolderToCombineA.FindPropertyRelative("item");
			this.spAmountA = serializedObject.FindProperty(PROP_AMOU_1);
			this.spItemHolderToCombineB = serializedObject.FindProperty(PROP_ITEM_2);
			this.spItemToCombineB = this.spItemHolderToCombineB.FindPropertyRelative("item");
			this.spAmountB = serializedObject.FindProperty(PROP_AMOU_2);

			this.spRemoveItemsOnCraft = serializedObject.FindProperty(PROP_ONCRAFT);
			this.spActionsList = serializedObject.FindProperty(PROP_ACTION);
			if (this.spActionsList.objectReferenceValue == null)
			{
				GameCreatorUtilities.CreateFolderStructure(PATH_PREFAB_RECIPES);
				string actionsPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(
					PATH_PREFAB_RECIPES, NAME_PREFAB_RECIPES)
				);

				GameObject sceneInstance = new GameObject("RecipeActions");
				sceneInstance.AddComponent<Actions>();

				GameObject prefabInstance = PrefabUtility.SaveAsPrefabAsset(sceneInstance, actionsPath);
				DestroyImmediate(sceneInstance);

				Actions prefabActions = prefabInstance.GetComponent<Actions>();
				prefabActions.destroyAfterFinishing = true;
				this.spActionsList.objectReferenceValue = prefabActions.actionsList;
				serializedObject.ApplyModifiedPropertiesWithoutUndo();
				serializedObject.Update();
			}

			this.actionsListEditor = (IActionsListEditor)Editor.CreateEditor(
				this.spActionsList.objectReferenceValue, typeof(IActionsListEditor)
			);

			this.animUnfold = new AnimBool(false);
			this.animUnfold.speed = ANIM_BOOL_SPEED;
			this.animUnfold.valueChanged.AddListener(this.Repaint);
		}

		public void OnDestroyRecipe()
		{
			if (this.spActionsList.objectReferenceValue != null)
			{
				IActionsList list = (IActionsList)this.spActionsList.objectReferenceValue;
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(list.gameObject));
				AssetDatabase.SaveAssets();
			}
		}

		public override void OnInspectorGUI ()
		{
			EditorGUILayout.HelpBox(
				"This Recipe can only be edited in the Inventory section of the Preferences window", 
				MessageType.Info
			);

			if (GUILayout.Button("Open Preferences"))
			{
				PreferencesWindow.OpenWindow();
			}
		}

        public bool OnPreferencesWindowGUI(DatabaseInventoryEditor inventoryEditor, int index)
		{
			serializedObject.Update();

            bool result = this.PaintHeader(inventoryEditor, index);
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

        private bool PaintHeader(DatabaseInventoryEditor inventoryEditor, int index)
		{
			bool removeItem = false;

			EditorGUILayout.BeginHorizontal();

            bool forceSortRepaint = false;
            if (inventoryEditor.recipesHandleRect.ContainsKey(index))
            {
                EditorGUIUtility.AddCursorRect(inventoryEditor.recipesHandleRect[index], MouseCursor.Pan);
                forceSortRepaint = inventoryEditor.editorSortableListRecipes.CaptureSortEvents(
                    inventoryEditor.recipesHandleRect[index], index
                );
            }

            if (forceSortRepaint) inventoryEditor.Repaint();

            GUILayout.Label("=", CoreGUIStyles.GetButtonLeft(), GUILayout.Width(25f));
            if (UnityEngine.Event.current.type == EventType.Repaint)
            {
                Rect dragRect = GUILayoutUtility.GetLastRect();
                if (inventoryEditor.recipesHandleRect.ContainsKey(index))
                {
                    inventoryEditor.recipesHandleRect[index] = dragRect;
                }
                else
                {
                    inventoryEditor.recipesHandleRect.Add(index, dragRect);
                }
            }

            if (inventoryEditor.recipesHandleRectRow.ContainsKey(index))
            {
                inventoryEditor.editorSortableListRecipes.PaintDropPoints(
                    inventoryEditor.recipesHandleRectRow[index],
                    index,
                    inventoryEditor.spRecipes.arraySize
                );
            }

			string name = (this.animUnfold.target ? "▾ " : "▸ ") + this.GetHeaderName();
            GUIStyle style = (this.animUnfold.target
                ? CoreGUIStyles.GetToggleButtonMidOn()
                : CoreGUIStyles.GetToggleButtonMidOff()
            );

			if (GUILayout.Button(name, style))
			{
				this.animUnfold.target = !this.animUnfold.value;
			}

			if (GUILayout.Button("×", CoreGUIStyles.GetButtonRight(), GUILayout.Width(25)))
			{
				removeItem = true;
			}

			EditorGUILayout.EndHorizontal();
            if (UnityEngine.Event.current.type == EventType.Repaint)
            {
                Rect rect = GUILayoutUtility.GetLastRect();
                if (inventoryEditor.recipesHandleRectRow.ContainsKey(index))
                {
                    inventoryEditor.recipesHandleRectRow[index] = rect;
                }
                else
                {
                    inventoryEditor.recipesHandleRectRow.Add(index, rect);
                }
            }

			return removeItem;
		}

		private void PaintContent()
		{
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			EditorGUILayout.PropertyField(this.spItemHolderToCombineA);
			EditorGUILayout.PropertyField(this.spAmountA, this.guiContentAmount);

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(this.spItemHolderToCombineB);
			EditorGUILayout.PropertyField(this.spAmountB, this.guiContentAmount);
			EditorGUILayout.EndVertical();

			this.spAmountA.intValue = Mathf.Max(0, this.spAmountA.intValue);
			this.spAmountB.intValue = Mathf.Max(0, this.spAmountB.intValue);

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			EditorGUILayout.PropertyField(this.spRemoveItemsOnCraft);
			if (this.actionsListEditor != null) this.actionsListEditor.OnInspectorGUI();
			EditorGUILayout.EndVertical();
		}

		// PRIVATE METHODS: --------------------------------------------------------------------------------------------

		private string GetHeaderName()
		{
			bool existsA = this.spItemToCombineA != null && this.spItemToCombineA.objectReferenceValue != null;
			bool existsB = this.spItemToCombineB != null && this.spItemToCombineB.objectReferenceValue != null;

			return string.Format(
				NAME_FORMAT,
				this.spAmountA.intValue.ToString(),
				(existsA ? ((Item)this.spItemToCombineA.objectReferenceValue).itemName.content : "(none)"),
				this.spAmountB.intValue.ToString(),
				(existsB ? ((Item)this.spItemToCombineB.objectReferenceValue).itemName.content : "(none)")
			);
		}
	}
}