/*
 * Copyright (c) 2018 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.UnityClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

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
    private HighScoreContractWrapper scoreContractService;
    private int aliveTimeMilliSeconds;
    /// <summary>
    /// This is the most gas that can be consumed by a transaction, else it will be rejected
    /// It is a "nothing up my sleeve" number = PI * 1.5
    /// </summary>
    private HexBigInteger gasLimit = new HexBigInteger(4712388);


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
        
        // Service for interaction with high score contract
        scoreContractService = new HighScoreContractWrapper();
    }

    public void GetAccountBalance()
    {
        // 2) Get address balance 
        StopCoroutine(getAccountBalanceCoroutine);
        getAccountBalanceCoroutine = GetAccountBalanceCoroutine();
        StartCoroutine(getAccountBalanceCoroutine);
    }

    public void GetHighScores()
    {
        // 3) Get high scores      
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
        var getBalanceRequest = new EthGetBalanceUnityRequest(networkUrl);
        // Send balance request with player's account, asking for balance in latest block
        yield return getBalanceRequest.SendRequest(playerEthereumAccount, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());

        if (getBalanceRequest.Exception == null)
        {
            var balance = getBalanceRequest.Result.Value;
            // Convert the balance from wei to ether and round to 8 decimals for display
            uiTextEtherBalance.text = Nethereum.Util.UnitConversion.Convert.FromWei(balance, 18).ToString("n8");
        }
        else
        {
            Debug.Log("RW: Get Account Balance gave an exception: " + getBalanceRequest.Exception.Message);
        }
    }

    /// <summary>
    /// Get high scores
    /// </summary>
    public IEnumerator GetHighScoresCoroutine()
    {
        var topScoreRequest = new EthCallUnityRequest(networkUrl);
        // Use the service to create a call input
        var countTopScoresCallInput = scoreContractService.CreateCountTopScoresCallInput();
        // Call request sends and yield for response	
        yield return topScoreRequest.SendRequest(countTopScoresCallInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());

        // Decode the top score using the service
        var scores = new List<HighScoreDataTypeDefinition>();
        var count = scoreContractService.DecodeTopScoreCount(topScoreRequest.Result);
        Debug.Log("RW: High Score count: " + count);
        for (int i = 0; i < count; i++)
        {
            topScoreRequest = new EthCallUnityRequest(networkUrl);
            var topScoreCallInput = scoreContractService.CreateTopScoresCallInput(i);
            yield return topScoreRequest.SendRequest(topScoreCallInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());
            scores.Add(scoreContractService.DecodeTopScoreDTO(topScoreRequest.Result));
        }

        var orderedScores = scores.OrderByDescending(x => x.Score).ToList();
        var topScores = "";
        foreach (var score in orderedScores)
        {
            string playerAddress = PrettifyAddress(score.Addr);
            string playerSCore = PrettifyScore(score.Score);
            topScores = topScores + playerSCore + " : " + playerAddress + Environment.NewLine;
        }
        uiTextHighScores.text = topScores;
    }

    private string PrettifyScore(BigInteger score)
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
        // Create the transaction input with encoded values for the function      
        var transactionInput = scoreContractService.CreateSetTopScoreTransactionInput(playerEthereumAccount, contractOwnerAddress, contractOwnerPK, aliveTimeMilliSeconds, gasLimit);

        // Create Unity Request with the private key, url and user address       
        var transactionSignedRequest = new TransactionSignedUnityRequest(networkUrl, playerEthereumAccountPK, playerEthereumAccount);

        // Send request and wait
        yield return transactionSignedRequest.SignAndSendTransaction(transactionInput);

        if (transactionSignedRequest.Exception == null)
        {
            // Get transaction receipt
            Debug.Log("RW: Top score submitted tx: " + transactionSignedRequest.Result);
        }
        else
        {
            Debug.Log("RW: Error submitted tx: " + transactionSignedRequest.Exception.Message);
        }
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
