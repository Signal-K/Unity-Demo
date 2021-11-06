using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LootLocker.Requests;
using System;

public class GameManager : MonoBehaviour
{
    public InputField PlayerNameInputField, XpIncreaseInputField;
    public GameObject CreatePlayerButton;
    Guid currentDeviceID;

    public void GiveXP() {
        LootLockerSDKManager.SubmitXp(int.Parse(XpIncreaseInputField.text), (response) => {
            if (response.success) {
                Debug.Log("Success");
            }
            else {
                Debug.Log("Failed" + response.Error);
            }
        });
    }

    public void CreatePlaye() {
        currentDeviceID = Guid.NewGuid();
        PlayerPrefs.SetString("GUID", currentDeviceID.ToString());

        LootLockerSDKManager.StartSession(currentDeviceID.ToString(), (response) => {
            if(response.success) {
                LootLockerSDKManager.SetPlayerName(PlayerNameInputField.text, null);
                Debug.Log("Success");
            }
            else {
                Debug.Log("Failed" + response.Error);
            }
        });
    }

    public void Login() {
        LootLockerSDKManager.StartSession(PlayerPrefs.GetString("GUID", ""), (response) =>
        {
            if(response.success) {
                Debug.Log("Success");
            }
            else {
                Debug.Log("Failed" + response.Error);
            }
        });
    }
}
