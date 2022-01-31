namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using GameCreator.Core;

	public class ItemHolderPropertyDrawerWindow : PopupWindowContent
	{
		private const string INPUTTEXT_NAME = "gamecreator-itemholder-input";
		private const float WIN_HEIGHT = 300f;
		private static DatabaseInventory DATABASE_INVENTORY;

		private Rect windowRect = Rect.zero;
		private bool inputfieldFocus = true;
		private Vector2 scroll = Vector2.zero;
		private int itemsIndex = -1;

		private string searchText = "";
		private List<int> suggestions = new List<int>();

		private GUIStyle inputBGStyle;
		private GUIStyle suggestionHeaderStyle;
		private GUIStyle suggestionItemStyle;
		private GUIStyle searchFieldStyle;
		private GUIStyle searchCloseOnStyle;
		private GUIStyle searchCloseOffStyle;

        //ItemHolderPropertyDrawer itemHolderPropertyDrawer;
        SerializedProperty property;

		private bool keyPressedAny   = false;
		private bool keyPressedUp    = false;
		private bool keyPressedDown  = false;
		private bool keyPressedEnter = false;
		private bool keyFlagVerticalMoved = false;
		private Rect itemSelectedRect = Rect.zero;

		// PUBLIC METHODS: ---------------------------------------------------------------------------------------------

        public ItemHolderPropertyDrawerWindow(Rect activatorRect, SerializedProperty property)
		{
			this.windowRect = new Rect(
				activatorRect.x,
				activatorRect.y + activatorRect.height,
				activatorRect.width,
				WIN_HEIGHT
			);

			this.inputfieldFocus = true;
			this.scroll = Vector2.zero;
            this.property = property;

			if (DATABASE_INVENTORY == null) DATABASE_INVENTORY = DatabaseInventory.LoadDatabase<DatabaseInventory>();
		}

		public override Vector2 GetWindowSize ()
		{
			return new Vector2(this.windowRect.width, WIN_HEIGHT);
		}

		public override void OnOpen ()
		{
			this.inputBGStyle = new GUIStyle(GUI.skin.FindStyle("TabWindowBackground"));
			this.suggestionHeaderStyle = new GUIStyle(GUI.skin.FindStyle("IN BigTitle"));
			this.suggestionHeaderStyle.margin = new RectOffset(0,0,0,0);
			this.suggestionItemStyle = new GUIStyle(GUI.skin.FindStyle("MenuItem"));
			this.searchFieldStyle = new GUIStyle(GUI.skin.FindStyle("SearchTextField"));
			this.searchCloseOnStyle = new GUIStyle(GUI.skin.FindStyle("SearchCancelButton"));
			this.searchCloseOffStyle = new GUIStyle(GUI.skin.FindStyle("SearchCancelButtonEmpty"));

			this.inputfieldFocus = true;

			this.searchText = "";
			this.suggestions = DATABASE_INVENTORY.GetItemSuggestions(this.searchText);
		}

		// GUI METHODS: ------------------------------------------------------------------------------------------------

		public override void OnGUI(Rect windowRect)
		{
			if (this.property == null) { this.editorWindow.Close(); return; }
			this.property.serializedObject.Update();

			this.HandleKeyboardInput();

			string modSearchText = this.searchText;
			this.PaintInputfield(ref modSearchText);
			this.PaintSuggestions(ref modSearchText);

			this.searchText = modSearchText;

			this.property.serializedObject.ApplyModifiedPropertiesWithoutUndo();

			if (this.keyPressedEnter) 
			{
				this.editorWindow.Close();
				UnityEngine.Event.current.Use();
			}

			bool repaintEvent = false;
			repaintEvent = repaintEvent || UnityEngine.Event.current.type == EventType.MouseMove;
			repaintEvent = repaintEvent || UnityEngine.Event.current.type == EventType.MouseDown;
			repaintEvent = repaintEvent || this.keyPressedAny;
			if (repaintEvent) this.editorWindow.Repaint();
		}

		// PRIVATE METHODS: --------------------------------------------------------------------------------------------

		private void HandleKeyboardInput()
		{
			this.keyPressedAny   = false;
			this.keyPressedUp    = false;
			this.keyPressedDown  = false;
			this.keyPressedEnter = false;

			if (UnityEngine.Event.current.type != EventType.KeyDown) return;

			this.keyPressedAny   = true;
			this.keyPressedUp    = (UnityEngine.Event.current.keyCode == KeyCode.UpArrow);
			this.keyPressedDown  = (UnityEngine.Event.current.keyCode == KeyCode.DownArrow);

			this.keyPressedEnter = (
				UnityEngine.Event.current.keyCode == KeyCode.KeypadEnter || 
				UnityEngine.Event.current.keyCode == KeyCode.Return
			);

			this.keyFlagVerticalMoved = (
				this.keyPressedUp || 
				this.keyPressedDown
			);
		}

		private void PaintInputfield(ref string modifiedText)
		{
			EditorGUILayout.BeginHorizontal(this.inputBGStyle);

			GUI.SetNextControlName(INPUTTEXT_NAME);
			modifiedText = EditorGUILayout.TextField(GUIContent.none, modifiedText, this.searchFieldStyle);


			GUIStyle style = (string.IsNullOrEmpty(this.searchText) 
				? this.searchCloseOffStyle 
				: this.searchCloseOnStyle
			);

			if (this.inputfieldFocus)
			{
				EditorGUI.FocusTextInControl(INPUTTEXT_NAME);
				this.inputfieldFocus = false;
			}

			if (GUILayout.Button("", style)) 
			{
				modifiedText = "";
				GUIUtility.keyboardControl = 0;
				EditorGUIUtility.keyboardControl = 0;
				this.inputfieldFocus = true;
			}

			EditorGUILayout.EndHorizontal();
		}

		private void PaintSuggestions(ref string modifiedText)
		{
			EditorGUILayout.BeginHorizontal(this.suggestionHeaderStyle);
			EditorGUILayout.LabelField("Suggestions", EditorStyles.boldLabel);
			EditorGUILayout.EndHorizontal();

			this.scroll = EditorGUILayout.BeginScrollView(this.scroll);
			if (modifiedText != this.searchText)
			{
				this.suggestions = DATABASE_INVENTORY.GetItemSuggestions(modifiedText);
			}

			int suggestionCount = this.suggestions.Count;

			if (suggestionCount > 0)
			{
				for (int i = 0; i < suggestionCount; ++i)
				{
					Item item = DATABASE_INVENTORY.inventoryCatalogue.items[this.suggestions[i]];
					string itemName = (string.IsNullOrEmpty(item.itemName.content) ? "No-name" : item.itemName.content);
					GUIContent itemContent = new GUIContent(itemName);

					Rect itemRect = GUILayoutUtility.GetRect(itemContent, this.suggestionItemStyle);
					bool itemHasFocus = (i == this.itemsIndex);
					bool mouseEnter = itemHasFocus && UnityEngine.Event.current.type == EventType.MouseDown;

					if (UnityEngine.Event.current.type == EventType.Repaint)
					{
						this.suggestionItemStyle.Draw(
							itemRect, 
							itemContent, 
							itemHasFocus, 
							itemHasFocus, 
							false, 
							false
						);
					}

					if (this.itemsIndex == i) this.itemSelectedRect = itemRect;

					if (itemHasFocus)
					{
						if (mouseEnter || this.keyPressedEnter)
						{
							if (this.keyPressedEnter) UnityEngine.Event.current.Use();
							modifiedText = itemName;
                            SerializedProperty spItem = this.property.FindPropertyRelative(ItemHolderPropertyDrawer.PROP_ITEM);
                            spItem.objectReferenceValue = item;
                            this.property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                            this.property.serializedObject.Update();

							this.editorWindow.Close();
						}
					}

					if (UnityEngine.Event.current.type == EventType.MouseMove &&
						GUILayoutUtility.GetLastRect().Contains(UnityEngine.Event.current.mousePosition))
					{
						this.itemsIndex = i;
					}
				}

				if (this.keyPressedDown && this.itemsIndex < suggestionCount - 1)
				{
					this.itemsIndex++;
					UnityEngine.Event.current.Use();
				}
				else if (this.keyPressedUp && this.itemsIndex > 0)
				{
					this.itemsIndex--;
					UnityEngine.Event.current.Use();
				}
			}

			EditorGUILayout.EndScrollView();
			float scrollHeight = GUILayoutUtility.GetLastRect().height;

			if (UnityEngine.Event.current.type == EventType.Repaint && this.keyFlagVerticalMoved)
			{
				this.keyFlagVerticalMoved = false;
				if (this.itemSelectedRect != Rect.zero)
				{
					bool isUpperLimit = this.scroll.y > this.itemSelectedRect.y;
					bool isLowerLimit = (this.scroll.y + scrollHeight < 
						this.itemSelectedRect.position.y + this.itemSelectedRect.size.y
					);

					if (isUpperLimit)
					{
						this.scroll = Vector2.up * (this.itemSelectedRect.position.y);
						this.editorWindow.Repaint();
					}
					else if (isLowerLimit)
					{
						float positionY = this.itemSelectedRect.y + this.itemSelectedRect.height - scrollHeight;
						this.scroll = Vector2.up * positionY;
						this.editorWindow.Repaint();
					}
				}
			}
		}
	}
}