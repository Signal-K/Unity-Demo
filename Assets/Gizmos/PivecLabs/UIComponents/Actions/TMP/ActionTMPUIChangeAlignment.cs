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
	public class ActionTMPUIChangeAlignment : IAction
	{
    
        public GameObject textObject;
        private TextMeshProUGUI textdata;

     
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
            textdata = textObject.GetComponent<TextMeshProUGUI>();

            textdata.gameObject.SetActive(false);

      
                        switch (this.alignment)
                        {
                            case ALIGN.Left:
                                textdata.alignment = TextAlignmentOptions.Left;
                                break;
                            case ALIGN.Center:
                                textdata.alignment = TextAlignmentOptions.Center;
                                break;
                            case ALIGN.Right:
                                textdata.alignment = TextAlignmentOptions.Right;
                                break;
                            case ALIGN.Justified:
                                textdata.alignment = TextAlignmentOptions.Justified;
                                break;
                        }


            
                       

            textdata.ForceMeshUpdate();
            textdata.gameObject.SetActive(true);
            yield return 0;
        }


      
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

        public static new string NAME = "UI/TMP/Change Text Alignment";
		private const string NODE_TITLE = "Change Text Alignment";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";
        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptextmesh;
        private SerializedProperty spAlignment;
   
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{

             return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.sptextmesh = this.serializedObject.FindProperty("textObject");
           
            this.spAlignment = this.serializedObject.FindProperty("alignment");
        }

        protected override void OnDisableEditorChild ()
		{
			this.sptextmesh = null;
           
            this.spAlignment = null;
            
        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.sptextmesh, new GUIContent("TextMeshPro Object"));
            EditorGUILayout.Space();

           
        
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(new GUIContent("Update Properties"));
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(this.spAlignment, new GUIContent("Text alignment"));
            
      
            EditorGUI.indentLevel--;
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
