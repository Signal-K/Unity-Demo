using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.UnityClient;

public class HighScoreController : MonoBehaviour, IMainGameEvents
{
    [Header("Game Settings")]
    [Tooltip("True means send eligible high scores to Ethereum network (can leave false when testing)")]
    public bool submitHighScores = true;

    [Header("Links to UI GameObjects")]
    public Text uiTextHighScores;
    public Text uiTextEtherBalance;

    [Header("Ethereum Settings")]
    /// <summary>
    /// The Ethereum network we will make calls to
    /// </summary>
    [Tooltip("The Ethereum network we will make calls to")]
    public string networkUrl = "https://rinkeby.infura.io";
    public string playerEthereumAccount = "0x4E77642b6C5d7c8ECDb809c60123E911aD1a2267";

    /// <summary>
    /// Remember don't ever reveal your LIVE network private key, it is not safe to store that in game code. This is a test account so it is ok.
    /// </summary>
    [Tooltip("Remember don't ever reveal your LIVE network private key, it is not safe to store that in game code. This is a test account so it is ok.")]
    public string playerEthereumAccountPK = "05be6ce86de17211a67d1fd83a94a5282a0f7e15d2e9887966161385787e82fc";

    private string contractOwnerAddress = "0x32A555F2328e85E489f9a5f03669DC820CE7EBe9";
    private string contractOwnerPK = "517311d936323b28ca55379280d3b307d354f35ae35b214c6349e9828e809adc";   
    private IEnumerator getAccountBalanceCoroutine;
    private IEnumerator getHighScoresCoroutine;
    private IEnumerator submitHighScoreCoroutine;
    private int aliveTimeMilliSeconds;

    private static HighScoreController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Prepare our coroutines            
        getAccountBalanceCoroutine = GetAccountBalanceCoroutine(); 
        getHighScoresCoroutine = GetHighScoresCoroutine(); 
        submitHighScoreCoroutine = SubmitHighScoreCoroutine();
    }

    public void GetAccountBalance()
    {
        // Get address balance 
        StopCoroutine(getAccountBalanceCoroutine);
        getAccountBalanceCoroutine = GetAccountBalanceCoroutine();
        StartCoroutine(getAccountBalanceCoroutine);
    }

    public void GetHighScores()
    {
        // Get high scores      
        StopCoroutine(getHighScoresCoroutine);
        getHighScoresCoroutine = GetHighScoresCoroutine();
        StartCoroutine(getHighScoresCoroutine);
    }

    public void SubmitHighScore()
    {
        StopCoroutine(submitHighScoreCoroutine);
        submitHighScoreCoroutine = SubmitHighScoreCoroutine();
        StartCoroutine(submitHighScoreCoroutine);
    }

    /// <summary>
    /// Check Ether balance of the player account
    /// </summary>
    public IEnumerator GetAccountBalanceCoroutine()
    {
        var getBalanceRequest = new EthGetBalanceUnityRequest(networkUrl); // New Nethereum request passing in the network you are using
        // Send balance request with player's account, asking for balance in latest block
        yield return getBalanceRequest.SendRequest(playerEthereumAccount, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());  // Sends the request with the player's eth account and adding that we want the latest block

        if(getBalanceRequest.Exception == null) { // Checks for an error in the call
            var balance = getBalanceRequest.Result.Value; // Extracts the balance
            uiTextEtherBalance = Nethereum.Util.UnitConversion.Convert.FromWei(balance, 18).ToString("n8"); // Unit conversion from Wei to Ether
        }
        else {
            Debug.Log("Get account balance gave an exception: " + getBalanceRequest.Exception.Message);
        }
    }

    /// <summary>
    /// Get high scores
    /// </summary>
    public IEnumerator GetHighScoresCoroutine()
    {
        yield return null;
    }

    private string PrettifyScore(int score)
    {
        float scoreFloat = 0f;
        string scoreString;
        scoreFloat = (float)score / 1000f;
        scoreString = scoreFloat.ToString("n3");
        return scoreString.PadLeft(8, ' ');
    }

    private string PrettifyAddress(string addr)
    {
        if (addr.ToLowerInvariant() == playerEthereumAccount.ToLowerInvariant())
        {
            return "Me";
        }
        else
        {
            return addr;
        }
    }

    public void OnRefreshButtonPressed()
    {
        GetHighScores();
        GetAccountBalance();
    }

    /// <summary>
    /// Submit a high score
    /// </summary>   
    public IEnumerator SubmitHighScoreCoroutine()
    {
        yield return null;
    }

    void IMainGameEvents.OnGameOver(float aliveTimeSeconds)
    {
        if (submitHighScores)
        {
            // Store the new high score
            aliveTimeMilliSeconds = (int)(aliveTimeSeconds * 1000f);
            SubmitHighScore();
        }
    }

    // have to implement for interface, but dont care about it for now
    void IMainGameEvents.OnGameStarted() { }
}
