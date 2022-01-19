namespace GameCreator.Stats
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
    using GameCreator.Variables;

	#if UNITY_EDITOR
	using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ActionChangeStatUI : IAction
	{
		public StatUI statUI;

		[Space]
        public TargetGameObject stats = new TargetGameObject(TargetGameObject.Target.Player);
        [StatSelector] public StatAsset stat;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	        if (this.statUI == null) return true;
	        if (this.stat == null) return true;
	        
	        Stats componentTarget = this.stats.GetGameObject(target).GetComponentInChildren<Stats>();
            if (componentTarget == null) return true;

            GameObject statsValue = this.stats.GetGameObject(target);
            
            this.statUI.stat = this.stat;
            this.statUI.target = new TargetGameObject(TargetGameObject.Target.GameObject)
            {
	            gameObject = statsValue
            };

            return true;
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

        #if UNITY_EDITOR

        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Stats/Icons/Actions/";

        public static new string NAME = "Stats/Change Stat UI";
        private const string NODE_TITLE = "Change StatUI to {0}";

		public override string GetNodeTitle()
		{
            return string.Format(
                NODE_TITLE, 
                this.stats
            );
		}

#endif
	}
}
