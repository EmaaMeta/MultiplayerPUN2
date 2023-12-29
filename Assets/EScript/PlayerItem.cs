using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using JetBrains.Annotations;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public Text playerName;
    //[Header("Current Player")]
    // Removed Image component
    Image backgroundImage;
    public Color highlightColor;



    public GameObject leftArrowButton;
    public GameObject rightArrowButton;

    

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    // Removed Image component and avatars array

    public GameObject[] characterPrefabs;

    public Player player; // Made player public
    GameObject characterInstance;

    private void Start()
    {
        backgroundImage = GetComponent<Image>();
    }



    public void SetPlayerInfo(Player _Player)
    {
        playerName.text = _Player.NickName;
        player = _Player;
        UpdatePlayerItem(player);
    }

    public void ApplyLocalChanges()
    {
        backgroundImage.color = highlightColor;
        leftArrowButton.SetActive(true); 
        rightArrowButton.SetActive(true);
    }

    public void OnClickLeftArrow()
    {
        // ... (existing code to update avatar index)

        // Re-instantiate character based on updated index
        int avatarIndex = (int)playerProperties["playerAvatar"];
        Destroy(characterInstance); // Destroy previous instance
        characterInstance = Instantiate(characterPrefabs[avatarIndex], transform);
        characterInstance.transform.parent = transform;
        // Adjust position and scale as needed
    }

    public void OnClickRightArrow()
    {
        // ... (similar logic as OnClickLeftArrow)
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }

    public void UpdatePlayerItem(Player player) // Made UpdatePlayerItem public
    {
        if (playerProperties.ContainsKey("playerAvatar"))
        {
            int avatarIndex = (int)playerProperties["playerAvatar"];
            if (characterInstance != null)
            {
                Destroy(characterInstance); // Destroy previous instance if exists
            }
            characterInstance = Instantiate(characterPrefabs[avatarIndex], transform);
            characterInstance.transform.parent = transform;
            // Adjust position and scale as needed
        }
        else
        {
            // Handle default character if no avatar is set
        }
    }
}
