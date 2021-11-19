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
	public class ActionRotateCanvasOut : IAction
	{

        public GameObject canvasPanel;
        public RectTransform animpanel;

        public enum ALIGN
        {
            Top,
            Bottom,
            Left,
            Right
        }
        public ALIGN alignment = ALIGN.Top;

        public NumberProperty AnimateDuration = new NumberProperty(1.0f);
        public Easing.EaseType easing = Easing.EaseType.QuadInOut;

        public Vector3 animStartPosition;
        public Vector3 AnimInPosition;
        public Vector3 AnimOutPositionTop;
        public Vector3 AnimOutPositionBottom;
        public Vector3 AnimOutPositionLeft;
        public Vector3 AnimOutPositionRight;

        private Vector3 rotation;
        private Quaternion rotation2;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
          
         
            return false;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
           
            animpanel = canvasPanel.GetComponent<RectTransform>();
            Transform targetTrans = this.animpanel.transform;
       
            AnimInPosition = new Vector3(0, 0, 0);

            animStartPosition = animpanel.localPosition;

            AnimOutPositionTop = new Vector3(0, animpanel.rect.height, 0);
            AnimOutPositionBottom = new Vector3(0, -animpanel.rect.height, 0);
            AnimOutPositionLeft = new Vector3(-animpanel.rect.width, 0, 0);
            AnimOutPositionRight = new Vector3(animpanel.rect.width, 0, 0);

            float vMoveSpeed = AnimateDuration.GetValue(target);
            if (vMoveSpeed < 0.2f) vMoveSpeed = 0.2f;

            float initTime = Time.unscaledTime;
            Quaternion rotation1 = Quaternion.identity;
  

            switch (this.alignment)
                {
                    case ALIGN.Top:
                    animpanel.pivot = new Vector2(0f, 1f);
                    rotation = new Vector3(90f, 0.0f, 0f);
                    rotation2 = Quaternion.Euler(rotation);

                    while (Time.unscaledTime - initTime < vMoveSpeed)
                    {
                        if (animpanel == null) break;
                        float t = (Time.unscaledTime - initTime) / vMoveSpeed;
                        float easeValue = Easing.GetEase(easing, 0.0f, 1.0f, t);

                        animpanel.localRotation = Quaternion.Lerp(
                           rotation1,
                          rotation2,
                         easeValue


                      );

                        yield return null;
                        }

                        break;
                    case ALIGN.Bottom:
                    animpanel.pivot = new Vector2(1f, 0f);
                    rotation = new Vector3(-90f, 0f, 0f);
                    rotation2 = Quaternion.Euler(rotation);

                    while (Time.unscaledTime - initTime < vMoveSpeed)
                    {
                        if (animpanel == null) break;
                        float t = (Time.unscaledTime - initTime) / vMoveSpeed;
                        float easeValue = Easing.GetEase(easing, 0.0f, 1.0f, t);

                        animpanel.localRotation = Quaternion.Lerp(
                           rotation1,
                          rotation2,
                         easeValue


                      );

                        yield return null;
                        }
                        break;
                    case ALIGN.Left:
                    animpanel.pivot = new Vector2(0.0f, 0.5f);
                    rotation = new Vector3(0f, 90f, 0f);
                    rotation2 = Quaternion.Euler(rotation);

                    while (Time.unscaledTime - initTime < vMoveSpeed)
                        {
                            if (animpanel == null) break;
                            float t = (Time.unscaledTime - initTime) / vMoveSpeed;
                            float easeValue = Easing.GetEase(easing, 0.0f, 1.0f, t);

                          animpanel.localRotation = Quaternion.Lerp(
                             rotation1,
                            rotation2,
                           easeValue

           
                        );


                            yield return null;
                        }

                        break;
                    case ALIGN.Right:
                    animpanel.pivot = new Vector2(1f, 1f);
                    rotation = new Vector3(0f, -90f, 0f);
                    rotation2 = Quaternion.Euler(rotation);

                    while (Time.unscaledTime - initTime < vMoveSpeed)
                        {
                            if (animpanel == null) break;
                            float t = (Time.unscaledTime - initTime) / vMoveSpeed;
                            float easeValue = Easing.GetEase(easing, 0.0f, 1.0f, t);

                        animpanel.localRotation = Quaternion.Lerp(
                                rotation1,
                               rotation2,
                              easeValue

                           );

                            yield return null;
                        }

                        break;
                     }


        



            yield return 0;
        }


      
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

        public static new string NAME = "UI/Canvas/Rotate Canvas Panel Out";
		private const string NODE_TITLE = "Rotate Canvas Panel Out";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spcanvas;
        private SerializedProperty spAlignment;
        private SerializedProperty spAnimstate;
        private SerializedProperty spAnimDuration;
 
        // INSPECTOR METHODS: ---------------------------------------------------------------------




        public override string GetNodeTitle()
		{

             return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
	
            this.spcanvas = this.serializedObject.FindProperty("canvasPanel");
            this.spAlignment = this.serializedObject.FindProperty("alignment");
            this.spAnimstate = this.serializedObject.FindProperty("animState");
            this.spAnimDuration = this.serializedObject.FindProperty("AnimateDuration");
    
        }

        protected override void OnDisableEditorChild ()
		{
	
            this.spcanvas = null;
            this.spAlignment = null;
            this.spAnimstate = null;
            this.spAnimDuration = null;
     
        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

            EditorGUILayout.LabelField("Screen Space - Camera", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(this.spcanvas, new GUIContent("Canvas Panel"));
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(this.spAlignment, new GUIContent("Rotate out from"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spAnimDuration, new GUIContent("Duration"));
            EditorGUILayout.Space();
            EditorGUI.indentLevel--;
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
