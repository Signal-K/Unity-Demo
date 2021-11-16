using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal {
    public Player player;

    public GoalType goalType;

    public int requiredAmount;
    public int currentAmount;

    public bool IsReached() { // Has the goal been reached?
       return (currentAmount >= requiredAmount);
    }

    public void MaterialGathered() { 
        /*
        For demo purposes:
        The demo quest is collecting materials, this function is run whenever a material is collected
        When the currentAmount variable is >= requiredAmount, the quest is finished!
        
        This is a demo method to create an update for the currentAmount, however later we'll create a proper scene to show this off
        */

        if(goalType == GoalType.Gathering) // Only do this if the goal for the quest is to gather materials
            currentAmount++;
    }

    public void EnemyKilled() {
        if(goalType == GoalType.Kill)
            currentAmount++;
    }


    // Specific quest types scaffolding
    public void FightEnemy() {
        player.health -= 1;
        EnemyKilled();
    }
}

public enum GoalType {
    Kill,
    Gathering
}