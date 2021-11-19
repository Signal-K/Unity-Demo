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
	public class ActionRemoveMapTMPLabels : IAction
    {
      
     

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {

	 	        
	     
	        foreach(GameObject gameObj in GameObject.FindObjectsOfType<GameObject>())
	        {
		        if(gameObj.name == "MapMarkerLabel")
		        {
			        Destroy(gameObj);
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

	    public static new string NAME = "UI/MiniMap/Remove MiniMap TMP Labels";
	    private const string NODE_TITLE = "Remove MiniMap TMP Labels";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        


        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
            {
                return string.Format(
                     NODE_TITLE
                   //  (targetObject == null ? "none" : targetObject.name)
                 );
            }

        protected override void OnEnableEditorChild()
            {
       
	       
        }


        protected override void OnDisableEditorChild()
            {
          
	       

        }

        public override void OnInspectorGUI()
            {
                this.serializedObject.Update();
           
	            EditorGUILayout.LabelField("Removing ALL MiniMap TMP Labels");
	           
            this.serializedObject.ApplyModifiedProperties();
            }
	    

	 
#endif

        }
    }
