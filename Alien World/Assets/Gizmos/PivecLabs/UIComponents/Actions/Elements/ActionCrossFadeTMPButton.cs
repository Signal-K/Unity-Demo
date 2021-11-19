namespace GameCreator.UIComponents
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.UI;
	using GameCreator.Core;
	using GameCreator.Variables;
	using TMPro;
	
#if UNITY_EDITOR
    using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ActionCrossFadeTMPButton : IAction
	{

		public GameObject canvasButton;
   
		private Image image;
		private TextMeshProUGUI text;
		private ButtonActions button;

		private  Color curColor;

		[Range(0.0f, 2.0f)]
		public float duration = 0.5f;


		public NumberProperty alpha = new NumberProperty(0.0f);

		
        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	       
            return false;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
           
	        text = canvasButton.GetComponentInChildren<TextMeshProUGUI>();
	        image = canvasButton.GetComponent<Image>();
	        button = canvasButton.GetComponent<ButtonActions>();
	        canvasButton.SetActive(true);

	        
	        if (button != null)
	        {
	        
	        if (text != null)
	        {
		        float targetAlpha = alpha.GetValue(target);

		    
		        float currentAlpha = text.color.a;
		        float startTime = Time.unscaledTime;
		
		        text.CrossFadeAlpha(targetAlpha, duration, false);
			       
		        
	        }
	        if (image != null)
	        {
		        float targetAlpha = alpha.GetValue(target);

		    
			        float currentAlpha = image.color.a;
			        float startTime = Time.unscaledTime;
		
		        image.CrossFadeAlpha(targetAlpha, duration, false);
			       
		        
	        }
		        yield return new WaitForSeconds(duration);
		        
		        float targetA = alpha.GetValue(target);

		        if(targetA == 0) 
		        {
		        	button.interactable = false;
		        	canvasButton.SetActive(false);
		        }
		        
		        else if(targetA == 1) 
		        {
		        	button.interactable = true;
			        canvasButton.SetActive(true);
		        }
		        
		        
	       
        	}
        	
        	
	        yield return 0;
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "UI/Elements/Crossfade TMP Button";
		private const string NODE_TITLE = "Crossfade {0} TMP Button";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

 
        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spcanvas;
		private SerializedProperty spDuration;
		private SerializedProperty spAlpha;

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
			this.spDuration = this.serializedObject.FindProperty("duration");
			this.spAlpha = this.serializedObject.FindProperty("alpha");
    
        }

        protected override void OnDisableEditorChild ()
		{
	
            this.spcanvas = null;
			this.spDuration = null;
			this.spAlpha = null;
    
        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.PropertyField(this.spcanvas, new GUIContent("GC Button"));
              EditorGUILayout.Space();
     
 			EditorGUILayout.PropertyField(this.spDuration);
			EditorGUILayout.PropertyField(this.spAlpha);
			EditorGUILayout.Space();
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
