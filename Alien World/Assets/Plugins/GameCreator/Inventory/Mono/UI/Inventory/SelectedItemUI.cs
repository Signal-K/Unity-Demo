using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameCreator.Inventory
{
    [AddComponentMenu("Game Creator/UI/Item Selected")]
    public class SelectedItemUI : ItemUI
    {
        private static readonly Dictionary<int, SelectedItemUI> SelectedItemUIs =
            new Dictionary<int, SelectedItemUI>();
        
        public UnityEvent OnDeselect;
        public UnityEvent OnSelect;

        private void Awake()
        {
            SelectedItemUIs[this.GetInstanceID()] = this;
            
            this.UpdateUI(null, 0);
            this.OnDeselect.Invoke();
        }

        private void OnDestroy()
        {
            SelectedItemUIs.Remove(this.GetInstanceID());
        }

        public static void Select(int uuid)
        {
            foreach (KeyValuePair<int,SelectedItemUI> entry in SelectedItemUIs)
            {
                if (entry.Value != null)
                {
                    if (InventoryManager.Instance.itemsCatalogue.TryGetValue(uuid, out Item item))
                    {
                        int amount = InventoryManager.Instance.GetInventoryAmountOfItem(uuid);
                
                        entry.Value.UpdateUI(item, amount);
                        entry.Value.OnSelect.Invoke();
                    }
                }
            }
        }
        
        public static void Deselect()
        {
            foreach (KeyValuePair<int,SelectedItemUI> entry in SelectedItemUIs)
            {
                if (entry.Value != null)
                {
                    entry.Value.UpdateUI(null, 0);
                    entry.Value.OnDeselect.Invoke();
                }
            }
        }
    }
}