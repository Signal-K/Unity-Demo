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
	public class ActionToggleMapMarkers : IAction
    {
      
     public bool showing = false;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {

	 	     
	        foreach(GameObject gameObj in Resources.FindObjectsOfTypeAll<GameObject>())
	        {
		        if(gameObj.name == "MapMarkerImage")
		        {
			        gameObj.SetActive(showing);
	
		        }
	        }
	 
			  


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

	    public static new string NAME = "UI/MiniMap/Toggle MiniMap Markers";
	    private const string NODE_TITLE = "{0} All MiniMap Markers";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        
	    private SerializedProperty spshow;
 

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
            {
                return string.Format(
	                NODE_TITLE,
	                (showing == false ? "Hide" : "Show")
                 );
            }

        protected override void OnEnableEditorChild()
            {
       
	            this.spshow = this.serializedObject.FindProperty("showing");

        }


        protected override void OnDisableEditorChild()
            {
          
	            this.spshow = null;
 

        }

        public override void OnInspectorGUI()
            {
                this.serializedObject.Update();
           
	            EditorGUILayout.LabelField("MiniMap Markers");
	            EditorGUILayout.PropertyField(this.spshow, new GUIContent("Show/Hide"));

            this.serializedObject.ApplyModifiedProperties();
            }
	    

	 
#endif

        }
    }
