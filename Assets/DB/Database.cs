using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Database : MonoBehaviour {
    public ItemDatabase items;
    private static Database instance; // Ensure we only ever have 1 instance of the database at any time

    private void Awake() {
        if(instance == null) { // If null, we don't currently have a database instance
            instance = this;
            DontDestroyOnLoad(gameObject); // Ensure this object is not destroyed when loading a new scene
        }
        else {
            Destroy(gameObject); // If we already have a database instance, destroy this one
        }
    }

    public static Item GetItemById(string ID) {
        // foreach(IEqualityComparer item in instance.items.allItems) {
        //     if(item.itemID == ID) 
        //         return item;
        // }

        // // If we get here, we didn't find an item with the given ID
        // return null;

        // System.Linq fetch [iem] method
        return instance.items.allItems.FirstOrDefault(i => i.itemID == ID);
    }

    //Get random item -> e.g. random items that an enemy might drop
    public static Item GetRandomItem() {
        return instance.items.allItems[Random.Range(0, instance.items.allItems.Count())];
    }
}