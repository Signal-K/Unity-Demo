using UnityEngine;
using LootLocker.Requests;

public class Lootlocker : MonoBehaviour
{
    void Start()
    {
    //    return;
    //    LootLockerSDKManager.StartGuestSession((response) =>
    //    {
    //        if (!response.success)
    //        {
    //            Debug.Log("error starting LootLocker session");

    //            return;
    //        }

    //        Debug.Log("successfully started LootLocker session");
    //    });

    //    LootLockerSDKManager.SetPlayerName("Chris", (response) =>
    //    {
    //        if (response.success)
    //        {
    //            Debug.Log("Successfully set player name");
    //        }
    //        else
    //        {
    //            Debug.Log("Error setting player name");
    //        }
    //    });

    //    LootLockerSDKManager.GetInventory((response) =>
    //    {
    //        if (response.success)
    //        {
    //            Debug.Log("Successfully retrieved player inventory: " + response.inventory.Length);

    //            foreach(var item in response.inventory)
    //            {
    //                Debug.Log("fill item with " + item.asset.name);

    //                switch (item.asset.name)
    //                {
    //                    case "Item_1":
    //                        Player.Instance.backpack.AddItem(Item.Type.rabbit, 1);
    //                        break;
    //                    case "Item_2":
    //                        Player.Instance.backpack.AddItem(Item.Type.sun, 1);
    //                        break;
    //                    case "Item_3":
    //                        Player.Instance.backpack.AddItem(Item.Type.spade, 1);
    //                        break;
    //                }
    //            }

    //            Store.Instance.window_1.RefreshItems(0);
    //            Store.Instance.window_2.RefreshItems(0);
    //        }
    //        else
    //        {
    //            Debug.Log("Error getting player inventory");
    //        }
    //    });
    }
}
