using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LootLocker.Requests;
using System;

public class GameManager : MonoBehaviour {
    public UnityEngine.UI.InputField PlayerNameInputField;
    public GameObject CreatePlayerButton;
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
                LootLockerSDKManager.SetPlayerName(PlayerNameInputField.text, null);
                Debug.Log("Success");
            } else {
                Debug.Log("Failed, " + response.Error);
            }
        }));
    }
}
