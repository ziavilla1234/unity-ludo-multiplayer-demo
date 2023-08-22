using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode.Transports;
using Unity.Networking.Transport.Relay;

public class Connectivity : MonoBehaviour
{
    public TMP_InputField JoinCodeInputField;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.Controls.gameObject.SetActive(false);
        GameManager.Instance.NetStatsMonitor.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public async void server_on_click()
    {
        //NetworkManager.Singleton.StartServer();
        //this.gameObject.SetActive(false);
        //await create_relay();
        //
        //GameManager.Instance.NetStatsMonitor.gameObject.SetActive(true);
    }

    public async void host_on_click() 
    {
        if(await create_relay())
        {
            
            NetworkManager.Singleton.StartHost();
            this.gameObject.SetActive(false);
            GameManager.Instance.Controls.gameObject.SetActive(true);
            GameManager.Instance.NetStatsMonitor.gameObject.SetActive(true);
        }
    }

    public async void client_on_click() 
    { 
        if(await join_relay(JoinCodeInputField.text))
        {
            NetworkManager.Singleton.StartClient();
            this.gameObject.SetActive(false);
            GameManager.Instance.Controls.gameObject.SetActive(true);
        }
    }

    private async Task<bool> create_relay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joincode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            JoinCodeInputField.text = joincode;


            RelayServerData rsd = new RelayServerData(allocation, "udp");//"dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(rsd);
            //NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port,
            //    allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);

            Debug.Log($"join code: {joincode}");

            
        }
        catch(RelayServiceException e)
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


            RelayServerData rsd = new RelayServerData(alloc, "udp");//"dtls");
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
