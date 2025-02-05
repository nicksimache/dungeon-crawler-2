using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Mirror;
using Steamworks;
using System.Linq;
using TMPro;

public class LobbyController : MonoBehaviour
{
    public static LobbyController Instance;

    public TMP_Text LobbyNameText;

    public GameObject PlayerListViewContent;
    public GameObject PlayerListItemPrefab;
    public GameObject LocalPlayerObject;

    public ulong CurrentLobbyID;
    public bool PlayerItemCreated = false;
    private List<PlayerListItem> playerListItems = new List<PlayerListItem>();
    public PlayerObjectController LocalPlayerController;

    private SteamNetworkManager manager;

    private SteamNetworkManager Manager{
        get{
            if(manager != null){
                return manager;
            }
            return manager = SteamNetworkManager.singleton as SteamNetworkManager;
        }
    }

    private void Awake(){
        if(Instance == null) Instance = this;
    }

    public void UpdateLobbyName(){
        CurrentLobbyID = Manager.GetComponent<SteamLobby>().CurrentLobbyID;
        LobbyNameText.text = SteamMatchmaking.GetLobbyData((new CSteamID(CurrentLobbyID)), "name");
    }

    public void UpdatePlayerList(){
        if(!PlayerItemCreated) CreateHostPlayerItem(); //host
        if(playerListItems.Count < Manager.connectedPlayers.Count) CreateClientPlayerItem();
        if(playerListItems.Count > Manager.connectedPlayers.Count) RemovePlayerItem();
        if(playerListItems.Count == Manager.connectedPlayers.Count) UpdatePlayerItem();
    }


    public void FindLocalPlayer(){
        LocalPlayerObject = GameObject.Find("LocalGamePlayer");
        LocalPlayerController = LocalPlayerObject.GetComponent<PlayerObjectController>();
    }

    public void CreateHostPlayerItem(){
        foreach(PlayerObjectController player in Manager.connectedPlayers){
            GameObject NewPlayerItem = Instantiate(PlayerListItemPrefab) as GameObject;
            PlayerListItem NewPlayerItemScript = NewPlayerItem.GetComponent<PlayerListItem>();

            NewPlayerItemScript.PlayerName = player.PlayerName;
            NewPlayerItemScript.ConnectionID = player.ConnectionID;
            NewPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
            NewPlayerItemScript.SetPLayerValues();


            NewPlayerItem.transform.SetParent(PlayerListViewContent.transform);
            NewPlayerItem.transform.localScale = Vector3.one;

            playerListItems.Add(NewPlayerItemScript);
        }
        PlayerItemCreated = true;
    }

    public void CreateClientPlayerItem(){
        foreach(PlayerObjectController player in Manager.connectedPlayers){
            if(!playerListItems.Any(b => b.ConnectionID == player.ConnectionID)){
                GameObject NewPlayerItem = Instantiate(PlayerListItemPrefab) as GameObject;
                PlayerListItem NewPlayerItemScript = NewPlayerItem.GetComponent<PlayerListItem>();

                NewPlayerItemScript.PlayerName = player.PlayerName;
                NewPlayerItemScript.ConnectionID = player.ConnectionID;
                NewPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
                NewPlayerItemScript.SetPLayerValues();


                NewPlayerItem.transform.SetParent(PlayerListViewContent.transform);
                NewPlayerItem.transform.localScale = Vector3.one;

                playerListItems.Add(NewPlayerItemScript);
            }
        }
    }

    public void UpdatePlayerItem(){
        foreach(PlayerObjectController player in Manager.connectedPlayers){
            foreach(PlayerListItem playerListItemScript in playerListItems){
                if(playerListItemScript.ConnectionID == player.ConnectionID){
                    playerListItemScript.PlayerName = player.PlayerName;
                    playerListItemScript.SetPLayerValues();
                }
            }
        }
    }

    public void RemovePlayerItem(){
        List<PlayerListItem> playerListItemToRemove = new List<PlayerListItem>();
        foreach(PlayerListItem pli in playerListItems){
            if(!Manager.connectedPlayers.Any(b => b.ConnectionID == pli.ConnectionID)){
                playerListItemToRemove.Add(pli);
            }
        }

        if(playerListItemToRemove.Count > 0){
            foreach(PlayerListItem pli in playerListItemToRemove){
                GameObject objectToRemove = pli.gameObject;
                playerListItems.Remove(pli);
                Destroy(objectToRemove);
                objectToRemove = null;

            }
        }


    }
    
}
