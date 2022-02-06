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
	public class ActionUITimerStop : IAction
	{


        public enum RESULT
        {
	        Nothing,
	        Action,
            Condition
        }
        public RESULT timerResult = RESULT.Action;

		public NumberProperty resettimerValue = new NumberProperty(10.0f);
        public Actions actionToCall;
        public Conditions conditionToCall;

        private float timervalue;

		public Actions actions;

   
        // EXECUTABLE: ----------------------------------------------------------------------------

		public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
		{
       
         
			var references = this.actions.gameObject.GetComponents<ActionUITimer>();
	        timervalue = resettimerValue.GetValue(target);

	        foreach (var reference in references)
	        {
		        reference.StopTimer(timervalue);
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

   

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "UI/Time/UI Timer Stop";
		private const string NODE_TITLE = "Stop UI Timer and Action";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty sptextmeshtimer;

       private SerializedProperty spactionToCall;
        private SerializedProperty spconditionToCall;
        private SerializedProperty sptimerResult;

         // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.sptextmeshtimer = this.serializedObject.FindProperty("actions");

            this.spactionToCall = this.serializedObject.FindProperty("actionToCall");
            this.spconditionToCall = this.serializedObject.FindProperty("conditionToCall");
            this.sptimerResult = this.serializedObject.FindProperty("timerResult");

 
        }

        protected override void OnDisableEditorChild ()
		{
			this.sptextmeshtimer = null;
            this.spactionToCall = null;
            this.spconditionToCall = null;
            this.sptimerResult = null;
  
        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.sptextmeshtimer, new GUIContent("UI Timer Action"));
            EditorGUILayout.Space();


			EditorGUILayout.PropertyField(this.sptimerResult, new GUIContent("When Timer Stopped"));

            switch ((RESULT)this.sptimerResult.intValue)
            {
            case RESULT.Nothing:
	            
	            break;
            case RESULT.Action:
                    EditorGUILayout.PropertyField(this.spactionToCall, new GUIContent("Action to Call"));
                    break;
            case RESULT.Condition:
                    EditorGUILayout.PropertyField(this.spconditionToCall, new GUIContent("Condition to Call"));
                    break;

            }

            this.serializedObject.ApplyModifiedProperties();
		}

#endif
    }
}
