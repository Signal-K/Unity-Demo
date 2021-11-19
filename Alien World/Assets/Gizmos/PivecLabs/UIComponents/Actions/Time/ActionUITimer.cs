namespace GameCreator.UIComponents
{
	using System;
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
	public class ActionUITimer : IAction
	{
        public GameObject textObject;
		private Text textdata;


        public enum RESULT
        {
	        Nothing,
	        Action,
            Condition
        }
        public RESULT timerResult = RESULT.Action;

        public bool countdown;
        public bool countup;
		public bool leadingZero;

        public NumberProperty InitialtimerValue = new NumberProperty(0.0f);
        public NumberProperty TotaltimerValue = new NumberProperty(10.0f);
        public Actions actionToCall;
        public Conditions conditionToCall;

        private float timervalue;
		private float totaltime;

        // EXECUTABLE: ----------------------------------------------------------------------------

        

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
	        textdata = textObject.GetComponent<Text>();
            timervalue = TotaltimerValue.GetValue(target);

            if (countdown == false)
            {
                timervalue = 1;
            }
            CancelInvoke("Timer"); 

            InvokeRepeating("Timer", InitialtimerValue.GetValue(target), 1.0f); 

	        totaltime = ((TotaltimerValue.GetValue(target) + InitialtimerValue.GetValue(target))*10);
	        
	        while(totaltime > 10 )
	        {
		        totaltime--;
		        yield return new WaitForSeconds(0.1f);
	        }

            CancelInvoke("Timer");

            if (countdown == true)
            {
                textdata.text = "0";
            }
            
            switch (this.timerResult)
            {
            case RESULT.Nothing:
	            break;
            case RESULT.Action:
                   this.actionToCall.Execute(gameObject, null);
                    break;
                case RESULT.Condition:
                    this.conditionToCall.Interact(gameObject);
                    break;
            }

                    yield return 0;
        }

        private void Timer()
        {
           
	        

            if (countdown == true)
            {
            	if (leadingZero)
	            	textdata.text = string.Format("{0:00}:{1:00}:{2:00}",TimeSpan.FromSeconds(timervalue).Hours,TimeSpan.FromSeconds(timervalue).Minutes,TimeSpan.FromSeconds(timervalue).Seconds);
	            else 
		            textdata.text = string.Format("{0}:{1}:{2}",TimeSpan.FromSeconds(timervalue).Hours,TimeSpan.FromSeconds(timervalue).Minutes,TimeSpan.FromSeconds(timervalue).Seconds);
	            
		            timervalue--;

            }
            else
            {
	            if (leadingZero)
		            textdata.text = string.Format("{0:00}:{1:00}:{2:00}",TimeSpan.FromSeconds(timervalue).Hours,TimeSpan.FromSeconds(timervalue).Minutes,TimeSpan.FromSeconds(timervalue).Seconds);
	            else
	            	textdata.text = string.Format("{0}:{1}:{2}",TimeSpan.FromSeconds(timervalue).Hours,TimeSpan.FromSeconds(timervalue).Minutes,TimeSpan.FromSeconds(timervalue).Seconds);
	           
	            timervalue++;
            }


        }

		public void StopTimer(float reset)
		{
			totaltime = 0;
			timervalue = reset;

		}
		
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

        public static new string NAME = "UI/Time/UI Timer";
		private const string NODE_TITLE = "Display UI Timer and Action";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptext;

        private SerializedProperty spInitialtimerValue;
        private SerializedProperty spTotaltimerValue;
        private SerializedProperty spactionToCall;
        private SerializedProperty spconditionToCall;
        private SerializedProperty sptimerResult;

        private SerializedProperty spcountdown;
        private SerializedProperty spcountup;
		private SerializedProperty spleadingZero;
		
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.sptext = this.serializedObject.FindProperty("textObject");

            this.spInitialtimerValue = this.serializedObject.FindProperty("InitialtimerValue");
            this.spTotaltimerValue = this.serializedObject.FindProperty("TotaltimerValue");
            this.spactionToCall = this.serializedObject.FindProperty("actionToCall");
            this.spconditionToCall = this.serializedObject.FindProperty("conditionToCall");
            this.sptimerResult = this.serializedObject.FindProperty("timerResult");

            this.spcountdown = this.serializedObject.FindProperty("countdown");
            this.spcountup = this.serializedObject.FindProperty("countup");
			this.spleadingZero = this.serializedObject.FindProperty("leadingZero");


        }

        protected override void OnDisableEditorChild ()
		{
			this.sptext = null;
            this.spInitialtimerValue = null;
            this.spTotaltimerValue = null;
            this.spactionToCall = null;
            this.spconditionToCall = null;
            this.sptimerResult = null;
            this.spcountdown = null;
            this.spcountup = null;
			this.spleadingZero = null;

        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.sptext, new GUIContent("UI Text"));
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(this.spInitialtimerValue, new GUIContent("Time before start"));
			EditorGUILayout.PropertyField(this.spTotaltimerValue, new GUIContent("Timer Value"));
			EditorGUILayout.PropertyField(this.spleadingZero, new GUIContent("Show leading Zeros"));

			EditorGUILayout.Space();

  
            EditorGUILayout.PropertyField(this.sptimerResult, new GUIContent("When Timer expires"));

            switch ((RESULT)this.sptimerResult.intValue)
            {
                case RESULT.Action:
                    EditorGUILayout.PropertyField(this.spactionToCall, new GUIContent("Action to Call"));
                    break;
                case RESULT.Condition:
                    EditorGUILayout.PropertyField(this.spconditionToCall, new GUIContent("Condition to Call"));
                    break;

            }
			EditorGUILayout.LabelField(new GUIContent("Count Direction"));

			EditorGUILayout.BeginHorizontal();
			EditorGUIUtility.labelWidth = 150;
			EditorGUI.indentLevel++;
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(this.spcountdown, new GUIContent("down"));
			EditorGUILayout.PropertyField(this.spcountup, new GUIContent("up"));
			countup = countdown ? false : true;
			EditorGUIUtility.labelWidth = 0;
			EditorGUILayout.EndHorizontal();
			EditorGUI.indentLevel--;
			EditorGUI.indentLevel--;

			EditorGUILayout.Space();

            this.serializedObject.ApplyModifiedProperties();
		}

#endif
    }
}
