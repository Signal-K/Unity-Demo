using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public int health = 5;
    public int experience = 40;
    public int currency = 1000;

    public Quest quest; // Turn this into a list/array to add multiple quests at a time (or there can be multiple types of quests). This isn't necessarily to avoid having to deal with lists, but since there will be different quest types we obviously need to have them set as different variables, not in the same list
    // public NPCQuest npcquest; // an e.g.

    public void QuestBattle () {
        // Scaffolding for the battle system
        health -= 1;
        experience += 2;
        currency += 5;
    }
}