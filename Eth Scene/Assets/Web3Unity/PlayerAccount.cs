using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAccount : MonoBehaviour
{
    public Text playerAccount;

    // Start is called before the first frame update
    void Start()
    {
        string account = PlayerPrefs.GetString("Account");
        playerAccount.text = account;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
