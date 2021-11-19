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
	public class ActionToggleSingleMapMarker : IAction
    {
	    private Transform markerObject;
	    public TargetGameObject target = new TargetGameObject();

    	public bool showing = false;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
         
        	GameObject targetValue = this.target.GetGameObject(target);

	        markerObject = targetValue.transform.Find("MapMarkerImage");

	       
	        markerObject.gameObject.SetActive(showing);
	
			  


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

	    public static new string NAME = "UI/MiniMap/Toggle Single MiniMap Marker";
	    private const string NODE_TITLE = "{0} {1} MiniMap Marker";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        
	    private SerializedProperty spshow;
	    private SerializedProperty spmarker;


        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
            {
                return string.Format(
	                NODE_TITLE,
	                (showing == false ? "Hide" : "Show"), this.target
                 );
            }

        protected override void OnEnableEditorChild()
            {
       
	            this.spshow = this.serializedObject.FindProperty("showing");
	            this.spmarker = this.serializedObject.FindProperty("target");

        }


        protected override void OnDisableEditorChild()
            {
          
	            this.spshow = null;
	            this.spmarker = null;


        }

        public override void OnInspectorGUI()
            {
                this.serializedObject.Update();
           
	            EditorGUILayout.PropertyField(this.spmarker, new GUIContent("Game Object"));

	            EditorGUILayout.PropertyField(this.spshow, new GUIContent("Show/Hide"));

            this.serializedObject.ApplyModifiedProperties();
            }
	    

	 
#endif

        }
    }
