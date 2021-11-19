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
	public class ActionHideCanvas : IAction
	{

        public GameObject canvasPanel;
        public RectTransform animpanel;
		public bool hide = true;
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

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
          
         
            return false;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
           
            animpanel = canvasPanel.GetComponent<RectTransform>();

	        AnimOutPositionTop = new Vector3(0, animpanel.rect.height, 0);
            AnimOutPositionBottom = new Vector3(0, -animpanel.rect.height, 0);
            AnimOutPositionLeft = new Vector3(-animpanel.rect.width, 0, 0);
            AnimOutPositionRight = new Vector3(animpanel.rect.width, 0, 0);

            if (hide == false)
			{
				animpanel.localPosition = new Vector3(0, 0, 0);
			}
			else
			{

			
                switch (this.alignment)
                {
                    case ALIGN.Top:

					animpanel.localPosition = AnimOutPositionTop;

						break;
                    case ALIGN.Bottom:

					animpanel.localPosition = AnimOutPositionBottom;

					break;
                    case ALIGN.Left:


					animpanel.localPosition = AnimOutPositionLeft;



					break;
                    case ALIGN.Right:

					animpanel.localPosition = AnimOutPositionRight;



					break;
                     }

			}


			yield return 0;
        }



		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

        public static new string NAME = "UI/Canvas/Hide Canvas Panel";
		private const string NODE_TITLE = "Hide Canvas Panel";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spcanvas;
        private SerializedProperty sphide;
        private SerializedProperty spAlignment;
 
        // INSPECTOR METHODS: ---------------------------------------------------------------------




        public override string GetNodeTitle()
		{

             return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
	
            this.spcanvas = this.serializedObject.FindProperty("canvasPanel");
            this.sphide = this.serializedObject.FindProperty("hide");
            this.spAlignment = this.serializedObject.FindProperty("alignment");
    
        }

        protected override void OnDisableEditorChild ()
		{
	
            this.spcanvas = null;
            this.sphide = null;
            this.spAlignment = null;
     
        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
            EditorGUILayout.LabelField("Screen Space - Overlay", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(this.spcanvas, new GUIContent("Canvas Panel"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.sphide, new GUIContent("Hide Panel"));
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
