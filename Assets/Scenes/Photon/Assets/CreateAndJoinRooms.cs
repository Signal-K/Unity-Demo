using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks {
    public InputField createInput; // Input field for creating a room
    public InputField joinInput; // Input field for joining a room

    public void CreateRoom() {
        PhotonNetwork.CreateRoom(createInput.text); // Name of room is set to the name in the input field `createInput`
    }

    public void JoinRoom() {
        PhotonNetwork.JoinRoom(joinInput.text); // Name of room is set to the name in the input field `joinInput`
    }

    public override void OnJoinedRoom() {
        PhotonNetwork.LoadLevel("LowPolyExample"); // Loads the scene `Game` -> whenever we want to load a multiplayer scene, use PhotonNetwork.LoadLevel()
    }
}