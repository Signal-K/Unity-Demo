namespace GameCreator.UIComponents
{
        using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;
        using UnityEngine.Events;
        using UnityEngine.UI;
        using UnityEngine.Video;
        using GameCreator.Core;
        using GameCreator.Variables;
        using GameCreator.Core.Hooks;
        using GameCreator.Characters;
		using System.Linq;
		
#if UNITY_EDITOR
    using UnityEditor;
#endif



	
    [AddComponentMenu("")]
	public class ActionStopBlinkMapMarker : IAction
    {
      

	    public TargetGameObject target = new TargetGameObject();
	    public Actions actions;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	        var references = this.actions.gameObject.GetComponents<ActionBlinkMapMarker>();

	         foreach (var reference in references)
	         {
		         reference.StopRepeating();
	         }
	          
	        GameObject targetValue = this.target.GetGameObject(target);
	        Transform markerObject = targetValue.gameObject.transform.Find("MapMarkerImage");
	        MeshRenderer renderer = markerObject.GetComponent<MeshRenderer>();
	        renderer.enabled = true;

	          return true;

        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {



            return base.Execute(target, actions, index);

        }

	   
	  
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

	    public static new string NAME = "UI/MiniMap/Stop Blink MiniMap Marker";
	    private const string NODE_TITLE = "Stop Blink MiniMap Marker for {0}";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        
	    private SerializedProperty spmarker;
	    private SerializedProperty spaction;


        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
            {
                return string.Format(
	                NODE_TITLE,
	                this.target
                 );
            }

        protected override void OnEnableEditorChild()
            {
       
	            this.spmarker = this.serializedObject.FindProperty("target");
	            this.spaction = this.serializedObject.FindProperty("actions");
 
        }


        protected override void OnDisableEditorChild()
            {
          
	            this.spmarker = null;
	            this.spaction = null;
 

        }

        public override void OnInspectorGUI()
            {
                this.serializedObject.Update();
            	EditorGUILayout.LabelField("GameObject");
	            EditorGUI.indentLevel++;

	            EditorGUILayout.PropertyField(this.spmarker, new GUIContent("with Marker"));
	            EditorGUILayout.PropertyField(this.spaction, new GUIContent("Start Blink Action"));
	            EditorGUI.indentLevel--;

	            
            this.serializedObject.ApplyModifiedProperties();
            }
	    

	 
#endif

        }
    }
