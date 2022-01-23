namespace GameCreator.UIComponents
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;
    using GameCreator.Core;
    using GameCreator.Variables;
    using TMPro;


#if UNITY_EDITOR
    using UnityEditor;
	#endif

	[AddComponentMenu("")]
    public class ActionTMPUINumVariableToText : IAction
    {
    
	    [VariableFilter(Variable.DataType.Number)]
        public VariableProperty targetVariable = new VariableProperty(Variable.VarType.GlobalVariable);
        public TMP_Text tmptext;
        public string content = "{0}";


        // EXECUTABLE: ----------------------------------------------------------------------------
        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            if (this.tmptext != null)
            {
                  this.tmptext.text = string.Format(
                      this.content,
                     new string[] { this.targetVariable.ToStringValue(target) }
                  );


            }




            return true;
        }


      
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

	    public static new string NAME = "UI/TMP/Number Variable To TMP_Text";
	    private const string NODE_TITLE = "Number Variable To TMP_Text";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptext;
        private SerializedProperty sptargetVariable;
   
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {
            return string.Format(NODE_TITLE);
        }

        protected override void OnEnableEditorChild()
        {
            this.sptext = this.serializedObject.FindProperty("tmptext");
            this.sptargetVariable = this.serializedObject.FindProperty("targetVariable");
        }

        protected override void OnDisableEditorChild()
        {
            this.sptext = null;
            this.sptargetVariable = null;
         }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            EditorGUILayout.PropertyField(this.sptargetVariable, new GUIContent("Target Variable"));

            EditorGUILayout.Space();
             EditorGUILayout.PropertyField(this.sptext, new GUIContent("UI TMP_Text Field"));



            EditorGUILayout.Space();

            this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}