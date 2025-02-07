using UnityEngine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Steamworks;

public class SteamNetworkManager : NetworkManager
{
    [SerializeField] private PlayerObjectController playerObjectControllerPrefab;
    public List<PlayerObjectController> connectedPlayers = new List<PlayerObjectController>();

    public List<PlayerObjectController> GetConnectedPlayers()
    {
        return connectedPlayers;
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().name == "GameLobby")
        {
            PlayerObjectController gamePlayerInstance = Instantiate(playerObjectControllerPrefab);

            gamePlayerInstance.ConnectionID = conn.connectionId;
            gamePlayerInstance.PlayerIDNumber = connectedPlayers.Count + 1;
            gamePlayerInstance.PlayerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.Instance.CurrentLobbyID, connectedPlayers.Count);

            NetworkServer.AddPlayerForConnection(conn, gamePlayerInstance.gameObject);

        }
    }

    public void StartGame(string SceneName)
    {
        ServerChangeScene(SceneName);
    }
}
