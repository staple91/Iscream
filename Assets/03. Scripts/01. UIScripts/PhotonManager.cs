using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace LeeJungChul
{
    public class PhotonManager : MonoBehaviourPunCallbacks
    {
        public TMP_InputField nickNameInputField;           // 닉네임 입력받는 곳.
        public TMP_Dropdown maxPlayerDropDown;              // 최대 인원 몇 명까지 할지.
        public TMP_Dropdown gamePlayTimeDropDown;           // 게임 시간은 몇 초로 정할 건지.

        public GameObject loadingUi;                        // 로딩 UI.
        public TextMeshProUGUI currentPlayerCountText;      // 로딩 UI 중에서 현재 인원 수를 나타냄.

        void Awake()
        {
            // 마스터 클라이언트는 PhotonNetwork.LoadLevel()를 호출할 수 있고, 모든 연결된 플레이어는 자동적으로 동일한 레벨을 로드한다.
            PhotonNetwork.AutomaticallySyncScene = true;

            loadingUi.SetActive(false);

            Screen.SetResolution(1960, 1080, true);
        }

        void Start()
        {
            Debug.Log("서버 연결 시도.");
            PhotonNetwork.ConnectUsingSettings();
        }

        /// <summary>
        /// 랜덤 방 만들기 함수
        /// </summary>
        public void JoinRandomOrCreateRoom()
        {
            string nick = nickNameInputField.text;

            if (nick.Length > 0)
            {
                Debug.Log($"{nick} 랜덤 매칭 시작.");
                PhotonNetwork.LocalPlayer.NickName = nick;  // 현재 플레이어 닉네임 설정

                // UI에서 값 얻어오기.
                byte maxPlayers = byte.Parse(maxPlayerDropDown.options[maxPlayerDropDown.value].text); // 드롭다운에서 값 얻어오기.
                int maxTime = int.Parse(gamePlayTimeDropDown.options[gamePlayTimeDropDown.value].text);

                RoomOptions roomOptions = new RoomOptions();
                roomOptions.MaxPlayers = maxPlayers; // 인원 지정.
                roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "maxTime", maxTime } }; // 게임 시간 지정.
                roomOptions.CustomRoomPropertiesForLobby = new string[] { "maxTime" }; // 여기에 키 값을 등록해야, 필터링이 가능하다.

                // 방 참가를 시도하고, 실패하면 생성해서 참가함.
                PhotonNetwork.JoinRandomOrCreateRoom
                (
                    expectedCustomRoomProperties: new ExitGames.Client.Photon.Hashtable() { { "maxTime", maxTime } },
                    expectedMaxPlayers: maxPlayers, // 참가할 때의 기준.
                    roomOptions: roomOptions // 생성할 때의 기준.
                );
            }
            else
            {
                Debug.Log("닉네임을 설정하세요");
            }
        }

        /// <summary>
        /// 매칭 취소 함수
        /// </summary>
        public void CancelMatching()
        {
            Debug.Log("매칭 취소.");
            loadingUi.SetActive(false);

            Debug.Log("방 떠남.");
            PhotonNetwork.LeaveRoom();
        }

        /// <summary>
        /// 플레이어 카운트 실시간 반영 함수
        /// </summary>
        private void UpdatePlayerCounts()
        {
            currentPlayerCountText.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
        }

        #region 포톤 콜백 함수

        public override void OnConnectedToMaster()
        {
            Debug.Log("서버 접속 완료.");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("방 참가 완료.");

            Debug.Log($"{PhotonNetwork.LocalPlayer.NickName}은 인원수 {PhotonNetwork.CurrentRoom.MaxPlayers} 매칭 기다리는 중.");
            Debug.Log($"{ PhotonNetwork.CurrentRoom.PlayerCount }");
            UpdatePlayerCounts();

            loadingUi.SetActive(true);
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            Debug.Log($"플레이어 {newPlayer.NickName} 방 참가.");
            UpdatePlayerCounts();

            if (PhotonNetwork.IsMasterClient)
            {
                // 목표 인원 수 채웠으면, 맵 이동을 한다. 권한은 마스터 클라이언트만.

                if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    PhotonNetwork.LoadLevel("PracticeScene");                   
                }
            }
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            Debug.Log($"플레이어 {otherPlayer.NickName} 방 나감.");
            UpdatePlayerCounts();
        }
        #endregion
    }
}
