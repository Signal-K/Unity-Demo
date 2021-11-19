namespace GameCreator.UIComponents
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Audio;
	using UnityEngine.UI;
	using GameCreator.Core;
    using GameCreator.Localization;

    #if UNITY_EDITOR
    using UnityEditor;
    #endif


    [AddComponentMenu("")]
    public class ActionShowTooltip : IAction
    {
	    public GameObject tooltipPanel;
	    public Color panelcolor = Color.black;
        public Font font;

	    public AudioClip audioClip;

        [LocStringNoPostProcess]
        public LocString message = new LocString();

	    public Color textcolor = Color.white;
        public float time = 2.0f;

        public TargetGameObject target = new TargetGameObject(TargetGameObject.Target.GameObject);
	    public Vector3 offset = new Vector3(-60, 60, 0);

	    private Text tooltipText;
	    private Image tooltippanel;
	    
        // EXECUTABLE: ----------------------------------------------------------------------------

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
	    {
		    
	        if (this.audioClip != null)
	        {
		        AudioMixerGroup voiceMixer = DatabaseGeneral.Load().voiceAudioMixer;
		        AudioManager.Instance.PlayVoice(this.audioClip, 0f, 1f, voiceMixer);
	        }
			
	        tooltippanel = tooltipPanel.GetComponent<Image>();
	        tooltipText = tooltipPanel.GetComponentInChildren<Text>();
	        
	        if (this.tooltippanel != null)
	        {
	        tooltippanel.color = this.panelcolor;
	        }
	        
	        if (this.tooltipText != null)
	        {
		        tooltipText.text = this.message.GetText();
		        tooltipText.color = this.textcolor;
		        
		        if (this.font != null)
		        {
			        tooltipText.font = font;
		        }
		        else
		        {
		        	tooltipText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		        }
		        

	        }
	
	        tooltipPanel.transform.position = new Vector3(
		        target.transform.position.x + this.offset.x,
		        target.transform.position.y + this.offset.y,
		        target.transform.position.z + this.offset.z
	        );
	        
	        tooltipPanel.SetActive(true);
	
	  
		    float waitTime = Time.unscaledTime + this.time;
		    WaitUntil waitUntil = new WaitUntil(() => Time.unscaledTime > waitTime);
	        yield return waitUntil;

	        if (this.audioClip != null) AudioManager.Instance.StopVoice(this.audioClip);
	        
	        tooltipPanel.SetActive(false);

	        yield return 0;
        }

#if UNITY_EDITOR
	    public static new string NAME = "UI/Elements/Show Tooltip Message";
	    private const string NODE_TITLE = "Show Tooltip for {0}";
	    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

	    private SerializedProperty spTooltipPanel;
	    private SerializedProperty spAudioClip;
        private SerializedProperty spMessage;
	    private SerializedProperty spTextColor;
	    private SerializedProperty spPanelColor;
	    private SerializedProperty spTime;
        private SerializedProperty spOffset;
	    private SerializedProperty spTarget;
	    private SerializedProperty spfont;
  
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {
            return string.Format(
	            NODE_TITLE, this.target
                
            );
        }

        protected override void OnEnableEditorChild()
        {
	        this.spTooltipPanel = this.serializedObject.FindProperty("tooltipPanel");
	        this.spAudioClip = this.serializedObject.FindProperty("audioClip");
            this.spMessage = this.serializedObject.FindProperty("message");
	        this.spTextColor = this.serializedObject.FindProperty("textcolor");
	        this.spPanelColor = this.serializedObject.FindProperty("panelcolor");
	        this.spTime = this.serializedObject.FindProperty("time");
	        this.spOffset = this.serializedObject.FindProperty("offset");
	        this.spTarget = this.serializedObject.FindProperty("target");
	        this.spfont = this.serializedObject.FindProperty("font");
  
        }

        public override void OnInspectorGUI()
        {
	        this.serializedObject.Update();
	        EditorGUILayout.PropertyField(this.spTarget);
	        EditorGUILayout.Space();

	        EditorGUILayout.PropertyField(this.spTooltipPanel, new GUIContent("Tooltip Panel"));
	        EditorGUI.indentLevel++;
	        EditorGUILayout.PropertyField(this.spPanelColor, new GUIContent("Panel Colour"));
	        EditorGUI.indentLevel--;
	        EditorGUILayout.Space();

	        EditorGUILayout.PropertyField(this.spMessage, new GUIContent("Tooltip Text"));
	        EditorGUI.indentLevel++;
	        EditorGUILayout.PropertyField(this.spTextColor, new GUIContent("Text Colour"));
	        EditorGUILayout.PropertyField(this.spfont, new GUIContent("Text Font"));
 
	        EditorGUILayout.PropertyField(this.spTime, new GUIContent("Time showing"));
	        EditorGUILayout.PropertyField(this.spOffset, new GUIContent("Offset from Target"));
	        EditorGUI.indentLevel--;
   
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spAudioClip, new GUIContent("Tooltip Audio Clip"));

 
            if (this.spAudioClip.objectReferenceValue != null)
            {
                AudioClip clip = (AudioClip)this.spAudioClip.objectReferenceValue;
                if (!Mathf.Approximately(clip.length, this.spTime.floatValue))
                {
                    Rect btnRect = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.miniButton);
                    btnRect = new Rect(
                        btnRect.x + EditorGUIUtility.labelWidth,
                        btnRect.y,
                        btnRect.width - EditorGUIUtility.labelWidth,
                        btnRect.height
                    );

                    if (GUI.Button(btnRect, "Use Audio Length", EditorStyles.miniButton))
                    {
                        this.spTime.floatValue = clip.length;
                    }
                }
            }

            EditorGUILayout.Space();

            this.serializedObject.ApplyModifiedProperties();
        }
#endif
    }
}
