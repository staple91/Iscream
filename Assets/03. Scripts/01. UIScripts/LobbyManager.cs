using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace LeeJungChul
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        private readonly string gameVersion = "1";

        [Header("네트워크 정보를 표시할 텍스트")]
        public TextMeshProUGUI connectionInfoText;
        [Header("룸 접속 버튼")]
        public Button joinButton;
        [Header("로딩 화면")]
        public GameObject panel;

        private void Start()
        {
            Screen.SetResolution(1920, 1080, false);
            PhotonNetwork.GameVersion = gameVersion;

            PhotonNetwork.ConnectUsingSettings();

            joinButton.interactable = false;

            connectionInfoText.text = "마스터 서버에 접속중...";
        }

        public override void OnConnectedToMaster()
        {
            joinButton.interactable = true;
            connectionInfoText.text = "온라인 : 마스터 서버와 연결됨";
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            joinButton.interactable = false;

            connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음 \n 접속 재시도 중...";

            PhotonNetwork.ConnectUsingSettings();
        }

        public void Connect()
        {
            joinButton.interactable = false;
            panel.SetActive(true);

            if (PhotonNetwork.IsConnected)
            {
                connectionInfoText.text = "룸에 접속...";
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n 접속 재시도 중...";
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            connectionInfoText.text = "빈 방이 없음, 새로운 방 생성...";

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
        }

        public override void OnJoinedRoom()
        {
            connectionInfoText.text = "방 참가 성공";

            PhotonNetwork.LoadLevel("LobbyScene");
        }
    }
}

