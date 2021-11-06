using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class LevelWindow : MonoBehaviour {
    private Text levelText;
    private Image experienceBarImage;
    private LevelSystem levelSystem;
    private void Awake() {
        levelText = transform.Find("levelText").GetComponent<Text>();
        experienceBarImage = transform.Find("experienceBar").Find("bar").GetComponent<Image>();

        transform.Find("experience5Btn").GetComponent<Button_UI>().ClickFunc = () => levelSystem.AddExperience(5);
        transform.Find("experience10Btn").GetComponent<Button_UI>().ClickFunc = () => levelSystem.AddExperience(10);
        transform.Find("experience50Btn").GetComponent<Button_UI>().ClickFunc = () => levelSystem.AddExperience(50);
    }

    // Set the size of the experience bar
    private void SetExperienceBarSize(float experienceNormalized) {
        experienceBarImage.fillAmount = experienceNormalized;
    }

    private void SetLevelNumber(int levelNumber) {
        levelText.text = "LEVEL\n" + (levelNumber + 1);
    }

    public void SetLevelSystem(LevelSystem levelSystem) {
        // Set the LevelSystem object
        this.levelSystem = levelSystem;

        // Update the starting values
        SetLevelNumber(levelSystem.GetLevelNumber());
        SetExperienceBarSize(levelSystem.GetExperienceNormalized());

        // Subscribe to the changed/updated events
        levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnExperienceChanged(object sender, System.EventArgs e) {
        // Experience changed, update bar size
        SetExperienceBarSize(levelSystem.GetExperienceNormalized());
    } 

    private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e) {
        // Level changed, update text
        SetLevelNumber(levelSystem.GetLevelNumber());
    }
}