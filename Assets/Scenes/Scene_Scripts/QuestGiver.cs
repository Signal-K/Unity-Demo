using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour {
    public Quest quest;

    public Player player;

    // References
    public GameObject questWindow;
    public Text titleText;
    public Text descriptionText;
    //public Text experienceText;
    //public Text currencyText;

    public void OpenQuestWindow() {
        questWindow.SetActive(true);
        titleText.text = quest.title;
        descriptionText.text = quest.description;
        //experienceText.text = quest.experienceReward.ToString(); -> Commenting out for now as this won't be displayed in the quest window
        //currencyText.text = quest.currencyReward.ToString(); // Instead it will be displayed in the quest journal/handbook
    }

    public void AcceptQuest() {
        questWindow.SetActive(false);
        quest.isActive = true;

        // Give the quest to the player
        player.quest = quest;
    }
}