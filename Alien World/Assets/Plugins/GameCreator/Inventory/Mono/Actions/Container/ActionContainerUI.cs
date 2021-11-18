namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
    using GameCreator.Core;

	[AddComponentMenu("")]
	public class ActionContainerUI : IAction
	{
        public enum Action
        {
            Open,
            Close
        }

        public TargetGameObject container = new TargetGameObject(TargetGameObject.Target.Invoker);
        public Action action = Action.Open;

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            switch (this.action)
            {
                case Action.Open:
                    GameObject containerGo = this.container.GetGameObject(target);
                    if (containerGo == null) return true;

                    Container containerTarget = containerGo.GetComponent<Container>();
                    if (containerTarget == null) return true;

                    ContainerUIManager.OpenContainer(containerTarget);
                    break;

                case Action.Close:
                    ContainerUIManager.CloseContainer();
                    break;
            }

            return true;
        }

        #if UNITY_EDITOR

        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Actions/";
        public static new string NAME = "Inventory/Container/Container UI";

        private const string NODE_TITLE = "{0} Container UI {1}";

        public override string GetNodeTitle()
        {
            return string.Format(NODE_TITLE, this.action, this.container);
        }

        #endif
    }
}
