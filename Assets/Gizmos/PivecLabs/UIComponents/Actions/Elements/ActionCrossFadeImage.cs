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
	public class ActionCrossFadeImage : IAction
	{

		public GameObject canvasImage;
   
		private Image image;
		private  Color curColor;

		[Range(0.0f, 5.0f)]
		public float duration = 0.5f;


		public NumberProperty alpha = new NumberProperty(0.0f);

		
        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	       
            return false;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
           
	        image = canvasImage.GetComponent<Image>();
         
	        if (image != null)
	        {
		        float targetAlpha = alpha.GetValue(target);

		    
			        float currentAlpha = image.color.a;
			        float startTime = Time.unscaledTime;
		
		        image.CrossFadeAlpha(targetAlpha, duration, false);
			       
		  
	        }

	        yield return 0;
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "UI/Elements/Crossfade Image";
		private const string NODE_TITLE = "Crossfade {0} Image";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

 
        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spcanvas;
		private SerializedProperty spDuration;
		private SerializedProperty spAlpha;

        // INSPECTOR METHODS: ---------------------------------------------------------------------




        public override string GetNodeTitle()
		{

			if 	(canvasImage != null)		
				return string.Format(NODE_TITLE, canvasImage.name);
			else 	
				return string.Format(NODE_TITLE, "");
		}

		protected override void OnEnableEditorChild ()
		{
	
			this.spcanvas = this.serializedObject.FindProperty("canvasImage");
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
			EditorGUILayout.PropertyField(this.spcanvas, new GUIContent("UI Image"));
              EditorGUILayout.Space();
     
 			EditorGUILayout.PropertyField(this.spDuration);
			EditorGUILayout.PropertyField(this.spAlpha);
			EditorGUILayout.Space();
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
