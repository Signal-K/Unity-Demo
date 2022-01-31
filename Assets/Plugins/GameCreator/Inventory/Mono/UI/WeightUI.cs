namespace GameCreator.Inventory
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using GameCreator.Core;
    using GameCreator.Variables;
    using GameCreator.Core.Hooks;

    [AddComponentMenu("Game Creator/UI/Weight")]
    public class WeightUI : MonoBehaviour
    {
        public Text text;

        [Tooltip("Use {0} for the current inventory weight and {1} for the total available")]
        public string format = "{0} / {1}";

        private bool isExitingApplication = false;

        // INITIALIZE METHODS: --------------------------------------------------------------------

        private void OnEnable()
        {
            InventoryManager.Instance.eventChangePlayerInventory.AddListener(this.OnUpdate);
            this.OnUpdate();
        }

        private void OnDisable()
        {
            if (this.isExitingApplication) return;
            InventoryManager.Instance.eventChangePlayerInventory.RemoveListener(this.OnUpdate);
        }

        private void OnApplicationQuit()
        {
            this.isExitingApplication = true;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnUpdate()
        {
            float curWeight = InventoryManager.Instance.GetCurrentWeight();
            GameObject player = HookPlayer.Instance == null ? null : HookPlayer.Instance.gameObject;

            float maxWeight = DatabaseInventory
                .Load()
                .inventorySettings
                .maxInventoryWeight
                .GetValue(player);

            this.text.text = string.Format(this.format, curWeight, maxWeight);
        }
    }
}