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

	[AddComponentMenu("")]
	public class ActionuGUIChangeTextColor : IAction
	{
    
        public GameObject textObject;
        private Text textdata;

    
        public ColorProperty textcolor = new ColorProperty(Color.white);
      


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

              
             textdata.gameObject.SetActive(true);
            yield return 0;
        }


      
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

        public static new string NAME = "UI/uGUI/Change Character Colour";
		private const string NODE_TITLE = "Change Character Colour";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptextmesh;
        private SerializedProperty spColortext;
     
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{

             return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.sptextmesh = this.serializedObject.FindProperty("textObject");
             this.spColortext = this.serializedObject.FindProperty("textcolor");
       }

        protected override void OnDisableEditorChild ()
		{
			this.sptextmesh = null;
              this.spColortext = null;
          }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.sptextmesh, new GUIContent("TextObject"));
            EditorGUILayout.Space();

         
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(new GUIContent("Update Properties"));
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(this.spColortext, new GUIContent("Text colour"));
      
            EditorGUI.indentLevel--;
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
