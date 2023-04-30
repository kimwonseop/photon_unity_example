using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks {
    public TMP_InputField NicknameInput;
    public GameObject RespawnPanel;
    public GameObject ConnectPanel;

    public void Awake() {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnJoinedRoom() {
        ConnectPanel.SetActive(false);
        // Cursor.lockState = CursorLockMode.Locked;

        Spwan();
    }

    public override void OnDisconnected(DisconnectCause cause) {
        ConnectPanel.SetActive(true);
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.LocalPlayer.NickName = NicknameInput.text;
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions {MaxPlayers = 5}, null);
    }

    public void Spwan() {
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        // RespawnPanel.SetActive(false);
    }
}
