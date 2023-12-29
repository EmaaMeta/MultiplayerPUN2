using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    //public TMP_Text roomNameE;
    public Text roomName;
    LobbyManager manager;

    private void Start()
    {
        manager = FindObjectOfType<LobbyManager>(); //search scen for a looby manager attach to it
    }
    public void SetRoomName(string _roomName)
    { 
       // roomNameE.text = _roomName;
       roomName.text= _roomName;
    }

    public void OnclickItem()
    { 
       //manager.JoinRoom(roomNameE.text);
       manager.JoinRoom(roomName.text);
    }




}
