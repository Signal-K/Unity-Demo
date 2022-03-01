using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_WEBGL
public class WebGLSendContractExample : MonoBehaviour
{
    async public void OnSendContract()
    {
        // smart contract method to call
        string method = "addTotal";
        // abi in json format
        string abi = "[ { \"inputs\": [ { \"internalType\": \"uint8\", \"name\": \"_myArg\", \"type\": \"uint8\" } ], \"name\": \"addTotal\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"myTotal\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
        // address of contract
        string contract = "0xd9145CCE52D386f254917e481eB44e9943F39138"; // Deployed on remix.ethereum.org
        // array of arguments for contract
        string args = "[\"1\"]"; // Incrementing by 1
        // value in wei
        string value = "0"; // Starting value
        // gas limit OPTIONAL
        string gasLimit = ""; // Metamask automatically configures the gas limits/values
        // gas price OPTIONAL
        string gasPrice = "";
        // connects to user's browser wallet (metamask) to update contract state
        try {
            string response = await Web3GL.SendContract(method, abi, contract, args, value, gasLimit, gasPrice);
            Debug.Log(response);
        } catch (Exception e) {
            Debug.LogException(e, this);
        }
    }
}
#endif