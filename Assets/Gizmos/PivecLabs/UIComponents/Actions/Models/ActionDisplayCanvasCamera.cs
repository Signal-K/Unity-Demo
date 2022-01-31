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

#if UNITY_EDITOR
    using UnityEditor;
#endif

        [AddComponentMenu("")]
        public class ActionDisplayCanvasCamera : IAction
        {
 
	    public GameObject cameraobject;
 
	    public GameObject targetObject;
        
        RenderTexture renderTexture;
        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
            {

	            if (renderTexture == null)
	            {
		            RectTransform rt = (RectTransform)targetObject.transform;
		            renderTexture = new RenderTexture((int)rt.rect.width, (int)rt.rect.height, 32);
		            renderTexture.Create();
	            }
	            
	            Camera cam = cameraobject.GetComponent<Camera>();
  
	             cam.targetTexture = renderTexture;
	            	RawImage img = targetObject.gameObject.GetComponent<RawImage>();

	            	img.texture = renderTexture;
 
	             cameraobject.SetActive(true);
 

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

	        public static new string NAME = "UI/Models/Add Camera to Canvas";
	        private const string NODE_TITLE = "Place Camera on {0}";
            public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

   	        private SerializedProperty spcamera;
              private SerializedProperty sptargetObject;
    
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
            {
                return string.Format(
                     NODE_TITLE,
                     (targetObject == null ? "none" : targetObject.name)
                 );
            }

            protected override void OnEnableEditorChild()
            {
                this.spcamera = this.serializedObject.FindProperty("cameraobject");
                 this.sptargetObject = this.serializedObject.FindProperty("targetObject");
         }

        protected override void OnDisableEditorChild()
            {
                this.spcamera = null;
                 this.sptargetObject = null;
   
        }

        public override void OnInspectorGUI()
            {
                this.serializedObject.Update();

	            EditorGUILayout.PropertyField(this.spcamera, new GUIContent("Camera to Display"));
         
          
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(this.sptargetObject, new GUIContent("Target Raw Image"));
          
            this.serializedObject.ApplyModifiedProperties();
            }

#endif
        }
    }
