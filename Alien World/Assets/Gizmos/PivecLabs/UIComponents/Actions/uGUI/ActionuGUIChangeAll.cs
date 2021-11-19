namespace GameCreator.UIComponents
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
    using GameCreator.Variables;
	using UnityEngine.UI;

#if UNITY_EDITOR
    using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ActionuGUIChangeAll : IAction
	{
    
        public GameObject textObject;
        private Text textdata;

        public Font font;

        public ColorProperty textcolor = new ColorProperty(Color.white);
        public ColorProperty outlinecolor = new ColorProperty(Color.black);

        public NumberProperty textsize = new NumberProperty(6f);
        public NumberProperty outlinewidth = new NumberProperty(0f);

        public string content = "";

        public enum ALIGN
        {
            Left,
            Center,
            Right,
            Justified
        }
        public ALIGN alignment = ALIGN.Left;


        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
          
         
            return false;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            textdata = textObject.GetComponent<Text>();

            textdata.gameObject.SetActive(false);

           textdata.color = textcolor.GetValue(target);

	        if (textObject.GetComponent<Outline>() == null)
	        {
		        Outline outline = textObject.AddComponent<Outline>();

		        outline.effectColor = outlinecolor.GetValue(target);
		        Vector2 outlineWdith = new Vector2(outlinewidth.GetValue(target), -outlinewidth.GetValue(target));
		        outline.effectDistance = outlineWdith;
	
	        }

	        else
	        {
		        Outline outline = textObject.GetComponent<Outline>();
		        outline.effectColor = outlinecolor.GetValue(target);
		        Vector2 outlineWdith = new Vector2(outlinewidth.GetValue(target), -outlinewidth.GetValue(target));
		        outline.effectDistance = outlineWdith;
	
	        }

	        textdata.font = font;
	        textdata.fontSize = (int)textsize.GetValue(target);

			textdata.text = this.content;

	        switch (this.alignment)
	        {
	        case ALIGN.Left:
		        textdata.alignment = TextAnchor.MiddleLeft;
		        break;
	        case ALIGN.Center:
		        textdata.alignment = TextAnchor.MiddleCenter;
		        break;
	        case ALIGN.Right:
		        textdata.alignment = TextAnchor.MiddleRight;
		        break;
                            
	        }

     
            textdata.gameObject.SetActive(true);
            yield return 0;
        }


      
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

        public static new string NAME = "UI/uGUI/Change All Properties";
		private const string NODE_TITLE = "Change All Properties";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptextmesh;
        private SerializedProperty spfont;
        private SerializedProperty spColortext;
        private SerializedProperty spColoroutline;
        private SerializedProperty spColortextsize;
        private SerializedProperty spColoroutlinesize;
        private SerializedProperty spAlignment;
        private SerializedProperty spContent;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{

             return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.sptextmesh = this.serializedObject.FindProperty("textObject");
            this.spfont = this.serializedObject.FindProperty("font");
            this.spColortext = this.serializedObject.FindProperty("textcolor");
            this.spColoroutline = this.serializedObject.FindProperty("outlinecolor");
            this.spColortextsize = this.serializedObject.FindProperty("textsize");
            this.spColoroutlinesize = this.serializedObject.FindProperty("outlinewidth");
            this.spContent = this.serializedObject.FindProperty("content");
            this.spAlignment = this.serializedObject.FindProperty("alignment");
        }

        protected override void OnDisableEditorChild ()
		{
			this.sptextmesh = null;
            this.spfont = null;
            this.spColortext = null;
            this.spColoroutline = null;
            this.spColortextsize = null;
            this.spColoroutlinesize = null;
            this.spAlignment = null;
            this.spContent = null;
        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.sptextmesh, new GUIContent("Text Object"));
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(this.spfont, new GUIContent("New Font"));
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(this.spContent, new GUIContent("New Text Content"));
        
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(new GUIContent("Update Properties"));
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(this.spAlignment, new GUIContent("Text alignment"));
            EditorGUILayout.PropertyField(this.spColortextsize, new GUIContent("Text size"));
            EditorGUILayout.PropertyField(this.spColortext, new GUIContent("Text colour"));
            EditorGUILayout.PropertyField(this.spColoroutlinesize, new GUIContent("Outline size"));
            EditorGUILayout.PropertyField(this.spColoroutline, new GUIContent("Outline colour"));
      
            EditorGUI.indentLevel--;
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
