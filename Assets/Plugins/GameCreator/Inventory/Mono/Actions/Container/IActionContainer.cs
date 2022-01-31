namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
    using GameCreator.Core;

	[AddComponentMenu("")]
	public abstract class IActionContainer : IAction
	{
        public TargetGameObject container = new TargetGameObject(TargetGameObject.Target.Invoker);

        protected Container GetContainer(GameObject target)
        {
            GameObject containerGo = this.container.GetGameObject(target);
            if (containerGo == null) return null;

            return containerGo.GetComponent<Container>();
        }
    }
}
