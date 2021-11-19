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
	public class ActionToggleButton : IAction
	{

		public GameObject canvasButton;
   
		private ButtonActions button;

		public BoolProperty interact = new BoolProperty(true);

		
        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	       
            return false;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
           
	        button = canvasButton.GetComponent<ButtonActions>();

	        if (canvasButton != null)
	        {
	  

		        if(interact.GetValue(target) == false) 
		        {
		        	button.interactable = false;
		        	
		        }
		        
		        else if(interact.GetValue(target) == true) 
		        {
		        	button.interactable = true;
			        
		        }
		        
		        
	       
        	}
        	
        	
	        yield return 0;
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "UI/Elements/Toggle Button";
		private const string NODE_TITLE = "Toggle {0} Button";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spcanvas;
		private SerializedProperty spinteract;

        // INSPECTOR METHODS: ---------------------------------------------------------------------




        public override string GetNodeTitle()
		{

			if 	(canvasButton != null)		
				return string.Format(NODE_TITLE, canvasButton.name);
			else 	
				return string.Format(NODE_TITLE, "");
		
		}

		protected override void OnEnableEditorChild ()
		{
	
			this.spcanvas = this.serializedObject.FindProperty("canvasButton");
			this.spinteract = this.serializedObject.FindProperty("interact");
  
        }

        protected override void OnDisableEditorChild ()
		{
	
            this.spcanvas = null;
			this.spinteract = null;
   
        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.PropertyField(this.spcanvas, new GUIContent("UI Button"));
             EditorGUILayout.Space();
     
			EditorGUILayout.PropertyField(this.spinteract,new GUIContent("Interactable"));
			EditorGUILayout.Space();
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
