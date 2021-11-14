using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public int health = 5;
    public int experience = 40;
    public int currency = 1000;

    public void QuestBattle () {
        // Scaffolding for the battle system
        health -= 1;
        experience += 2;
        currency += 5;
    }
}