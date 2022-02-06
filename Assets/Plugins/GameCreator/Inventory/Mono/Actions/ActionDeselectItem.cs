namespace GameCreator.Inventory
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using GameCreator.Core;

    [AddComponentMenu("")]
    public class ActionDeselectItem : IAction
    {
        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            SelectedItemUI.Deselect();
            return true;
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

        #if UNITY_EDITOR

        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Actions/";

        public static new string NAME = "Inventory/Deselect Item";
        private const string NODE_TITLE = "Deselects any selected item";

        public override string GetNodeTitle()
        {
            return NODE_TITLE;
        }

        #endif
    }
}
