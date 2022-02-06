namespace GameCreator.UIComponents
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
    using GameCreator.Variables;

#if UNITY_EDITOR
    using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ActionHideElement : IAction
	{

		public GameObject canvasElement;
		public RectTransform animElement;
       public enum ALIGN
        {
            Top,
            Bottom,
            Left,
            Right
        }
        public ALIGN alignment = ALIGN.Top;

       
        public Vector3 AnimOutPositionTop;
        public Vector3 AnimOutPositionBottom;
        public Vector3 AnimOutPositionLeft;
        public Vector3 AnimOutPositionRight;

		public StringProperty originalPos; 
		
        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
          
         
            return false;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
           
	        animElement = canvasElement.GetComponent<RectTransform>();
	        VariablesManager.SetLocal(canvasElement, "originalPosition", animElement.localPosition, true);
	       
	      
	        AnimOutPositionTop = new Vector3(animElement.localPosition.x, Screen.height, animElement.localPosition.z);
	        AnimOutPositionBottom = new Vector3(animElement.localPosition.x, -Screen.height, animElement.localPosition.z);
	        AnimOutPositionLeft = new Vector3(-Screen.width, animElement.localPosition.y, animElement.localPosition.z);
	        AnimOutPositionRight = new Vector3(Screen.width, animElement.localPosition.y, animElement.localPosition.z);

      		{

			
                switch (this.alignment)
                {
                    case ALIGN.Top:

					animElement.localPosition = AnimOutPositionTop;

						break;
                    case ALIGN.Bottom:

					animElement.localPosition = AnimOutPositionBottom;

					break;
                    case ALIGN.Left:


					animElement.localPosition = AnimOutPositionLeft;



					break;
                    case ALIGN.Right:

					animElement.localPosition = AnimOutPositionRight;



					break;
                     }

			}


			yield return 0;
        }



		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "UI/Elements/Hide Element";
		private const string NODE_TITLE = "Hide {0}";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spcanvas;
         private SerializedProperty spAlignment;
 
        // INSPECTOR METHODS: ---------------------------------------------------------------------




        public override string GetNodeTitle()
		{
			if 	(canvasElement != null)		
				return string.Format(NODE_TITLE, canvasElement.name);
			else 	
				return string.Format(NODE_TITLE, "");
		}

		protected override void OnEnableEditorChild ()
		{
	
			this.spcanvas = this.serializedObject.FindProperty("canvasElement");
                 this.spAlignment = this.serializedObject.FindProperty("alignment");
    
        }

        protected override void OnDisableEditorChild ()
		{
	
            this.spcanvas = null;
            this.spAlignment = null;
     
        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
     
			EditorGUILayout.PropertyField(this.spcanvas, new GUIContent("Canvas Element"));
            EditorGUILayout.Space();
         EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(this.spAlignment, new GUIContent("Hide to"));
            EditorGUILayout.Space();
            EditorGUI.indentLevel--;
            this.serializedObject.ApplyModifiedProperties();
		}

#endif
	}
}
