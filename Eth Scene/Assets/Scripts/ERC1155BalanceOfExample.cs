using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ERC1155BalanceOfExample : MonoBehaviour
{
    public GameObject Sphere;

    async void Start()
    {
        string chain = "ethereum";
        string network = "rinkeby";
        string contract = "0x88b48f654c30e99bc2e4a1559b4dcf1ad93fa656";
        string account = PlayerPrefs.GetString("Account"); // Test filler account
        string tokenId = "73027231590576456776384491076598269710522649071897130458994584332608519274497";

        BigInteger balanceOf = await ERC1155.BalanceOf(chain, network, contract, account, tokenId);
        print(balanceOf);

        if (balanceOf > 0) // We have the item
        {
            Sphere.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        }
    }
}
