namespace GameCreator.UIComponents
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Audio;
	using UnityEngine.UI;
	using GameCreator.Core;
    using GameCreator.Localization;

    #if UNITY_EDITOR
    using UnityEditor;
    #endif

    [AddComponentMenu("")]
	public class ActionHideTooltip : IAction
    {

	    public Actions actions;

	    
        // EXECUTABLE: ----------------------------------------------------------------------------

	    public override bool InstantExecute(GameObject target, IAction[] actions, int index)
	    {
		    var references = this.actions.gameObject.GetComponents<ActionShowTooltip>();

		    foreach (var reference in references)

		    {
			    reference.Stop();
		    }
	          
		 
		    return true;
        }

   
#if UNITY_EDITOR
	    public static new string NAME = "UI/Elements/Hide Tooltip Message";
	    private const string NODE_TITLE = "Hide Tooltip";
	    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

	    private SerializedProperty spaction;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {
            return string.Format(
	            NODE_TITLE
                
            );
        }

        protected override void OnEnableEditorChild()
        {
	        this.spaction = this.serializedObject.FindProperty("actions");
 
        }

        public override void OnInspectorGUI()
        {
	        this.serializedObject.Update();
	        EditorGUILayout.PropertyField(this.spaction, new GUIContent("Tooltip Action to Hide"));


            EditorGUILayout.Space();

            this.serializedObject.ApplyModifiedProperties();
        }
#endif
    }
}
