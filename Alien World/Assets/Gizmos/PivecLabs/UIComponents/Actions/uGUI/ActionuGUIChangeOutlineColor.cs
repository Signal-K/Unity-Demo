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
	public class ActionuGUIChangeOutlineColor : IAction
	{
    
        public GameObject textObject;
        private Text textdata;

  
         public ColorProperty outlinecolor = new ColorProperty(Color.black);

  


        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
          
         
            return false;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            if (textObject.GetComponent<Outline>() == null)
            {
                Outline outline = textObject.AddComponent<Outline>();

                outline.effectColor = outlinecolor.GetValue(target);
            }

            else
            {
                Outline outline = textObject.GetComponent<Outline>();
                outline.effectColor = outlinecolor.GetValue(target);
            }

            yield return 0;
        }


      
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

        public static new string NAME = "UI/uGUI/Change Text Outline Colour";
		private const string NODE_TITLE = "Change Text Outline Colour";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptextmesh;
         private SerializedProperty spColoroutline;
       // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{

             return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.sptextmesh = this.serializedObject.FindProperty("textObject");
            this.spColoroutline = this.serializedObject.FindProperty("outlinecolor");
        }

        protected override void OnDisableEditorChild ()
		{
			this.sptextmesh = null;
            this.spColoroutline = null;
     }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.sptextmesh, new GUIContent("TextMeshPro Object"));
            EditorGUILayout.Space();

             EditorGUILayout.Space();
            EditorGUILayout.LabelField(new GUIContent("Update Properties"));
            EditorGUI.indentLevel++;
               EditorGUILayout.PropertyField(this.spColoroutline, new GUIContent("Outline colour"));
      
            EditorGUI.indentLevel--;
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
