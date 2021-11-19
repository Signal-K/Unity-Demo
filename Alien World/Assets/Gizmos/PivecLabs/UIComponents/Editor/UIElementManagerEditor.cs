namespace GameCreator.UIComponents
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.UI;
	using GameCreator.Core;
	using GameCreator.Variables;
  

#if UNITY_EDITOR
	using UnityEditor;
#endif

	[CustomEditor(typeof(UIElementManager))]
	public class UIElementManagerEditor : Editor
	{
		private int headerWidthCorrectionForScaling = 38;
		public string headerFlexibleStyle = "Box";
		private Texture2D header;
		public Color backgroundColorByDefault;

		SerializedProperty keys1;
		SerializedProperty keys2;
		SerializedProperty keys3;
		SerializedProperty keys4;
		SerializedProperty uicanvas;

		
		void OnEnable()
		{
			header = Resources.Load("piveclabs") as Texture2D;
			keys1 = serializedObject.FindProperty("keys1");
			keys2 = serializedObject.FindProperty("keys2");
			keys3 = serializedObject.FindProperty("keys3");
			keys4 = serializedObject.FindProperty("keys4");
			uicanvas = serializedObject.FindProperty("uicanvas");

		}
		
				
		public override void OnInspectorGUI()
		{
			DrawEditorByDefaultWithHeaderAndHelpBox();
			serializedObject.Update();
			EditorGUILayout.Space();

			EditorGUILayout.LabelField("UI Element Manager", EditorStyles.boldLabel);
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(uicanvas,new GUIContent("UI Canvas"));
			EditorGUILayout.Space();
			EditorGUI.indentLevel++;
			EditorGUILayout.LabelField("GC Buttons", EditorStyles.label);
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(keys1,new GUIContent("Tab Key"));
			EditorGUI.indentLevel--;
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("GC Input Fields", EditorStyles.label);
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(keys2,new GUIContent("Tab Key"));
			EditorGUI.indentLevel--;
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("GC Sliders", EditorStyles.label);
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(keys3,new GUIContent("Tab Key"));
			EditorGUI.indentLevel--;
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("RawImages", EditorStyles.label);
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(keys4,new GUIContent("Tab Key"));

			EditorGUI.indentLevel--;
			EditorGUILayout.Space();
			EditorGUI.indentLevel--;

			EditorGUILayout.LabelField("Note:", EditorStyles.boldLabel);
			EditorGUILayout.LabelField("All TAB keys must be unqiue for each element on the Canvas.", EditorStyles.label);
			EditorGUILayout.Space();

			serializedObject.ApplyModifiedProperties();

		}
		
		public void DrawEditorByDefaultWithHeaderAndHelpBox()
		{

			DrawHeaderFlexible(header, Color.black);
			DrawHelpBox();

		}

		public void DrawHeaderFlexible(Texture2D header, Color backgroundColor)
		{
			if (header)
			{
				if (header.width + headerWidthCorrectionForScaling < EditorGUIUtility.currentViewWidth)
				{
					EditorGUILayout.BeginVertical(headerFlexibleStyle);

					DrawHeader(header);

					EditorGUILayout.EndVertical();
				}
				else
				{
					DrawHeaderIfScrollbar(header);
				}
			}
		}

		public void DrawHeaderIfScrollbar(Texture2D header)
		{
			EditorGUI.DrawTextureTransparent(
				GUILayoutUtility.GetRect(
				EditorGUIUtility.currentViewWidth - headerWidthCorrectionForScaling,
				header.height),
				header,
				ScaleMode.ScaleToFit);
		}

		public void DrawHeader(Texture2D header)
		{
			EditorGUI.DrawTextureTransparent(
				GUILayoutUtility.GetRect(
				header.width,
				header.height),
				header,
				ScaleMode.ScaleToFit);
		}


		public void DrawHelpBox()
		{
			LinkButton("https://docs.piveclabs.com");


		}

		private void LinkButton(string url)
		{
			var style = GUI.skin.GetStyle("HelpBox");
			style.richText = true;
			style.alignment = TextAnchor.MiddleCenter;

			bool bClicked = GUILayout.Button("<b>Online Documentation can be found at https://docs.piveclabs.com</b>", style);

			var rect = GUILayoutUtility.GetLastRect();
			EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
			if (bClicked)
				Application.OpenURL(url);
		}

	}
}