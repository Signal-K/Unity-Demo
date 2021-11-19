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
		using TMPro;
		
#if UNITY_EDITOR
    using UnityEditor;
#endif

    [AddComponentMenu("")]
	public class ActionAddMapTMPLabels : IAction
    {
        public GameObject mapPanel;
        public GameObject rawImage;
	    [Range(1,60)]
	    public float cameraSize = 20;
	    [Range(1,60)]
	    public float cameraDistance = 20;
	    public ColorProperty outlinecolor = new ColorProperty(Color.black);
	    public ColorProperty textcolor = new ColorProperty(Color.white);

	    [System.Serializable]
	    public class Marker
	    {
		    public string Tag;
		    public string text;
	    }

	    [SerializeField]
	    public List<Marker> MapMarkers = new List<Marker>();
	    
	    public Material frontPlane;

	    [Range(0,2)]
	    public float markerSize = 0.4f;
 
	    private Vector3 markers;

	    private TextMeshPro textMeshPro;
	  
	    
        private PlayerCharacter player;
     
        
	    public int Layer;
  

      

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {

	       
                        
		
	          
	        for (int i = 0; i < MapMarkers.Capacity; i++) {

		        
		        var gameobject = GameObject.FindGameObjectsWithTag(MapMarkers [i].Tag);
		        if (gameobject != null)
		        {
			        for (int a = 0; a < gameobject.Length; a++)
			        {
			        
				        GameObject plane  = GameObject.CreatePrimitive(PrimitiveType.Plane);
				        plane.name = "MapMarkerLabel";
				        plane.layer = Layer;
				        plane.transform.localScale = new Vector3(markerSize, markerSize, markerSize);
				        plane.transform.parent = gameobject[a].transform;
				        plane.transform.position = gameobject[a].transform.position + new Vector3(0,10,0);
				        plane.transform.localRotation = Quaternion.Euler(90, 0, 0);
				        Material material = new Material(Shader.Find("TextMeshPro/Bitmap"));
				      
	
				        textMeshPro = plane.AddComponent<TextMeshPro>();
				        textMeshPro.fontSize = 30;
				        textMeshPro.outlineColor = outlinecolor.GetValue(target);
				        textMeshPro.outlineWidth = 0.2f;
				        textMeshPro.color = textcolor.GetValue(target);
				        textMeshPro.enableAutoSizing = true;
				        textMeshPro.alignment= TextAlignmentOptions.Center;				        
				        textMeshPro.text = MapMarkers [i].text;
	
			        }

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

	    public static new string NAME = "UI/MiniMap/Add MiniMap TMP Labels";
	    private const string NODE_TITLE = "Add MiniMap TMP Labels";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        
	    private SerializedProperty spmarker;
	    private SerializedProperty splayer;
	    private SerializedProperty spColoroutline;
	    private SerializedProperty spColortext;
	    private SerializedProperty spmarkersize;
 

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
            {
                return string.Format(
                     NODE_TITLE
                  );
            }

        protected override void OnEnableEditorChild()
            {
       
	            this.spmarker = this.serializedObject.FindProperty("marker");
	            this.splayer = this.serializedObject.FindProperty("Layer");
	            this.spColoroutline = this.serializedObject.FindProperty("outlinecolor");
	            this.spColortext = this.serializedObject.FindProperty("textcolor");
	            this.spmarkersize = this.serializedObject.FindProperty("markerSize");
 
        }


        protected override void OnDisableEditorChild()
            {
          
	            this.spmarker = null;
	            this.splayer = null;
	            this.spColoroutline = null;
	            this.spColortext = null;
	            this.spmarkersize = null;
 

        }

        public override void OnInspectorGUI()
            {
                this.serializedObject.Update();
           
	            SerializedProperty property = serializedObject.FindProperty("MapMarkers");
	            EditorGUILayout.PropertyField(this.spmarkersize, new GUIContent("Label Scale"));
	            ArrayGUI(property, "Marker ", true);
	            EditorGUILayout.Space();
            	EditorGUILayout.PropertyField(this.spColortext, new GUIContent("Text colour"));
	            EditorGUILayout.PropertyField(this.spColoroutline, new GUIContent("Outline colour"));
	            EditorGUILayout.PropertyField(this.splayer, new GUIContent("Culling Layer"));
  
	 
            this.serializedObject.ApplyModifiedProperties();
            }
	    
	    private void ArrayGUI(SerializedProperty property, string itemType, bool visible)
	    {
		    EditorGUI.indentLevel++;
		    visible = EditorGUILayout.Foldout(visible, new GUIContent("Map Labels"));
		    if (visible)
		    {

			    EditorGUI.indentLevel++;
			    SerializedProperty arraySizeProp = property.FindPropertyRelative("Array.size");
			    EditorGUILayout.PropertyField(arraySizeProp,new GUIContent("Lable Types"));
             
			    for (int i = 0; i < arraySizeProp.intValue; i++)
			    {
				    EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i), new GUIContent(itemType + (i +1).ToString()), true);
				    
 
			    }

			    
			    EditorGUI.indentLevel--;
		    }
		    
		    EditorGUI.indentLevel--;
	    }

	 
#endif

        }
    }
