using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MainMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject findOpponentPanel = null;
    [SerializeField] private GameObject WaitingStatusPanel = null;
    [SerializeField] private TextMeshProUGUI WaitingStatusText = null;

    [SerializeField] private TMP_InputField newRoomName;
    [SerializeField] private TMP_InputField customRoomName;

    //private bool isConnecting = false;
    private const string GameVesrion = "0.1"; 

    private const int maxPlayerPerRoom = 2;

    void Awake() 
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        if(!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.GameVersion = GameVesrion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void FindOpponent()
    {
        //This helps in finding random rooms.
        //isConnecting = true;

        findOpponentPanel.SetActive(false);
        WaitingStatusPanel.SetActive(true);

        WaitingStatusText.text = "Searching...";

        PhotonNetwork.JoinRandomRoom();
    }
    
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
    }

    public void CreateCustomRoom()
    {
        if(string.IsNullOrEmpty(newRoomName.text))
        {
            Debug.Log("Please enter a room name");
            return;
        }

        PhotonNetwork.CreateRoom(newRoomName.text, new RoomOptions { MaxPlayers = maxPlayerPerRoom });
    }

    public void JoinCustomRoom()
    {
        if (string.IsNullOrEmpty(customRoomName.text))
        {
            Debug.Log("Please enter a room name");
            return;
        }

        PhotonNetwork.JoinRoom(customRoomName.text);
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

        if(playerCount != maxPlayerPerRoom)
        {
            WaitingStatusText.text = "Waiting for opponents to join...";
            Debug.Log("Waiting for opps...");
        }
        else
        {
            WaitingStatusText.text = "Opponent Found";
            Debug.Log("Match ready to begin");
        }
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == maxPlayerPerRoom)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;

            Debug.Log("Match ready to begin");
            WaitingStatusText.text = "Oppenent found";

            PhotonNetwork.LoadLevel("Scene_Main");
        }
    }
}
