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
/*
using Nethereum.ABI.Encoders;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using System.Numerics;

/// <summary>
/// This class wraps our Ethereum Smart Contract (which can be examined here https://rinkeby.etherscan.io/address/0xcd602de18ef419253da7be962d3f1d05da3dea33), making 
/// it easier to interact with in this project.
/// The code is a reduced and simplified version of the Nethereum sample https://github.com/Nethereum/Nethereum.Flappy/blob/master/Scripts/TopScoreService.cs
/// Notice this is not a MonoBehaviour.
/// </summary>
public class HighScoreContractWrapper
{
    public static string contractABI = @"[{'constant':false,'inputs':[{'name':'score','type':'int256'},{'name':'v','type':'uint8'},{'name':'r','type':'bytes32'},{'name':'s','type':'bytes32'}],'name':'setTopScore','outputs':[],'payable':false,'type':'function'},{'constant':true,'inputs':[{'name':'','type':'uint256'}],'name':'topScores','outputs':[{'name':'addr','type':'address'},{'name':'score','type':'int256'}],'payable':false,'type':'function'},{'constant':false,'inputs':[],'name':'getCountTopScores','outputs':[{'name':'','type':'uint256'}],'payable':false,'type':'function'},{'constant':true,'inputs':[{'name':'','type':'address'}],'name':'userTopScores','outputs':[{'name':'','type':'int256'}],'payable':false,'type':'function'},{'inputs':[],'payable':false,'type':'constructor'}]";

    private static string contractAddress = "0xcd602de18ef419253da7be962d3f1d05da3dea33";
    private Contract contract;

    public HighScoreContractWrapper()
    {
        this.contract = new Contract(null, contractABI, contractAddress);
    }
    
    public Function GetFunctionTopScores()
    {
        return contract.GetFunction("topScores");
    }
    
    public Function GetFunctionGetCountTopScores()
    {
        return contract.GetFunction("getCountTopScores");
    }

    public CallInput CreateTopScoresCallInput(BigInteger index)
    {
        var function = GetFunctionTopScores();
        return function.CreateCallInput(index);
    }
        
    public CallInput CreateCountTopScoresCallInput()
    {
        var function = GetFunctionGetCountTopScores();
        return function.CreateCallInput();
    }
    
    public Function GetFunctionSetTopScore()
    {
        return contract.GetFunction("setTopScore");
    }
    
    public TransactionInput CreateSetTopScoreTransactionInput(string addressFrom, string addressOwner, string privateKey, BigInteger score, HexBigInteger gas = null, HexBigInteger valueAmount = null)
    {
        var numberBytes = new IntTypeEncoder().Encode(score);
        var sha3 = new Nethereum.Util.Sha3Keccack();
        var hash = sha3.CalculateHashFromHex(addressFrom, addressOwner, numberBytes.ToHex());
        var signer = new MessageSigner();
        var signature = signer.Sign(hash.HexToByteArray(), privateKey);
        var ethEcdsa = MessageSigner.ExtractEcdsaSignature(signature);

        var function = GetFunctionSetTopScore();
        return function.CreateTransactionInput(addressFrom, gas, valueAmount, score, ethEcdsa.V, ethEcdsa.R, ethEcdsa.S);
    }

    public int DecodeTopScoreCount(string result)
    {
        var function = GetFunctionGetCountTopScores();
        return function.DecodeSimpleTypeOutput<int>(result);
    }
    
    public HighScoreDataTypeDefinition DecodeTopScoreDTO(string result)
    {
        var function = GetFunctionTopScores();
        return function.DecodeDTOTypeOutput<HighScoreDataTypeDefinition>(result);
    }
}

[FunctionOutput]
public class HighScoreDataTypeDefinition
{
    [Parameter("address", "addr", 1)]
    public string Addr { get; set; }

    [Parameter("int256", "score", 2)]
    public BigInteger Score { get; set; }
}
*/
