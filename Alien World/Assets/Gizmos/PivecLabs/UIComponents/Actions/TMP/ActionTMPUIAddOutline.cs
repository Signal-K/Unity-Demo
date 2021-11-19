namespace GameCreator.UIComponents
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
    using GameCreator.Variables;
    using TMPro;

#if UNITY_EDITOR
    using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ActionTMPUIAddOutline : IAction
	{
    
        public GameObject textObject;
        private TextMeshProUGUI textdata;

           public NumberProperty outlinewidth = new NumberProperty(0f);

    


        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
          
         
            return false;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            textdata = textObject.GetComponent<TextMeshProUGUI>();

            textdata.gameObject.SetActive(false);

           
             
                               Material mat = textdata.fontSharedMaterial;
                                mat.shaderKeywords = new string[] { "OUTLINE_ON" };

           

                            textdata.outlineWidth = outlinewidth.GetValue(target);

            

                  
                       

            textdata.ForceMeshUpdate();
            textdata.gameObject.SetActive(true);
            yield return 0;
        }


      
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

        public static new string NAME = "UI/TMP/Add Outline";
		private const string NODE_TITLE = "Add Outline";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptextmesh;
        private SerializedProperty spfont;
        private SerializedProperty spColoroutlinesize;
  
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{

             return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.sptextmesh = this.serializedObject.FindProperty("textObject");
            this.spColoroutlinesize = this.serializedObject.FindProperty("outlinewidth");
        }

        protected override void OnDisableEditorChild ()
		{
			this.sptextmesh = null;
               this.spColoroutlinesize = null;
         }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.sptextmesh, new GUIContent("TextMeshPro Object"));
            EditorGUILayout.Space();

              EditorGUILayout.LabelField(new GUIContent("Update Properties"));
            EditorGUI.indentLevel++;
             EditorGUILayout.PropertyField(this.spColoroutlinesize, new GUIContent("Outline size"));
      
            EditorGUI.indentLevel--;
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
