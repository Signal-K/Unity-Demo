using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tester : MonoBehaviour {
    public Text itemNameText; // should be TextMeshProGUI, currently debugging this (type or namespace not found)
    public Text itemDescriptionText; // should be TextMeshProGUI, currently debugging this (type or namespace not found)
    public Text itemCostText; // should be TextMeshProGUI, currently debugging this (type or namespace not found)
    public Image itemSprite;
    [Space]
    public string searchItemID; // This will be set based on whatever item we need to be generated at that point (e.g. after opening up a shop window or finishing a quest)

    public void GetNewRandomItem() {
        SetUI(Database.GetRandomItem());
    }

    public void GetItemById() {
        SetUI(Database.GetItemById(searchItemID));
    }

    private void SetUI(Item i) {
        itemNameText.text = i.itemName;
        itemDescriptionText.text = i.itemDescription;
        itemCostText.text = i.itemCost + "$";
        itemSprite.sprite = i.itemSprite;
    }
}