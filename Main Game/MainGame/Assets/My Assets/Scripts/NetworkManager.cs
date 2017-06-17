using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{

    const string VERSION = "v0.0.1";
    public string roomName = "MainGame";

    public string playerPrefabString = "Player Character";
    public GameObject spawnLocation;

    // Use this for initialization
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(VERSION);
    }

    void OnJoinedLobby()
    {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = false, MaxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate(playerPrefabString, spawnLocation.transform.position, spawnLocation.transform.rotation, 0);
    }
}
