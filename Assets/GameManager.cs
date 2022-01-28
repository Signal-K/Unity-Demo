using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LootLocker.Requests;
using System;

public class GameManager : MonoBehaviour {
    public UnityEngine.UI.InputField PlayerNameInputField, XpIncreaseInputField;
    public GameObject CreatePlayerButton;
    public GameObject LoginButton;
    Guid currentDeviceId;
    public Slider LevelSlider;
    public GameObject GiveXPButton;
    public Text CurrentLevelText, NextLevelText, CurrentXpText;

    public void CreatePlayer() {
        currentDeviceId = Guid.NewGuid();
        PlayerPrefs.SetString("GUID", currentDeviceId.ToString());

        // Create player
        LootLockerSDKManager.StartSession(currentDeviceId.ToString(), (response) => {
            if(response.success) {
                CreatePlayerButton.SetActive(false);
                LoginButton.SetActive(false);
                PlayerNameInputField.gameObject.SetActive(false);

                GiveXPButton.SetActive(true);
                LevelSlider.gameObject.SetActive(true);
                XpIncreaseInputField.gameObject.SetActive(true);
                LootLockerSDKManager.SetPlayerName(PlayerNameInputField.text, null);
                Debug.Log("Success");
                CheckLevel();
            } else {
                Debug.Log("Failed, " + response.Error);
            }
        });
    }

    public void Login() {
        LootLockerSDKManager.StartSession(PlayerPrefs.GetString("GUID", ""), (response => {
            if (response.success) {
                CreatePlayerButton.SetActive(false);
                LoginButton.SetActive(false);
                PlayerNameInputField.gameObject.SetActive(false);

                GiveXPButton.SetActive(true);
                LevelSlider.gameObject.SetActive(true);
                XpIncreaseInputField.gameObject.SetActive(true);
                Debug.Log("Success");
                CheckLevel();
            } else {
                Debug.Log("Failed, " + response.Error);
            }
        }));
    }

    public void GiveXP() {
        LootLockerSDKManager.SubmitXp(int.Parse(XpIncreaseInputField.text), (response) => {
            if (response.success) {
                CreatePlayerButton.SetActive(false);
                LoginButton.SetActive(false);
                PlayerNameInputField.gameObject.SetActive(false);
                Debug.Log("Success");
                CheckLevel();
            } else {
                Debug.Log("Failed, " + response.Error);
            }
        });
    }

    public void CheckLevel() {
        LootLockerSDKManager.GetPlayerInfo((response) => {
            CurrentLevelText.text = response.level.ToString();
            NextLevelText.text = (response.level + 1).ToString();
            CurrentXpText.text = response.xp.ToString() + " / " + response.level_thresholds.next.ToString(); // Displays how much xp we currently have and how much XP is needed to level up

            if(LevelSlider.value == LevelSlider.maxValue) { // Xp threshold for levelling up met
                LevelSlider.maxValue = response.level_thresholds.next - (float)response.xp - int.Parse(XpIncreaseInputField.text);
                LevelSlider.value = 0;
                // This assumes that the threshold/difference between levels will always be 100xp. Once the player gets to the next set of 100, the slider will go back to 0 (the xp total won't). We can change this if we want to have different thresholds by turning the float for xp into a percentage of the total xp needed to reach the next level versus the xp the player has now
            } else {
                LevelSlider.value = (float)response.xp - response.level_thresholds.current;
            }
        });
    }
}
