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
	public class ActionBlinkMapMarker : IAction
    {
      
	    public GameObject marker;
	    private Transform markerObject;

	    public NumberProperty repeating = new NumberProperty(0.5f);
	    public TargetGameObject target = new TargetGameObject();

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	        GameObject targetValue = this.target.GetGameObject(target);
	        markerObject = targetValue.gameObject.transform.Find("MapMarkerImage");
	 	        float re = repeating.GetValue(target);
		        if (markerObject != null)
		        {
			        InvokeRepeating("Blink", 0.5f, re);
		        
		        }

	

            return true;

        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {



            return base.Execute(target, actions, index);

        }


	    private void Blink()
	    {
		    MeshRenderer renderer = markerObject.GetComponent<MeshRenderer>();
		    
		    if (renderer.enabled == true)
		    
			    renderer.enabled = false;
			    
		    else
		    
			    renderer.enabled = true;
	    }
	  
	    public void StopRepeating()
	    {
		    CancelInvoke("Blink");
	    }
	    
	    
	    
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

	    public static new string NAME = "UI/MiniMap/Blink MiniMap Marker";
	    private const string NODE_TITLE = "Blink MiniMap Marker for {0}";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        
	    private SerializedProperty spmarker;
	    private SerializedProperty sprepeating;


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
	            this.sprepeating = this.serializedObject.FindProperty("repeating");

        }


        protected override void OnDisableEditorChild()
            {
          
	            this.spmarker = null;
	            this.sprepeating = null;
   

        }

        public override void OnInspectorGUI()
            {
                this.serializedObject.Update();
	            EditorGUILayout.LabelField("GameObject");
	            EditorGUI.indentLevel++;
	            EditorGUILayout.PropertyField(this.spmarker, new GUIContent("with Marker"));
	            EditorGUILayout.PropertyField(this.sprepeating, new GUIContent("Blink Rate"));
             	
	            EditorGUI.indentLevel--;

	
	            
	 
	           
            this.serializedObject.ApplyModifiedProperties();
            }
	    

	 
#endif

        }
    }
