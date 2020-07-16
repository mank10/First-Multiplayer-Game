using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class MainMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject findOpponentPanel = null;
    [SerializeField] private GameObject playerNamePanel = null;
    [SerializeField] private GameObject WaitingStatusPanel = null;
    [SerializeField] private TextMeshProUGUI WaitingStatusText = null;
    [SerializeField] private TextMeshProUGUI PlayerNamesText = null;
    [SerializeField] private GameObject StartButton = null;

    [SerializeField] private GameObject newRoomPanel = null;
    [SerializeField] private GameObject customRoomPanel = null;
    [SerializeField] private TMP_InputField newRoomName;
    [SerializeField] private TMP_InputField customRoomName;

    private const string GameVesrion = "0.1"; 

    private const int maxPlayerPerRoom = 4;

    void Awake() 
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        if(!PhotonNetwork.IsConnected)
        {
            playerNamePanel.SetActive(false);
            PhotonNetwork.GameVersion = GameVesrion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void FindOpponent()
    {
        //This helps in finding random rooms.
        findOpponentPanel.SetActive(false);
        WaitingStatusPanel.SetActive(true);

        WaitingStatusText.text = "Searching...";

        PhotonNetwork.JoinRandomRoom();
    }
    
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        playerNamePanel.SetActive(true);
    }

    public void CreateCustomRoom()
    {
        if(string.IsNullOrEmpty(newRoomName.text))
        {
            Debug.Log("Please enter a room name");
            return;
        }

        PhotonNetwork.CreateRoom(newRoomName.text, new RoomOptions { MaxPlayers = maxPlayerPerRoom });

        newRoomPanel.SetActive(false);
        WaitingStatusPanel.SetActive(true);
        WaitingStatusText.text = "New Room created successfully.";
    }

    public void JoinCustomRoom()
    {
        if (string.IsNullOrEmpty(customRoomName.text))
        {
            Debug.Log("Please enter a room name");
            return;
        }

        PhotonNetwork.JoinRoom(customRoomName.text);

        customRoomPanel.SetActive(false);
        WaitingStatusPanel.SetActive(true);
        WaitingStatusText.text = "Room joined successfully.";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Creating room failed beacuse " + message);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        WaitingStatusPanel.SetActive(false);
        findOpponentPanel.SetActive(true);

        Debug.Log($"Disconnected due to: {cause}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No clients are waiting for an opponents.");

        PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = maxPlayerPerRoom});
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Client joined successfully.");

        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        if(playerCount < 2) //playerCount != maxPlayerPerRoom
        {
            WaitingStatusText.text = "Waiting for opponents to join...";
            PlayerNamesText.text += PhotonNetwork.NickName;
            Debug.Log("Waiting for opps...");
        }
        else
        {
            WaitingStatusText.text = "Opponent Found";
            Debug.Log("Match ready to begin");
            PlayerNamesText.text += PhotonNetwork.NickName;
            if(PhotonNetwork.MasterClient == PhotonNetwork.LocalPlayer)
                StartButton.SetActive(true);
        }
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        
        if(PhotonNetwork.CurrentRoom.PlayerCount >= 2)  //PhotonNetwork.CurrentRoom.PlayerCount == maxPlayerPerRoom
        {
            //PhotonNetwork.CurrentRoom.IsOpen = false;
            if(PhotonNetwork.CurrentRoom.PlayerCount == maxPlayerPerRoom)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }

            Debug.Log("Match ready to begin");
            WaitingStatusText.text = "Oppenent found";
            PlayerNamesText.text += "\n" + player.NickName;

            if (PhotonNetwork.MasterClient == PhotonNetwork.LocalPlayer)
                StartButton.SetActive(true);
            //PhotonNetwork.LoadLevel("Scene_Main");
        }
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Scene_Main");
    }
}
