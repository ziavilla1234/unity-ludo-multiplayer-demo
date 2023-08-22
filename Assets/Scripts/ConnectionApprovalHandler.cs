using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ConnectionApprovalHandler : MonoBehaviour
{
    


    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += evt_connection_approval;
    }

    private void evt_connection_approval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        var client_id = request.ClientNetworkId;

        response.Approved = true;

        response.CreatePlayerObject = true;
        response.PlayerPrefabHash = null;

        Debug.Log($"No of clients: {NetworkManager.Singleton.ConnectedClients.Count}");

        if (NetworkManager.Singleton.ConnectedClients.Count >= 3)
        {
            response.Approved = false;
            response.Reason = "Max player count reached.";
        }
        
        response.Pending = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
