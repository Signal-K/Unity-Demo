namespace GameCreator.UIComponents
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;
	using GameCreator.Core;
    using GameCreator.Core.Hooks;
    using GameCreator.Variables;

#if UNITY_EDITOR
    using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ActionFillImage : IAction
	{
        public enum FILLMETHOD
        {
            Horizontal,
            Vertical,
            Radial90,
            Radial180,
            Radial360

        }

        public enum FILLORIGINH
        {
            Left,
            Right
        }

        public enum FILLORIGINV
        {
            Bottom,
            Top
        }

        public enum FILLORIGINR90
        {
            BottomLeft,
            TopLeft,
            TopRight,
            BottomRight
        }

        public enum FILLORIGINR
        {
            Bottom,
            Left,
            Top,
            Right
        }

        public Image imagetofill;

        public FILLMETHOD fillmethod = FILLMETHOD.Horizontal;
        public FILLORIGINH filloriginh = FILLORIGINH.Left;
        public FILLORIGINV filloriginv = FILLORIGINV.Bottom;
        public FILLORIGINR90 filloriginr90 = FILLORIGINR90.BottomLeft;
        public FILLORIGINR filloriginr = FILLORIGINR.Bottom;

        public bool clockwise = true;

      //  public float fillamount = 0f;

        public NumberProperty fillamount = new NumberProperty(0.0f);
       
         private Image image;


        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {

            imagetofill.type = Image.Type.Filled;

            switch (this.fillmethod)
            {
                case FILLMETHOD.Horizontal:
                    imagetofill.fillMethod = Image.FillMethod.Horizontal;
                    this.FillOriginH();
                    break;
                case FILLMETHOD.Vertical:
                    imagetofill.fillMethod = Image.FillMethod.Vertical;
                    this.FillOriginV();
                    break;
                case FILLMETHOD.Radial90:
                    imagetofill.fillMethod = Image.FillMethod.Radial90;
                    this.FillOriginR90();
                    imagetofill.fillClockwise = clockwise;
                    break;
                case FILLMETHOD.Radial180:
                    imagetofill.fillMethod = Image.FillMethod.Radial180;
                    this.FillOriginR180();
                    imagetofill.fillClockwise = clockwise;
                    break;
                case FILLMETHOD.Radial360:
                    imagetofill.fillMethod = Image.FillMethod.Radial360;
                    this.FillOriginR360();
                    imagetofill.fillClockwise = clockwise;
                    break;
            }


      
            imagetofill.fillAmount = fillamount.GetValue(target);

            return true;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
          
            return base.Execute(target, actions, index);
        }


        private void FillOriginH()
        {
            switch (this.filloriginh)
            {
                case FILLORIGINH.Left:
                    imagetofill.fillOrigin = (int)Image.OriginHorizontal.Left;
                    break;
                case FILLORIGINH.Right:
                    imagetofill.fillOrigin = (int)Image.OriginHorizontal.Right;
                    break;


            }
        }

        private void FillOriginV()
        {
            switch (this.filloriginv)
            {
                case FILLORIGINV.Bottom:
                    imagetofill.fillOrigin = (int)Image.OriginVertical.Bottom;
                    break;
                case FILLORIGINV.Top:
                    imagetofill.fillOrigin = (int)Image.OriginVertical.Top;
                    break;


            }
        }

        private void FillOriginR90()
        {
            switch (this.filloriginr90)
            {
                case FILLORIGINR90.BottomLeft:
                    imagetofill.fillOrigin = (int)Image.Origin90.BottomLeft;
                    break;
                case FILLORIGINR90.BottomRight:
                    imagetofill.fillOrigin = (int)Image.Origin90.BottomRight;
                    break;
                case FILLORIGINR90.TopLeft:
                    imagetofill.fillOrigin = (int)Image.Origin90.TopLeft;
                    break;
                case FILLORIGINR90.TopRight:
                    imagetofill.fillOrigin = (int)Image.Origin90.TopRight;
                    break;


            }
        }

        private void FillOriginR180()
        {
            switch (this.filloriginr)
            {
                case FILLORIGINR.Bottom:
                    imagetofill.fillOrigin = (int)Image.Origin180.Bottom;
                    break;
                case FILLORIGINR.Left:
                    imagetofill.fillOrigin = (int)Image.Origin180.Left;
                    break;
                case FILLORIGINR.Right:
                    imagetofill.fillOrigin = (int)Image.Origin180.Right;
                    break;
                case FILLORIGINR.Top:
                    imagetofill.fillOrigin = (int)Image.Origin180.Top;
                    break;


            }
        }
        private void FillOriginR360()
        {
            switch (this.filloriginr)
            {
                case FILLORIGINR.Bottom:
                    imagetofill.fillOrigin = (int)Image.Origin360.Bottom;
                    break;
                case FILLORIGINR.Left:
                    imagetofill.fillOrigin = (int)Image.Origin360.Left;
                    break;
                case FILLORIGINR.Right:
                    imagetofill.fillOrigin = (int)Image.Origin360.Right;
                    break;
                case FILLORIGINR.Top:
                    imagetofill.fillOrigin = (int)Image.Origin360.Top;
                    break;


            }
        }


        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "UI/Elements/Fill an Image";
		private const string NODE_TITLE = "Fill an Image ";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spimagetofill;
        private SerializedProperty spfillMethod; 
        private SerializedProperty spfillOriginH;
        private SerializedProperty spfillOriginV;
        private SerializedProperty spfillOriginR90;
        private SerializedProperty spfillOriginR;
        private SerializedProperty spfillamount;
        private SerializedProperty spclockwise;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
            this.spimagetofill = this.serializedObject.FindProperty("imagetofill"); 
            this.spfillMethod = this.serializedObject.FindProperty("fillmethod"); 
            this.spfillOriginH = this.serializedObject.FindProperty("filloriginh");
            this.spfillOriginV = this.serializedObject.FindProperty("filloriginv");
            this.spfillOriginR = this.serializedObject.FindProperty("filloriginr");
            this.spfillOriginR90 = this.serializedObject.FindProperty("filloriginr90");
            this.spfillamount = this.serializedObject.FindProperty("fillamount");
            this.spclockwise = this.serializedObject.FindProperty("clockwise");

        }

        protected override void OnDisableEditorChild ()
		{
            this.spimagetofill = null; 
            this.spfillMethod = null; 
            this.spfillOriginH = null;
            this.spfillOriginV = null;
            this.spfillOriginR = null;
            this.spfillOriginR90 = null;
            this.spfillamount = null;
            this.spclockwise = null;
        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
            EditorGUILayout.PropertyField(this.spimagetofill, new GUIContent("Image to fill"));
            EditorGUILayout.PropertyField(this.spfillMethod, new GUIContent("Fill method")); 
    


            switch ((FILLMETHOD)this.spfillMethod.intValue)
            {
              case FILLMETHOD.Horizontal:
                    EditorGUILayout.PropertyField(this.spfillOriginH, new GUIContent("Fill origin"));
                    break;
                case FILLMETHOD.Vertical:
                    EditorGUILayout.PropertyField(this.spfillOriginV, new GUIContent("Fill origin"));
                    break;
                case FILLMETHOD.Radial90:
                    EditorGUILayout.PropertyField(this.spfillOriginR90, new GUIContent("Fill origin"));
                    EditorGUILayout.PropertyField(this.spclockwise, new GUIContent("Clockwise"));
                    break;
                case FILLMETHOD.Radial180:
                    EditorGUILayout.PropertyField(this.spfillOriginR, new GUIContent("Fill origin"));
                    EditorGUILayout.PropertyField(this.spclockwise, new GUIContent("Clockwise"));
                    break;
                case FILLMETHOD.Radial360:
                    EditorGUILayout.PropertyField(this.spfillOriginR, new GUIContent("Fill origin"));
                    EditorGUILayout.PropertyField(this.spclockwise, new GUIContent("Clockwise"));
                    break;
            }
            EditorGUILayout.PropertyField(this.spfillamount, new GUIContent("Fill amount"));
            this.serializedObject.ApplyModifiedProperties();


        }

#endif
    }
}
