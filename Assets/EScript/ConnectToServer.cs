using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


//public class ConnectToServer : MonoBehaviour
public class ConnectToServer : MonoBehaviourPunCallbacks //special function called auto by pun to load scen from onConnected to master
{
    //public InputField usernameInput;
    public TMP_InputField usernameInput;
    public TMP_Text buttonText;

    public void OnclickConnect()
    {
        if (usernameInput.text.Length >= 1) //connect only if there is name enterd
        { 
            PhotonNetwork.NickName = usernameInput.text; //to display player user name later
            buttonText.text= "Connecting.."; //change caption fron connect to connecting
            PhotonNetwork.AutomaticallySyncScene = true; // to allow other player to play the game
            PhotonNetwork.ConnectUsingSettings(); // to connect to PUN server
        }
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("Lobby");
    }
}
