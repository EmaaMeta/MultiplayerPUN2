using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime; // to gave us extra extra multiplayer functions
using UnityEngine.UI;

//public class LobbyManager : MonoBehaviour
public class LobbyManager : MonoBehaviourPunCallbacks // if we need to do something once connect will change it to call back see below override 
{
    //public TMP_InputField roomInputField;
    public InputField roomInputField;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    //public TMP_Text roomName;
    public Text roomName;
    //-----------------------
    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contentObject; //to store object in the scroll view that will parent in our room
    //----------------------------
    public float timeBetweenUpdates = 1.5f;
    float nextUpdateTime;
    //--------------------------
    
    [Header("Player")]
    public List<PlayerItem> playerItemsList = new List<PlayerItem>(); // check emaaa
    public PlayerItem playerItemPrefab;
    public Transform playerItemParent;



    public GameObject playButton;

    private void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    private void Update()
    {
        //if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >=2) // so game wont start if number of player than 2
            if (PhotonNetwork.IsMasterClient)
        {
            playButton.SetActive(true);
        }
        else 
        {
            playButton.SetActive(false);
        }

    }
    public void OnclickCreate()
    { 
        if (roomInputField.text.Length >= 1) //check if there is name of room enterd 
        {
            PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() {  MaxPlayers=4, BroadcastPropsChangeToAll= true}); // create room
        }
    }


    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = " Room Name : " +  PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)// ro retrive name of all rooms
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
    }


    void UpdateRoomList(List<RoomInfo> list)
    {
        //1 To destroy all current group name in the scene
        foreach (RoomItem item in roomItemsList)
        { 
            Destroy(item.gameObject);
        }

        roomItemsList.Clear(); // after this list completly empty


        //2 to repopulate the scene with newly updated room items
        foreach (RoomInfo room in list)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
            newRoom.SetRoomName(room.Name);
           
            
            
            
            //roomItemsList.Add(newRoom);
        }
    
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom(roomName);
    }

    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }


    void UpdatePlayerList()
    {
        // Destroy any unused items
        foreach (PlayerItem item in playerItemsList)
        {
            Destroy(item.gameObject);
        }
        playerItemsList.Clear ();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        // Reuse existing items or create new ones as needed
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);


            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.ApplyLocalChanges();
            }

            playerItemsList.Add(newPlayerItem);

        }


    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public void OnclickPlayButton()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
