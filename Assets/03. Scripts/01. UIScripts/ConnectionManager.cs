using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField m_inputField_Nickname; // 닉네임 입력받는 곳.
    public TMP_Dropdown m_dropdown_RoomMaxPlayers; // 최대 인원 몇 명까지 할지.
    public TMP_Dropdown m_dropdown_MaxTime; // 게임 시간은 몇 초로 정할 건지.

    public GameObject m_panel_Loading; // 로딩 UI.
    public TextMeshProUGUI m_text_CurrentPlayerCount; // 로딩 UI 중에서 현재 인원 수를 나타냄.

    void Awake()
    {
        // 마스터 클라이언트는 PhotonNetwork.LoadLevel()를 호출할 수 있고, 모든 연결된 플레이어는 자동적으로 동일한 레벨을 로드한다.
        PhotonNetwork.AutomaticallySyncScene = true;
        
        m_panel_Loading.SetActive(false);
    }

    void Start()
    {

        print("서버 연결 시도.");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinRandomOrCreateRoom()
    {
        string nick = m_inputField_Nickname.text;

        print($"{nick} 랜덤 매칭 시작.");
        PhotonNetwork.LocalPlayer.NickName = nick; // 현재 플레이어 닉네임 설정하기.

        // UI에서 값 얻어오기.
        byte maxPlayers = byte.Parse(m_dropdown_RoomMaxPlayers.options[m_dropdown_RoomMaxPlayers.value].text); // 드롭다운에서 값 얻어오기.
        int maxTime = int.Parse(m_dropdown_MaxTime.options[m_dropdown_MaxTime.value].text);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers; // 인원 지정.
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "maxTime", maxTime } }; // 게임 시간 지정.
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "maxTime" }; // 여기에 키 값을 등록해야, 필터링이 가능하다.

        // 방 참가를 시도하고, 실패하면 생성해서 참가함.
        PhotonNetwork.JoinRandomOrCreateRoom(
            expectedCustomRoomProperties: new ExitGames.Client.Photon.Hashtable() { { "maxTime", maxTime } }, expectedMaxPlayers: maxPlayers, // 참가할 때의 기준.
            roomOptions: roomOptions // 생성할 때의 기준.
        );
    }

    public void CancelMatching()
    {
        print("매칭 취소.");
        m_panel_Loading.SetActive(false);

        print("방 떠남.");
        PhotonNetwork.LeaveRoom();
    }

    private void UpdatePlayerCounts()
    {
        m_text_CurrentPlayerCount.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
    }

    #region 포톤 콜백 함수

    public override void OnConnectedToMaster()
    {
        print("서버 접속 완료.");

    }
    public override void OnJoinedRoom()
    {
        print("방 참가 완료.");

        Debug.Log($"{PhotonNetwork.LocalPlayer.NickName}은 인원수 {PhotonNetwork.CurrentRoom.MaxPlayers} 매칭 기다리는 중.");
        UpdatePlayerCounts();

        m_panel_Loading.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"플레이어 {newPlayer.NickName} 방 참가.");
        UpdatePlayerCounts();

        if (PhotonNetwork.IsMasterClient)
        {
            // 목표 인원 수 채웠으면, 맵 이동을 한다. 권한은 마스터 클라이언트만.
            // PhotonNetwork.AutomaticallySyncScene = true; 를 해줬어야 방에 접속한 인원이 모두 이동함.
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