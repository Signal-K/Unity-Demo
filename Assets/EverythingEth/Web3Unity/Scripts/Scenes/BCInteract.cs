using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Used by metadata class for storing attributes
public class Attributes {
    // Type or name of given trait
    public string trait_type;
    // Value associated with trait_type
    public string value;
}

// Storing NFT metadata from standard NFT json files
public class Metadata {
    // List storing attributes of the NFT
    public List<Attributes> attributes { get; set; }

    // Description of NFT
    public string description { get; set; }

    // Exrternal link for NFT
    public string external_url { get; set; }

    //Image stores the NFT's URI for image NFTs
    public string image { get; set; }

    // Name of NFT
    public string name { get; set; }
}

// Interacting with the blockchain
public class BCInteract : MonoBehaviour {
    // Chain to interact with (using Polygon)
    public string chain = "polygon";
    public string network = "mainnet";
    public string contract = "";
    public string tokenID = "0";
    Metadata metadata;

    private void Start() {
        // Starts async function to get the NFT image
        GetNFTImage();
    }

    async private void GetNFTImage() {
        string URI = await ERC721.URI(chain, network, contract, tokenID); // Interacts with the blockchain to find the URI related to the tokenID

        // Perform async request to get JSON file from the URI
        using (UnityWebRequest webRequest = UnityWebRequest.Get(URI)) {
            await webRequest.SendWebRequest(); // sends web request
            string metadataString = webRequest.downloadHandler.text; // texct from web request
            metadata = JsonConvert.DeserializeObject<Metadata>(metadataString); // Converts the metadata string to the Metadata object
        }

        // Perform another web request to collect the image RELATED TO the URI
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(metadata.image)) {
            // Sends webRequest
            await webRequest.SendWebRequest();

            // Get image from web request and store it as a texture
            Texture texture = DownloadHandlerTexture.GetContent(webRequest);
            // Set the object's main render material to the texture
            GetComponent<MeshRenderer>().material.mainTexture = texture;
        }
    }
}