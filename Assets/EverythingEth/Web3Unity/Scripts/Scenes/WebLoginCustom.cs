/*using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WebLoginCustom: MonoBehaviour {
    [DllImport("__Internal")]
    private static extern void Web3Connect();
    [DllImport("__Internal")]
    private static extern string ConnectAccount();
    [DllImport("__Internal")]
    private static extern void SetConnectAccount(string value);

    private int expirationTime;
    private string account;

    public void OnLogin() {
#if UNITY_WEBGL
        Web3Connect();
#endif
        OnConnected();
    }

    async private void OnConnected() {
#if UNITY_WEBGL
        account = ConnectAccount();
        while (account == "") {
            await new WaitForSeconds(1f);
            account = ConnectAccount();
        };
        SetConnectAccount("");
#else
        account = "0xCdc5929e1158F7f0B320e3B942528E6998D8b25c";
#endif
        // Save account for next scene
        PlayerPrefs.SetString("Account", account.ToLower());

        // Reset login message

        // Load next scene
        SceneManager.LoadScene(SceneManager.GetActivated().buildIndex + 1);
    }

    public void OnSkip() {
        // Test account so user can skip sign-in process temporarily
        PlayerPrefs.SetString("Account", "");
        // Move to next scene
        SceneManager.LoadScene(SceneManager.GetActivated().buildIndex + 1);
    }
}*/