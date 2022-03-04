using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCallExample : MonoBehaviour
{
    async void Start()
    {
        /*
        // SPDX-Licenese-Identifier: MIT
        pragma solidity ^0.8.0;

        contract SubtractToken {
            // Demo function that subtracts a value when it's clicked/interacted with in Unity
            uint256 public tokenTotal = 0;

            function addTotal(uint8 _myArg) public {
                // Say if they were buying a token
                tokenTotal = tokenTotal + _myArg;
            }

            function subtractTotal(uint8 _myArg) public {
                // If end user is using a token
                tokenTotal = tokenTotal - _myArg;
            }
        }

        */
        // set chain: ethereum, moonbeam, polygon etc
        string chain = "ethereum";
        // set network mainnet, testnet
        string network = "rinkeby";
        // smart contract method to call
        string method = "tokenTotal";
        // abi in json format
        string abi = "[ { \"inputs\": [ { \"internalType\": \"uint8\", \"name\": \"_myArg\", \"type\": \"uint8\" } ], \"name\": \"addTotal\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"myTotal\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
        // address of contract
        string contract = "0xd9145CCE52D386f254917e481eB44e9943F39138";
        // array of arguments for contract
        string args = "[]";
        // connects to user's browser wallet to call a transaction
        string response = await EVM.Call(chain, network, contract, abi, method, args); // Pass in all params
        // display response in game
        print(response);
    }
}

/* ABI
"inputs": [
			{
				"internalType": "uint8",
				"name": "_myArg",
				"type": "uint8"
			}
		],
		"name": "addTotal",
		"outputs": [],
		"stateMutability": "nonpayable",
		"type": "function"
	},
	{
		"inputs": [
			{
				"internalType": "uint8",
				"name": "_myArg",
				"type": "uint8"
			}
		],
		"name": "subtractTotal",
		"outputs": [],
		"stateMutability": "nonpayable",
		"type": "function"
	},
	{
		"inputs": [],
		"name": "tokenTotal",
		"outputs": [
			{
				"internalType": "uint256",
				"name": "",
				"type": "uint256"
			}
		],
		"stateMutability": "view",
		"type": "function"*/