namespace GameCreator.UIComponents
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
    using GameCreator.Core.Hooks;

#if UNITY_EDITOR
    using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ActionShakeElement : IAction
	{
        public GameObject shaker;
		public float Duration = 3.0f;          
		public float Amount = 5.0f;                 
  		private Vector3 originalPos;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
        	originalPos = shaker.transform.parent.localPosition;
        	StopAllCoroutines();  
	        StartCoroutine(cShake(Duration, Amount));
	        
            return true;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
          
            return base.Execute(target, actions, index);
        }


		public IEnumerator cShake (float duration, float amount) {
			
			float endTime = Time.time + duration;
			shaker.transform.localPosition = originalPos;
			
			while (Time.time < endTime) {
				shaker.transform.localPosition = originalPos + Random.insideUnitSphere * amount;
				duration -= Time.deltaTime;
				yield return null;
			}

			shaker.transform.localPosition = originalPos;
		}
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

        #if UNITY_EDITOR

		public static new string NAME = "UI/Elements/Shake UI Element";
		private const string NODE_TITLE = "Shake UI Element";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spFloatObjectShaker; 
		private SerializedProperty spFloatDuration;
		private SerializedProperty spFloatAmount;
 
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
            this.spFloatObjectShaker = this.serializedObject.FindProperty("shaker"); 
            this.spFloatDuration = this.serializedObject.FindProperty("Duration");
            this.spFloatAmount = this.serializedObject.FindProperty("Amount");
        }

        protected override void OnDisableEditorChild ()
		{
            this.spFloatObjectShaker = null; 
            this.spFloatDuration = null;
            this.spFloatAmount = null;
        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.spFloatObjectShaker, new GUIContent("Element")); 
			EditorGUILayout.PropertyField(this.spFloatDuration, new GUIContent("Duration"));
			EditorGUILayout.PropertyField(this.spFloatAmount, new GUIContent("Amount"));
			
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
