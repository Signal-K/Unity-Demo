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

    public void CreatePlayer() {
        currentDeviceId = Guid.NewGuid();
        PlayerPrefs.SetString("GUID", currentDeviceId.ToString());

        // Create player
        LootLockerSDKManager.StartSession(currentDeviceId.ToString(), (response) => {
            if(response.success) {
                LootLockerSDKManager.SetPlayerName(PlayerNameInputField.text, null);
                Debug.Log("Success");
            } else {
                Debug.Log("Failed, " + response.Error);
            }
        });
    }

    public void Login() {
        LootLockerSDKManager.StartSession(PlayerPrefs.GetString("GUID", ""), (response => {
            if (response.success) {
                Debug.Log("Success");
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
            } else {
                Debug.Log("Failed, " + response.Error);
            }
        });
    }
}
