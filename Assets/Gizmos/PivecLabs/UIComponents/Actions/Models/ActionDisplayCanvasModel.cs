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
    public class ActionDisplayCanvasModel : IAction
    {
	    public LayerMask objectImageLayer;
	    public LightType objectLight;

        public GameObject targetObject;
        public GameObjectProperty targetModel = new GameObjectProperty();

        public bool spinObject;
        public bool dragObject;
	    public bool mousedrag = true;
	    public bool keydrag;
	    public bool outlineObject;
	    public bool outlineObjectKey;
	    public KeyCode selectedKey = KeyCode.None;


        public bool xAxis;
        public bool yAxis;
	    public bool xAxisAuto;
	    public bool yAxisAuto;
	    
	    public bool modelPosition;
	    public Vector3 mPosition;
	    public Vector3 lPosition;
     
	    [Range(0f, 40f)]
	    public float lightIntensity = 5f;

        public Color lightColour = Color.white;

        [Range(1f, 10f)]
        public float objectSize = 4.0f;

 
	    [Range(0.5f, 20f)]
	    public float autoSpeed = 1f;

	    [Range(1f, 30.0f)]
	    public float dragSpeed = 10f;

	    [Range(0f, 100f)]
	    public float centerModel = 0.0f;
	
	    [Range(-1000f, 1000f)]
	    public float centerCamera = 0.0f;

	    [Range(0.1f, 10.0f)]
	    public float outlineWidth;
	    public Color outlineColour;

	    RenderTexture renderTexture;
        RectTransform rt;
        RawImage img;
        private Camera targetCamera;
        private GameObject imageObject;
	    private Transform imageObjectTransform;
       
        public Vector3 axis = new Vector3(0, 1f, 0);
        float Rotation;
        private Light cameraLight;
        private Vector3 speed = Vector3.zero;
        private Vector3 averageSpeed = Vector3.zero;
 
        private Vector2 lastMousePosition = Vector2.zero;
      
        public float RotationSpeed = 10f;

      
	    public bool RotateX = true;
	    public bool InvertX = false;
        private int _xMultiplier
        {
            get { return InvertX ? -1 : 1; }
        }

       
        public bool RotateY = true;
	    public bool InvertY = false;
        private int _yMultiplier
        {
            get { return InvertY ? -1 : 1; }
        }

	    public bool invertZ = false;
	    public int invert;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	        invert = invertZ ? -1 : 1;

	 
            if (renderTexture == null)
            {
                rt = (RectTransform)targetObject.transform;
	            renderTexture = new RenderTexture((int)rt.rect.width*2, (int)rt.rect.height*2, 24, RenderTextureFormat.ARGB32);
	            renderTexture.Create();

            }

	        img = targetObject.gameObject.GetComponent<RawImage>();

	        if (img != null)
            {

                 img.texture = renderTexture;

            }
            
	        if (imageObject == null)
	        {
               
		        imageObject = Instantiate(targetModel.GetValue(target), rt.position, Quaternion.identity);
		        imageObject.name = "UICloneObject";

		        imageObjectTransform = imageObject.GetComponent<Transform>();
		        imageObjectTransform.localScale = new Vector3(objectSize, objectSize, objectSize);

	        }

            if (targetCamera == null)
            {

                GameObject camera3d = new GameObject();
	            targetCamera = camera3d.AddComponent<Camera>();
	            
                targetCamera.enabled = true;
	            targetCamera.allowHDR = true;
                targetCamera.targetTexture = renderTexture;
                targetCamera.orthographic = true;

	            targetCamera.name = "UI3DCamera";

                targetCamera.clearFlags = CameraClearFlags.SolidColor;
                targetCamera.backgroundColor = Color.clear;
                targetCamera.gameObject.layer = layermask_to_layer(objectImageLayer.value);
                targetCamera.cullingMask = objectImageLayer.value;
	         

	           
            }

           


            if (cameraLight == null)

            {
                cameraLight = targetCamera.gameObject.AddComponent<Light>();

                cameraLight.gameObject.layer = layermask_to_layer(objectImageLayer.value);
                cameraLight.cullingMask = objectImageLayer.value;
	            cameraLight.type = objectLight;
	            cameraLight.intensity = (lightIntensity / 10);
	            cameraLight.range = 200;
                cameraLight.color = lightColour;
                cameraLight.bounceIntensity = 1;
               }


            Vector3 containerLocalPosition = imageObject.transform.position - targetCamera.transform.position;


	        float DesireDistanceFromCamera = 1.0f;
            imageObjectTransform.position = targetCamera.transform.position + (containerLocalPosition * DesireDistanceFromCamera);

	        imageObjectTransform.position = new Vector3 (imageObject.transform.position.x, imageObject.transform.position.y , imageObject.transform.position.z - centerModel);
	        
	        targetCamera.transform.position = new Vector3(imageObject.transform.position.x, (imageObject.transform.position.y+centerCamera), invert);
 
	        imageObjectTransform.rotation = Quaternion.Euler(this.mPosition);

            var position = imageObjectTransform.position + Vector3.up * 5;
            cameraLight.transform.LookAt(position);
	        targetCamera.transform.LookAt(position+lPosition);
	        targetCamera.Render();
	        

	        

            if (!targetObject.gameObject.GetComponent<Outline>())
	          targetObject.gameObject.AddComponent<Outline>();
            if (!targetObject.gameObject.GetComponent<MouseOver>())
                targetObject.gameObject.AddComponent<MouseOver>();


	        targetObject.gameObject.GetComponent<Outline>().effectDistance = new Vector2(outlineWidth,-outlineWidth);
	        targetObject.gameObject.GetComponent<Outline>().effectColor = outlineColour;
	        targetObject.gameObject.GetComponent<Outline>().enabled = false;


          
               
	        if (outlineObject == true)
	        {
		        targetObject.gameObject.GetComponent<MouseOver>().outlineObject = true;

	        }
	        else 
	        {
		        targetObject.gameObject.GetComponent<MouseOver>().outlineObject = false;

	        }
	        
	     

  
            return true;

        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {



            return base.Execute(target, actions, index);

        }

        public static int layermask_to_layer(LayerMask layerMask)
        {
            int layerNumber = 0;
            int layer = layerMask.value;
            while (layer > 0)
            {
                layer = layer >> 1;
                layerNumber++;
            }

            return layerNumber - 1;
        }


        void Update()
        {

            if ((imageObject != null) && (spinObject == true))
            { 
                    float xAuto = 0;
                    float yAuto = 0;

                     if (xAxisAuto == true)
                    {
                        xAuto = autoSpeed;

                    }
                    if (yAxisAuto == true)
                    {
                        yAuto = autoSpeed;

                    }
                    if (imageObject != null)
                        imageObject.transform.Rotate(yAuto, xAuto, 0);

            }

            if ((imageObject != null) && (dragObject == true) && (mousedrag == true))
            
            {
                bool overObj = targetObject.gameObject.GetComponent<MouseOver>().overObject;
                if (overObj == true)
                {
                    if (lastMousePosition == Vector2.zero) lastMousePosition = Input.mousePosition;

                    if (Input.GetMouseButton(0))
                    {
                        var mouseDelta = ((Vector2)Input.mousePosition - lastMousePosition) * 100;
                        mouseDelta.Set(mouseDelta.x / Screen.width, mouseDelta.y / Screen.height);

                        speed = new Vector3(-mouseDelta.x * _xMultiplier, mouseDelta.y * _yMultiplier, 0);
                    }


                    if (speed != Vector3.zero)
                    {
                        if (xAxis == true)
                        {
                            imageObject.transform.Rotate(0, speed.x * dragSpeed, 0);
                        }
                        if (yAxis == true)
                        {
                            imageObject.transform.Rotate(speed.y * dragSpeed, 0, 0);
                        }
                    }

                    lastMousePosition = Input.mousePosition;
                }
            }

         if ((imageObject != null) && (dragObject == true) && (keydrag == true))
            
         {
	         bool overObj = this.targetObject.gameObject.GetComponent<Outline>().enabled;
	         if (overObj == true)
	         {
		         

		         if (Input.GetKey(KeyCode.LeftArrow))
		         {
			         if (xAxis == true)
			         {
				         imageObject.transform.Rotate(Vector3.up * 10 * dragSpeed * Time.deltaTime);
			         }
			         if (yAxis == true)
			         {
				         imageObject.transform.Rotate(Vector3.right * 10 * dragSpeed * Time.deltaTime);
			         }
		         }
			     
			     if (Input.GetKey(KeyCode.RightArrow))
				         {
					         if (xAxis == true)
					         {
						         imageObject.transform.Rotate(Vector3.down * 10 * dragSpeed * Time.deltaTime);
					         }
					         if (yAxis == true)
					         {
						         imageObject.transform.Rotate(Vector3.left * 10 * dragSpeed * Time.deltaTime);
					         }
				         }
			       
		         }
         }
	       
	         
         }






        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

	    public static new string NAME = "UI/Models/Display 3D Model on Canvas";
            private const string NODE_TITLE = "Display 3D Model on {0}";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptargetModel;
        private SerializedProperty spimageLayer;
        private SerializedProperty sptargetObject;
        private SerializedProperty spspinObject;
        private SerializedProperty spdragObject;
	    private SerializedProperty spmousedrag;
	    private SerializedProperty spkeydrag;
	    private SerializedProperty spxAxis;
        private SerializedProperty spyAxis;
	    private SerializedProperty spxAxisAuto;
	    private SerializedProperty spyAxisAuto;
	    private SerializedProperty spscale;
        private SerializedProperty splight;
	    private SerializedProperty spinvertz;
	    private SerializedProperty spautospeed;
	    private SerializedProperty spdragspeed;
	    private SerializedProperty spobjectlight;
        private SerializedProperty splightColour;
        private SerializedProperty spoutlineObject;
	    private SerializedProperty spcenterModel;
	    private SerializedProperty spcenterCamera;
	    private SerializedProperty spoutlineWidth;
	    private SerializedProperty spoutlineColour;
	    private SerializedProperty spmodelPosition;
	    private SerializedProperty spmPosition;
	    private SerializedProperty splPosition;
	    private SerializedProperty spselectedKey;


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
            this.spimageLayer = this.serializedObject.FindProperty("objectImageLayer");
            this.sptargetModel = this.serializedObject.FindProperty("targetModel");
            this.sptargetObject = this.serializedObject.FindProperty("targetObject");
            this.spspinObject = this.serializedObject.FindProperty("spinObject");
            this.spdragObject = this.serializedObject.FindProperty("dragObject");
	            this.spmousedrag = this.serializedObject.FindProperty("mousedrag");
	            this.spkeydrag = this.serializedObject.FindProperty("keydrag");
	            this.spxAxis = this.serializedObject.FindProperty("xAxis");
            this.spyAxis = this.serializedObject.FindProperty("yAxis");
	        this.spxAxisAuto = this.serializedObject.FindProperty("xAxisAuto");
	        this.spyAxisAuto = this.serializedObject.FindProperty("yAxisAuto");
	        this.spscale = this.serializedObject.FindProperty("objectSize");
            this.splight = this.serializedObject.FindProperty("lightIntensity");
            this.splightColour = this.serializedObject.FindProperty("lightColour");
            this.spinvertz = this.serializedObject.FindProperty("invertZ");
	            this.spautospeed = this.serializedObject.FindProperty("autoSpeed");
	            this.spdragspeed = this.serializedObject.FindProperty("dragSpeed");
	            this.spobjectlight = this.serializedObject.FindProperty("objectLight");
	            this.spoutlineObject = this.serializedObject.FindProperty("outlineObject");
	            this.spcenterModel = this.serializedObject.FindProperty("centerModel");
	            this.spcenterCamera = this.serializedObject.FindProperty("centerCamera");
	            this.spoutlineWidth = this.serializedObject.FindProperty("outlineWidth");
	            this.spoutlineColour = this.serializedObject.FindProperty("outlineColour");
	            this.spmodelPosition = this.serializedObject.FindProperty("modelPosition");
	            this.spmPosition = this.serializedObject.FindProperty("mPosition");
	            this.splPosition = this.serializedObject.FindProperty("lPosition");
	            this.spselectedKey = this.serializedObject.FindProperty("selectedKey");
            }


        protected override void OnDisableEditorChild()
            {
            this.spimageLayer = null;
            this.sptargetObject = null;
            this.sptargetModel = null;
            this.spspinObject = null;
            this.spdragObject = null;
             this.spmousedrag = null;
	            this.spkeydrag = null;
	            this.spxAxis = null;
            this.spyAxis = null;
	        this.spxAxisAuto = null;
	        this.spyAxisAuto = null;
	        this.spscale = null;
            this.splight = null;
            this.splightColour = null;
            this.spinvertz = null;
	            this.spautospeed = null;
	            this.spdragspeed = null;
	            this.spobjectlight = null;
	            this.spoutlineObject = null;
	            this.spcenterModel = null;
	            this.spcenterCamera = null;
	            this.spoutlineWidth = null;
	            this.spoutlineColour = null;
	            this.spmodelPosition = null;
	            this.spmPosition = null;
	            this.splPosition = null;
	            this.spselectedKey = null;

        }

        public override void OnInspectorGUI()
	    {
		

                this.serializedObject.Update();
            EditorGUILayout.PropertyField(this.sptargetObject, new GUIContent("Canvas RawImage"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.sptargetModel, new GUIContent("3D Model"));
            EditorGUI.indentLevel++;
		    EditorGUILayout.PropertyField(this.spimageLayer, new GUIContent("Image Layer of Model"));
		    EditorGUILayout.PropertyField(this.spmodelPosition, new GUIContent("Model Rotation"));
		    if (modelPosition == true)
		    { 
			    EditorGUI.indentLevel++;
			    EditorGUILayout.PropertyField(this.spmPosition, new GUIContent("Local Rotation"));
			    EditorGUILayout.PropertyField(this.splPosition, new GUIContent("LookAt Offset"));
			    EditorGUI.indentLevel--;
		    }
	            EditorGUILayout.PropertyField(this.spinvertz, new GUIContent("Invert Model"));
		    EditorGUILayout.PropertyField(this.spcenterModel, new GUIContent("Reposition Model"));
		    EditorGUILayout.PropertyField(this.spcenterCamera, new GUIContent("Reposition Camera"));
		    EditorGUI.indentLevel--;
		    EditorGUILayout.Space(); EditorGUILayout.LabelField("Lighting", EditorStyles.boldLabel);
		    EditorGUI.indentLevel++;
        		    EditorGUILayout.PropertyField(this.spscale, new GUIContent("Size of Model"));
                    EditorGUILayout.PropertyField(this.splight, new GUIContent("Light Intensity"));
                    EditorGUILayout.PropertyField(this.splightColour, new GUIContent("Light Color"));
				    EditorGUILayout.PropertyField(this.spobjectlight, new GUIContent("Type"));
		    EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            EditorGUILayout.Space(); EditorGUILayout.LabelField("Display Properties", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
	            EditorGUILayout.PropertyField(this.spspinObject, new GUIContent("Auto Rotate"));
	            if (spinObject == true)
	            { 
	   	            EditorGUI.indentLevel++;
		            EditorGUILayout.Space();
		            EditorGUILayout.PropertyField(this.spxAxisAuto, new GUIContent("x Axis"));
		            EditorGUILayout.PropertyField(this.spyAxisAuto, new GUIContent("y Axis"));
		            EditorGUILayout.PropertyField(this.spautospeed, new GUIContent("Rotate Speed"));
		            Rect position = EditorGUILayout.GetControlRect(false, 2 * EditorGUIUtility.singleLineHeight); 
		            position.height *= 0.5f;
           
		            position.y += position.height - 20;
		            position.x += EditorGUIUtility.labelWidth -30;
		            position.width -= EditorGUIUtility.labelWidth + 26; 
		            GUIStyle style = GUI.skin.label;
		            style.fontSize = 10;
		            style.alignment = TextAnchor.UpperLeft; EditorGUI.LabelField(position, "Slower", style);
		            style.alignment = TextAnchor.UpperRight; EditorGUI.LabelField(position, "Faster", style);

		            EditorGUI.indentLevel--;
	            }
	            
		    EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spdragObject, new GUIContent("Drag to Rotate"));
	     if (dragObject == true)
            	{  
		            EditorGUI.indentLevel++;
		     EditorGUILayout.Space();
		     EditorGUILayout.PropertyField(this.spmousedrag,new GUIContent("Mouse"));
		     EditorGUILayout.PropertyField(this.spkeydrag, new GUIContent("Arrow Keys") );
                	EditorGUILayout.PropertyField(this.spxAxis, new GUIContent("x Axis"));
                	EditorGUILayout.PropertyField(this.spyAxis, new GUIContent("y Axis"));
		            EditorGUILayout.PropertyField(this.spdragspeed, new GUIContent("Drag Speed"));
		            EditorGUI.indentLevel--;
	       		 }
          
		    EditorGUILayout.Space();
		    EditorGUILayout.PropertyField(this.spoutlineObject, new GUIContent("Outline on Mouseover"));
            EditorGUILayout.Space();

		    if (outlineObject == true || outlineObjectKey == true)
				  {  
			    EditorGUI.indentLevel++;
			    EditorGUILayout.PropertyField(this.spoutlineWidth, new GUIContent("Outline Width"));
			    EditorGUILayout.PropertyField(this.spoutlineColour, new GUIContent("Outline Color"));
			    EditorGUI.indentLevel--;
				  }

		    EditorGUI.indentLevel--;
            this.serializedObject.ApplyModifiedProperties();
            }

#endif

        }
    }
