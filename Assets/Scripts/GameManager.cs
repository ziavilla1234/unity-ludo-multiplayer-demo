using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Multiplayer.Tools.NetStatsMonitor;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MovementControls Controls;
    public RuntimeNetStatsMonitor NetStatsMonitor;

    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("SignedIn...");
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public async Task<bool> HostRelayAsync()
    {
        if (await create_relay())
        {

            NetworkManager.Singleton.StartHost();
            this.gameObject.SetActive(false);
            GameManager.Instance.Controls.gameObject.SetActive(true);
            GameManager.Instance.NetStatsMonitor.gameObject.SetActive(true);
            Debug.Log("Sucessfully hosted relay...");
            return true;
        }
        return false;
    }
    public async Task<bool> JoinRelayAsync(string joincode)
    {
        if (await join_relay(joincode))
        {
            NetworkManager.Singleton.StartClient();
            this.gameObject.SetActive(false);
            GameManager.Instance.Controls.gameObject.SetActive(true);
            Debug.Log("Sucessfully joined relay...");
            return true;
        }
        return false;
    }
    private async Task<bool> create_relay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joincode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            RelayServerData rsd = new RelayServerData(allocation, "wss");//"dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(rsd);
            //NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port,
            //    allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);

            Debug.Log($"join code: {joincode}");


        }
        catch (RelayServiceException e)
        {
            Debug.Log($"error setting up relay: {e.Message}");
            return false;
        }
        return true;
    }

    private async Task<bool> join_relay(string joincode)
    {
        try
        {
            var alloc = await RelayService.Instance.JoinAllocationAsync(joincode);


            RelayServerData rsd = new RelayServerData(alloc, "wss");//"dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(rsd);

            //NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(alloc.RelayServer.IpV4, (ushort)alloc.RelayServer.Port,
            //    alloc.AllocationIdBytes, alloc.Key, alloc.ConnectionData, alloc.HostConnectionData);

        }
        catch (RelayServiceException e)
        {
            Debug.Log($"error joining relay: {e.Message}");
            return false;
        }
        return true;
    }
}
