using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal {
    public GoalType goalType;

    public int requiredAmount;
    public int currentAmount;

    public bool IsReached() { // Has the goal been reached?
       return (currentAmount >= requiredAmount);
    }
}

public enum GoalType {
    Kill,
    Gathering
}