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
	public class ActionCrossFadePanel : IAction
	{

		public GameObject canvasPanel;
   
		private Image image;
		private Text text;
		private Dropdown dropdown;
		private SliderVariable slider;
		private ToggleVariable toggle;
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
	            
	        canvasPanel.SetActive(true);

	           
	        text = canvasPanel.GetComponentInChildren<Text>();
	        image = canvasPanel.GetComponent<Image>();
	           
	        dropdown = canvasPanel.GetComponent<Dropdown>();
	          
	        slider = canvasPanel.GetComponent<SliderVariable>();
	        
	        toggle = canvasPanel.GetComponent<ToggleVariable>();
	    
	        button = canvasPanel.GetComponent<ButtonActions>();
	   
	   
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
			        if (dropdown != null)
			        {	
			        	dropdown.interactable = false;
				        dropdown.enabled = false;
			        }
			        if (slider != null)
			        {	
			        	slider.interactable = false;
				        slider.enabled = false;
			        }
			        if (toggle != null)
			        {	
			        	toggle.interactable = false;
				        toggle.enabled = false;
			        }
			        
			        if (button != null)
			        {	
			        	button.interactable = false;
			        }
			        
			        canvasPanel.SetActive(false);
		        }
		        
		        else if(targetA > 0) 
		        {
			        if (dropdown != null)
			        {	
			        	dropdown.interactable = true;
				        dropdown.enabled = true;
			        }
			        if (slider != null)
			        {	
			        	slider.interactable = true;
				        slider.enabled = true;
			        }
			        if (toggle != null)
			        {	
			        	toggle.interactable = true;
				        toggle.enabled = true;
			        }
			        if (button != null)
			        {	
			        	button.interactable = true;
			        }
			        
			        canvasPanel.SetActive(true);

		        }
		        
    	 
        	
        	
	        yield return 0;
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "UI/Elements/Crossfade Panel";
		private const string NODE_TITLE = "Crossfade {0} Panel";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

 
        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spcanvas;
		private SerializedProperty spDuration;
		private SerializedProperty spAlpha;

        // INSPECTOR METHODS: ---------------------------------------------------------------------




        public override string GetNodeTitle()
		{

			if 	(canvasPanel != null)		
				return string.Format(NODE_TITLE, canvasPanel.name);
			else 	
				return string.Format(NODE_TITLE, "");
		}

		protected override void OnEnableEditorChild ()
		{
	
			this.spcanvas = this.serializedObject.FindProperty("canvasPanel");
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
			EditorGUILayout.PropertyField(this.spcanvas, new GUIContent("UI Panel Element"));
              EditorGUILayout.Space();
     
 			EditorGUILayout.PropertyField(this.spDuration);
			EditorGUILayout.PropertyField(this.spAlpha);
			EditorGUILayout.Space();
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
