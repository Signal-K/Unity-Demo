namespace GameCreator.Inventory
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using GameCreator.Core;

    [AddComponentMenu("Game Creator/UI/Currency")]
    public class CurrencyUI : MonoBehaviour
    {
        public Text text;

        private bool isExitingApplication = false;

        // INITIALIZE METHODS: --------------------------------------------------------------------

        private void OnEnable()
        {
            InventoryManager.Instance.eventChangePlayerCurrency.AddListener(this.OnUpdate);
            this.OnUpdate();
        }

        private void OnDisable()
        {
            if (this.isExitingApplication) return;
            InventoryManager.Instance.eventChangePlayerCurrency.RemoveListener(this.OnUpdate);
        }

        private void OnApplicationQuit()
        {
            this.isExitingApplication = true;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnUpdate()
        {
            int currency = InventoryManager.Instance.GetCurrency();
            this.text.text = currency.ToString();
        }
    }
}