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
	public class ActionuGUIChangeContent : IAction
	{
    
        public GameObject textObject;
        private Text textdata;

     

        public string content = "";

    


        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
          
         
            return false;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            textdata = textObject.GetComponent<Text>();

            textdata.gameObject.SetActive(false);

           
            

                            textdata.text = this.content;


             
            textdata.gameObject.SetActive(true);
            yield return 0;
        }


      
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

        public static new string NAME = "UI/uGUI/Change Content";
		private const string NODE_TITLE = "Change Content";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptextmesh;
         private SerializedProperty spContent;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{

             return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.sptextmesh = this.serializedObject.FindProperty("textObject");
             this.spContent = this.serializedObject.FindProperty("content");
        }

        protected override void OnDisableEditorChild ()
		{
			this.sptextmesh = null;
              this.spContent = null;
        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.sptextmesh, new GUIContent("Text Object"));
            EditorGUILayout.Space();

     
            EditorGUILayout.PropertyField(this.spContent, new GUIContent("New Text Content"));
        
            EditorGUILayout.Space();
           
      
          
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
