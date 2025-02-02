using UnityEngine;
using Unity.Netcode;

public class NetworkEventHandler : MonoBehaviour
{

    [SerializeField] private GameObject cameraToDestroy;

    private void Start(){
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
    }

    private void OnServerStarted(){
        if (NetworkManager.Singleton.IsHost)
        {
            Debug.Log("🟢 Host started! (Server + Client)");

            Destroy(cameraToDestroy);
        }
        else
        {
            Debug.Log("🟡 Dedicated Server started!");
        }
    }
    
}
