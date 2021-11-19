namespace GameCreator.UIComponents
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
	using UnityEngine.UI;
	using GameCreator.Variables;

	#if UNITY_EDITOR
	using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ActionUIAnalogClock : IAction
	{
    
        public GameObject textObject;
        public bool seconds;
		public Transform clockHourHandTransform;
		public Transform clockMinuteHandTransform;
		public Transform clockSecondHandTransform;
		private float day;
		private System.DateTime time;
		private string[] timeArray;
		private float minute;
		private float second;
		private float hour;

		public bool fromVariable;
		[VariableFilter(Variable.DataType.String)]
		public VariableProperty targetVariable = new VariableProperty(Variable.VarType.GlobalVariable);

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{
			if (fromVariable == true)
			{
				string hms = this.targetVariable.ToStringValue(target);;
				timeArray = hms.Split(':');
			
			}

            CancelInvoke("systemTime");
            if (seconds == true)
			{
                InvokeRepeating("systemTime", 0, 1.0f);
            }
            else
			{
                InvokeRepeating("systemTime", 0, 60.0f);
            }
           

           

            return true;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            return base.Execute(target, actions, index);
        }

        private void systemTime()
        {
	        if (fromVariable == true)
	        {
	       
	        	second = float.Parse(timeArray[2]);
		        minute = float.Parse(timeArray[1]);
		        hour = float.Parse(timeArray[0]);
	        }
	        else
	        {
	        	
		        time = System.DateTime.Now;
		        minute = (float) time.Minute;
		        second = (float) time.Second;
		        hour = (float) time.Hour;
	        }
	        
	        if (seconds == true)
	        {
		        
		        float secondAngle = -360 *(second/60);
		        clockSecondHandTransform.localRotation = Quaternion.Euler(0,0,secondAngle);

	        }

	        
	        float minuteAngle = -360 *(minute/60);
	        clockMinuteHandTransform.localRotation = Quaternion.Euler(0,0,minuteAngle);

	        
	        float hourAngle = -360 *(hour/12);
	        clockHourHandTransform.localRotation = Quaternion.Euler(0,0,hourAngle);
  	      

        }

        public void StopRepeating()
        {
            CancelInvoke("systemTime");
        }
            // +--------------------------------------------------------------------------------------+
            // | EDITOR                                                                               |
            // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "UI/Time/UI Analog Clock";
		private const string NODE_TITLE = "Display System Time - Analog";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spclockHourHandTransform;
		private SerializedProperty spclockMinuteHandTransform;
		private SerializedProperty spclockSecondHandTransform;
		private SerializedProperty spseconds;
		private SerializedProperty spvarbool;
		private SerializedProperty spvariable;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.spclockHourHandTransform = this.serializedObject.FindProperty("clockHourHandTransform");
			this.spclockMinuteHandTransform = this.serializedObject.FindProperty("clockMinuteHandTransform");
			this.spclockSecondHandTransform = this.serializedObject.FindProperty("clockSecondHandTransform");
	 		this.spseconds = this.serializedObject.FindProperty("seconds");
 			this.spvarbool = this.serializedObject.FindProperty("fromVariable");
			this.spvariable = this.serializedObject.FindProperty("targetVariable");
 	}

        protected override void OnDisableEditorChild ()
		{
			this.spclockHourHandTransform = null;
			this.spclockMinuteHandTransform = null;
			this.spclockSecondHandTransform = null;
 			this.spseconds = null;
			this.spvarbool = null;
			this.spvariable = null;
		}

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.PropertyField(this.spvarbool, new GUIContent("Get Time from Variable"));
			if (fromVariable)
			{
				EditorGUILayout.PropertyField(this.spvariable, new GUIContent("String Variable"));

			}
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spclockHourHandTransform, new GUIContent("Hour Hand"));
			EditorGUILayout.PropertyField(this.spclockMinuteHandTransform, new GUIContent("Minute Hand"));
			EditorGUILayout.PropertyField(this.spclockSecondHandTransform, new GUIContent("Second Hand"));
 			EditorGUI.indentLevel++;
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(this.spseconds, new GUIContent("show seconds"));
			EditorGUI.indentLevel--;
            

            this.serializedObject.ApplyModifiedProperties();
		}

#endif
        }
}
