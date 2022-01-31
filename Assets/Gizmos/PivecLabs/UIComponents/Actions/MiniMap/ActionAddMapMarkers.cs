namespace GameCreator.UIComponents
{
        using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;
        using UnityEngine.Events;
        using UnityEngine.UI;
        using GameCreator.Core;
        using GameCreator.Variables;
        using GameCreator.Core.Hooks;
        using GameCreator.Characters;

#if UNITY_EDITOR
    using UnityEditor;
#endif

    [AddComponentMenu("")]
	public class ActionAddMapMarkers : IAction
    {
        public GameObject mapPanel;
        public GameObject rawImage;

	    [System.Serializable]
	    public class Marker
	    {
		    public string Tag;
		    public Texture2D Image;
	    }

	    [SerializeField]
	    public List<Marker> MapMarkers = new List<Marker>();
	    
	    public Material frontPlane;

	    [Range(0,5)]
	    public float markerSize = 0.2f;
 
	    private Vector3 markers;
	  
	    
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
				        plane.name = "MapMarkerImage";
				        plane.transform.localScale = new Vector3(markerSize, markerSize, markerSize);
				        plane.transform.parent = gameobject[a].transform;
				        plane.transform.position = gameobject[a].transform.position + new Vector3(0,10,0);
				        plane.layer = Layer;
				        Material material = new Material(Shader.Find("Unlit/Transparent Cutout"));
				        material.mainTexture = MapMarkers [i].Image;
				        plane.GetComponent<Renderer>().material = material;
	
			        }

		        }
		        
	        }
	 
	 
			  


            return true;

        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {



            return base.Execute(target, actions, index);

        }
#if UNITY_EDITOR

	    public static void AddAlwaysIncludedShader(string shaderName)
	    {
	    	
	 	    var shader = Shader.Find(shaderName);
		    if (shader == null)
			    return;
		    SerializedObject graphicsSettings = new SerializedObject(AssetDatabase.LoadAssetAtPath<Object>("ProjectSettings/GraphicsSettings.asset"));
			var arrayProp = graphicsSettings.FindProperty("m_AlwaysIncludedShaders");
		    bool hasShader = false;
		    for (int i = 0; i < arrayProp.arraySize; ++i)
		    {
			    var arrayElem = arrayProp.GetArrayElementAtIndex(i);
			    if (shader == arrayElem.objectReferenceValue)
			    {
				    hasShader = true;
				    break;
			    }
		    }
 
		    if (!hasShader)
		    {
			    int arrayIndex = arrayProp.arraySize;
			    arrayProp.InsertArrayElementAtIndex(arrayIndex);
			    var arrayElem = arrayProp.GetArrayElementAtIndex(arrayIndex);
			    arrayElem.objectReferenceValue = shader;
 
			    graphicsSettings.ApplyModifiedProperties();
 
			    AssetDatabase.SaveAssets();
			    
			    Debug.Log("Shader Added");
		    }
		    
		   
	    }
	    
	   #endif
 
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR


	    public static new string NAME = "UI/MiniMap/Add MiniMap Markers";
	    private const string NODE_TITLE = "Add MiniMap Markers";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        
	    private SerializedProperty spmarker;
	    private SerializedProperty spmarkersize;
	    private SerializedProperty splayer;
 

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
	            this.spmarkersize = this.serializedObject.FindProperty("markerSize");
	            this.splayer = this.serializedObject.FindProperty("Layer");
   
        }


        protected override void OnDisableEditorChild()
            {
          
	            this.spmarker = null;
	            this.spmarkersize = null;
            
	            this.splayer = null;

        }

        public override void OnInspectorGUI()
            {
                this.serializedObject.Update();
           
	            SerializedProperty property = serializedObject.FindProperty("MapMarkers");
	            EditorGUILayout.PropertyField(this.spmarkersize, new GUIContent("Marker Scale"));
	            EditorGUILayout.PropertyField(this.splayer, new GUIContent("Culling Layer"));

	 	        ArrayGUI(property, "Marker ", true);
	            AddAlwaysIncludedShader("Unlit/Transparent Cutout") ;          

	           
            this.serializedObject.ApplyModifiedProperties();
            }
	    
	    private void ArrayGUI(SerializedProperty property, string itemType, bool visible)
	    {
		    EditorGUI.indentLevel++;
		    visible = EditorGUILayout.Foldout(visible, new GUIContent("Map Markers"));
		    if (visible)
		    {

			    EditorGUI.indentLevel++;
			    SerializedProperty arraySizeProp = property.FindPropertyRelative("Array.size");
			    EditorGUILayout.PropertyField(arraySizeProp,new GUIContent("Marker Types"));
             
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
